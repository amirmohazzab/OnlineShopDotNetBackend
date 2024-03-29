
using AutoMapper;
using Core;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Notifications;

public class AddRefreshTokenNotification : INotification
{
    
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; }

    public int RefreshTokenTimeout { get; set; }
    public int RefreshTokenTimeOut { get; internal set; }
}

public class AddRefreshTokenNotificationHandler : INotificationHandler<AddRefreshTokenNotification>
{
    private readonly OnlineShopDbContext onlineShopDbContext;
    private readonly IMapper mapper;

    public AddRefreshTokenNotificationHandler(OnlineShopDbContext onlineShopDbContext, IMapper mapper)
    {
        this.onlineShopDbContext = onlineShopDbContext;
        this.mapper = mapper;
    }
    public async Task Handle(AddRefreshTokenNotification notification, CancellationToken cancellationToken)
    {
        var userRefreshToken = mapper.Map<UserRefreshToken>(notification);
       
       var currentRefreshToken = await onlineShopDbContext.UserRefreshTokens
       .SingleOrDefaultAsync(q => q.UserId == notification.UserId);

       if(currentRefreshToken == null) 
       { 
            await onlineShopDbContext.AddAsync(userRefreshToken);
       } 
       else 
       { 
            currentRefreshToken.RefreshToken = userRefreshToken.RefreshToken;
            currentRefreshToken.RefreshTokenTimeout = userRefreshToken.RefreshTokenTimeout;
            currentRefreshToken.CreateDate = userRefreshToken.CreateDate;
            currentRefreshToken.IsValid = true;
       }

       await onlineShopDbContext.SaveChangesAsync();
    }
}
