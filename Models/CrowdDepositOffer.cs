using System;

namespace TimeDepositAPI.Models
{
    public enum Status{
    Waiting,
    InProgress,
    Completed,
    Canceled,
    Deleted,
    }
    
    public class CrowdDepositOffer
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }      // Total amount or threshold as needed
        public double APY { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public Status Status { get; set; }
    }
}
