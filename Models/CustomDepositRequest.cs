using System;

namespace TimeDepositAPI.Models
{
    public enum CustomDepositStatus
    {
        Pending,
        Approved,
        Rejected,
    }
    public class CustomDepositRequest
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int DurationInDays { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
        public CustomDepositStatus Status { get; set; }
    }
}