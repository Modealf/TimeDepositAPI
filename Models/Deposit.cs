using System;

namespace TimeDepositAPI.Models
{
    public enum DepositType
    {
        Custom,
        Crowd
    }
    
    public enum Period
    {
        None,
        Daily,
        Weekly,
        Monthly,
    }
    

    public class Deposit
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DepositType Type { get; set; }
        public Period Period { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public double APY { get; set; }

        // Replace IsRolledOver with RolloverUntil
        public DateTime? RolloverUntil { get; set; }

        // Foreign key and navigation property for User
        public int UserId { get; set; }
        public required User User { get; set; }
    }
}
