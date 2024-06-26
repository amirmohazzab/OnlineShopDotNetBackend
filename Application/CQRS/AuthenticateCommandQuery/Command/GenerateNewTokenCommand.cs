using Application.CQRS.Notifications;
using Core;
using Infrastructure.Models;
using Infrastructure.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.CQRS.AuthenticateCommandQuery;

public class GenerateNewTokenCommand : IRequest<GenerateNewTokenCommandResponse>
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

}

public class GenerateNewTokenCommandResponse
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

}

public class GenerateNewTokenCommandHandler : IRequestHandler<GenerateNewTokenCommand, GenerateNewTokenCommandResponse>
{
    private readonly OnlineShopDbContext onlineShopDbContext;
    private readonly EncryptionUtility encryptionUtility;
    private readonly IMediator mediator;
    private readonly Configs configs;

    public GenerateNewTokenCommandHandler(OnlineShopDbContext onlineShopDbContext, EncryptionUtility encryptionUtility, 
                                            IOptions<Configs> options, IMediator mediator)
    {
        this.onlineShopDbContext = onlineShopDbContext;
        this.encryptionUtility = encryptionUtility;
        this.mediator = mediator;
        configs = options.Value;
    }
    public async Task<GenerateNewTokenCommandResponse> Handle(GenerateNewTokenCommand request, CancellationToken cancellationToken)
    {

        var userRefreshToken = await onlineShopDbContext.UserRefreshTokens.SingleOrDefaultAsync(q => q.RefreshToken == request.RefreshToken);

        if(userRefreshToken == null) throw new Exception();

        var token = encryptionUtility.GetNewToken(userRefreshToken.UserId);
        var refreshToken = encryptionUtility.GetNewRefreshToken();

        var addRefreshTokenNotification = new AddRefreshTokenNotification
        {
            RefreshToken = refreshToken,
            RefreshTokenTimeout = configs.RefreshTokenTimeout,
            UserId = userRefreshToken.UserId,
        };

        await mediator.Publish(addRefreshTokenNotification);

        var response = new GenerateNewTokenCommandResponse 
        {
            RefreshToken = refreshToken,
            Token = token
        };

        return response;
    }
}

