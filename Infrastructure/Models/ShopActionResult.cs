
namespace Infrastructure.Models;

public class ShopActionResult<T>
{

    public bool isSuccess { get; set; }

    public string Message { get; set; }
    public T Data { get; set; }

    public int Total { get; set; }

    public int Page { get; set; }

    public int PageCount { 
        get {
            if (Total == 0) return 0;
            return Convert.ToInt32(Math.Ceiling(Total/(double)Size));
        } 
    }

    public int Size { get; set; }

}