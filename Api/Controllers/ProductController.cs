using System.Security.Cryptography;
using Api.CustomAttribute;
using Infrastructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Profiling;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/[Controller]")]
[EnableCors("MyApi")]
//[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService productService;
    //private readonly IHubContext hubContext;

    public ProductController(IProductService ProductService)
    {
        productService = ProductService;
        //this.hubContext = hubContext;
    }

    [HttpGet("{Id}")]
    [SwaggerOperation(
        Summary = "Get a Product",
        Description = "Get a Product with Id",
        OperationId = "Products.Get",
        Tags = new[] {"ProductController"})
    ]
    public async Task<IActionResult> Get(int Id) {

        var result = await productService.Get(Id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int page=1, int size=3) {

        using (MiniProfiler.Current.Step("pRODUCT Get All Method")) 
        {
            //await hubContext.Clients.All.SendAsync("Notify", $"Call Product GetAll at: {DateTime.Now}");
            var result = await productService.GetAll(page, size);
            return Ok(result);
        }
        
    }

    [HttpPost]
    //[AccessControl(Permission = "product-add")]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromForm] ProductDto model) {

        var result = await productService.Add(model);
        return Ok(result);
    }


    [HttpGet("GetFileContent")]
    [AllowAnonymous]
    public async Task<FileContentResult> GetFileContent(string fileUrl)
    {
        var urlSections = fileUrl.Split("/");
        //read file and decrypt content
        byte[] encryptedData = await System.IO.File.ReadAllBytesAsync("");
        var decryptedData = Decrypt(encryptedData);

        return new FileContentResult(decryptedData, "application/txt");
    }

    [HttpPost("upload")]
    [AllowAnonymous]
    public async Task<IActionResult> Upload(IFormFile thumbnail) 
    {

        //save to byte[]
        using (var target = new MemoryStream())
        {
            thumbnail.CopyTo(target);
            var ThumbnailByteArray = target.ToArray();
        }

         //2-save in folders
        string filePath = @"C:\";
        FileInfo fileInfo = new FileInfo(thumbnail.FileName);
        string fileName = thumbnail.FileName + fileInfo.Extension;

        string fileNameWithPath = Path.Combine(filePath, fileName);

        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
        {
            thumbnail.CopyTo(stream);
        }

        return Ok();

    }


    private byte[] Encrypt(byte[] fileContent)
    {
        string EncryptionKey = "MAKV2SPBNI54324";
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

    private byte[] Decrypt(byte[] fileContent)
    {
        string EncryptionKey = "MAKV2SPBNI54324";
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
    
}