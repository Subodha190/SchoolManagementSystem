using System;

namespace SchoolManagement.Application.Common.Models
{
    public class CreateFeePaymentDto
    {
        public int StudentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }

    public class UpdateFeePaymentDto : CreateFeePaymentDto
    {
        public int Id { get; set; }
    }

    public class FeePaymentResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
