using System;

namespace TimeDepositAPI.Models
{
    public class CrowdDepositOffer
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }      // Total amount or threshold as needed
        public double APY { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public bool IsActive { get; set; }
    }
}
