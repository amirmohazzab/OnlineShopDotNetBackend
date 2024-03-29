using AutoMapper.Internal.Mappers;
using Core;
using Core.Entities;
using Infrastructure.Dto;
using Infrastructure.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;


namespace Application.CQRS.AuthenticateCommandQuery.Command
{
    public class RegisterCommand:IRequest<int>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
    {
        private readonly OnlineShopDbContext onlineShopDbContext;
        private readonly EncryptionUtility encryptionUtility;

        public RegisterCommandHandler(OnlineShopDbContext onlineShopDbContext, EncryptionUtility encryptionUtility)
        {
            this.onlineShopDbContext = onlineShopDbContext;
            this.encryptionUtility = encryptionUtility;
        }
        //public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var salt = encryptionUtility.GetNewSalt();
            var hashPassowrd = encryptionUtility.GetSHA256(request.Password, salt);

            var user = new User
            {
                Id= Guid.NewGuid(),
                Password = hashPassowrd,
                PasswordSalt = salt,
                RegisterDate= DateTime.Now,
                UserName = request.UserName
            };

            await onlineShopDbContext.Users.AddAsync(user);
            var result = await onlineShopDbContext.SaveChangesAsync();

            //return Unit.Value;

            return result;
        }

    }
}
