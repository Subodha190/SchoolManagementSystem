using SchoolManagement.Application.Common.Interfaces;

namespace SchoolManagement.Infrastructure.Request
{
    public class RequestContext : IRequestContext
    {
        public int? UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public int SchoolId { get; set; }

        public string Role { get; set; } = string.Empty;

        public string IPAddress { get; set; } = string.Empty;

        public string UserAgent { get; set; } = string.Empty;

        public string TraceId { get; set; } = string.Empty;

        public string CorrelationId { get; set; } = string.Empty;
    }
}
