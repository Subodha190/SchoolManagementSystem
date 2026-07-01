namespace SchoolManagement.Application.Common.Attributes
{
    // Marker attribute used for documentation and tooling in the application layer.
    // The actual runtime attribute used on controllers is provided in the API project
    // and maps to the dynamic authorization policy.
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method, AllowMultiple = true)]
    public class HasPermissionAttribute : System.Attribute
    {
        public string PermissionKey { get; }

        public HasPermissionAttribute(string permissionKey)
        {
            PermissionKey = permissionKey;
        }
    }
}
