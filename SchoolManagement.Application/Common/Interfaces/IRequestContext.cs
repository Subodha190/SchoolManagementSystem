namespace SchoolManagement.Application.Common.Interfaces
{
    public interface IRequestContext
    {
        int? UserId { get; set; }
        string UserName { get; set; }
        int SchoolId { get; set; }
        string Role { get; set; }
        string IPAddress { get; set; }
        string UserAgent { get; set; }
        string TraceId { get; set; }
        string CorrelationId { get; set; }
    }
}
