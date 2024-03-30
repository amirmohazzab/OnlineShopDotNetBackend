
namespace Application.Interfaces;

public interface IPermissionService
{
    Task<bool> CheckPermission(Guid UserId, string permissionFlag);


}