using Microsoft.AspNetCore.Authorization;

namespace SchoolManagement.API.Attributes
{
    // Runtime attribute used on controllers to map to dynamic permission policies
    public class HasPermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public HasPermissionAuthorizeAttribute(string permissionKey)
        {
            Policy = $"Permission:{permissionKey}";
        }
    }
}
