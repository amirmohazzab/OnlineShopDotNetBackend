using Core;
using Core.Entities;
using Core.IRepositories;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.CQRS.ProductCommandQuery.Command;

    public class SaveProductCommand : IRequest<SaveProductCommandResponse>
    {
        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }
    }

    public class SaveProductCommandResponse
    {
        public int ProductId { get; set; }
    }

public class SaveProductCommandHandler : IRequestHandler<SaveProductCommand, SaveProductCommandResponse>
{
    

    // private readonly OnlineShopDbContext onlineShopDbContext;

    // public SaveProductCommandHandler(OnlineShopDbContext onlineShopDbContext)
    // {
    //     this.onlineShopDbContext = onlineShopDbContext;
    // }

    private readonly IProductRepository productRepository;
    private readonly IUnitOfWork unitOfWork;

    public SaveProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        this.productRepository = productRepository;
        this.unitOfWork = unitOfWork;
    }
    public async Task<SaveProductCommandResponse> Handle(SaveProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product 
        {
            ProductName = request.ProductName,
            Price = request.Price,
        };
        
        // await onlineShopDbContext.Products.AddAsync(product);
        // await onlineShopDbContext.SaveChangesAsync();

        await productRepository.InsertAsync(product);
        await unitOfWork.SaveChangesAsync();

        var response = new SaveProductCommandResponse 
        {
            ProductId = product.Id
        };

        return response;
    }
}