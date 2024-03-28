
using Infrastructure.Dto;

public interface IProductService 
{
    Task<List<ProductDto>> GetAll(); 

    Task<ProductDto> Get(int Id);

    Task<ProductDto> Add(ProductDto model);
}