using System.Threading.Tasks;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> UserHasPermissionAsync(int userId, string permissionKey);
    }
}
