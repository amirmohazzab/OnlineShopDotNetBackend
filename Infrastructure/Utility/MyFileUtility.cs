

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Infrastructure.Utility;

public class MyFileUtility
{
    private readonly IWebHostEnvironment environment;
    private readonly IConfiguration configuration;
    private readonly IHttpContextAccessor httpContextAccessor;

    public MyFileUtility(IWebHostEnvironment environment, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        this.environment = environment;
        this.configuration = configuration;
        this.httpContextAccessor = httpContextAccessor;
    }


    public string GetFileFullPath(string fileName, string enityName)
    {
        var appRootPath = environment.WebRootPath;
        var mediaRootPath = configuration.GetValue<string>("MediaPath");

        return Path.Combine(appRootPath, mediaRootPath, enityName, fileName);
    }
    public string SaveFileInFolder(IFormFile file, string entityName, bool isEncrypt = false)
    {
        var applicationExecutionRootPath = environment.WebRootPath;
        var mediaRootPath = configuration.GetValue<string>("MediaPath");
        var folderRootPath = entityName;

        CheckAndCreatePathDirectory(applicationExecutionRootPath, mediaRootPath, folderRootPath);
        var newFileName = $"{DateTime.Now.Ticks.ToString()}{GetFileExtension(file.FileName)}";

        var newFilePath = Path.Combine(applicationExecutionRootPath, mediaRootPath, folderRootPath, newFileName);
        var byteArray = ConvertToByteArray(file);

        if (isEncrypt) 
        {
            byteArray = EncryptFile(byteArray);
        }
        
        using var writer = new BinaryWriter(System.IO.File.OpenWrite(newFilePath));
        writer.Write(byteArray);

        return newFileName;
    }


    private string GetEntityFolderUrl(string host, string entityName, bool isHttps)
    {
        //var applicationExecutionRootPath = environment.WebRootPath;
        var mediaRootPath = configuration.GetValue<string>("MediaPath").Replace("\\", "/");
        // var folderRootPath = entityName;
        // return Path.Combine(applicationExecutionRootPath, mediaRootPath, folderRootPath);
        var httpMode = isHttps ? "Https" : "Http";
        return $"{httpMode}://{host}/{mediaRootPath}/{entityName}";
    }


    private void CheckAndCreatePathDirectory(string appRootPath, string mediaRootPath, string entityFolderName)
    {
        var mediaFullPath = Path.Combine(appRootPath, mediaRootPath);
        if (!Directory.Exists(mediaFullPath))
        {
            Directory.CreateDirectory(mediaFullPath);
        }

        var entityFolderFullPath = Path.Combine(mediaFullPath, entityFolderName);
        if (!Directory.Exists(entityFolderFullPath))
       {
            Directory.CreateDirectory(entityFolderFullPath);
       }
    }
    public byte[] ConvertToByteArray(IFormFile file)
    {
        using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            return ms.ToArray();
        }
    }


    public string GetFileExtension(string fileName)
    {
        var fileInfo = new FileInfo(fileName);
        return fileInfo.Extension;
    }

    public string ConvertToBase64(byte[] data)
    {
        return Convert.ToBase64String(data);
    }

    public string GetFileUrl(string thumbnailFileName, string entityName)
    {
        var hostUrl = httpContextAccessor.HttpContext.Request.Host.Value;
        var isHttps = httpContextAccessor.HttpContext.Request.IsHttps;
        var folderPath = GetEntityFolderUrl(hostUrl, entityName, isHttps);

        return $"{folderPath}/{thumbnailFileName}";
    }

    
    public byte[] EncryptFile(byte[] fileContent)
        {
            string EncryptionKey = configuration.GetValue<string>("FileEncryptionKey");
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(fileContent, 0, fileContent.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public byte[] DecryptFile(byte[] fileContent)
        {
            string EncryptionKey = configuration.GetValue<string>("FileEncryptionKey");
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);


                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(fileContent, 0, fileContent.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }

    public string GetEncryptedFileActionUrl(string thumbnailFileName, string entityName)
    {
        var hostUrl = httpContextAccessor.HttpContext.Request.Host.Value;
        var isHttps = httpContextAccessor.HttpContext.Request.IsHttps;
        var httpMode = isHttps ? "https" : "http";

        return $"{httpMode}://{hostUrl}/Media/{entityName}/{thumbnailFileName}";
    }
}

