namespace TimeDepositAPI.DTOs
{
    public class CreateCustomDepositRequestDto
    {
        public decimal Amount { get; set; }
        public int DurationInDays { get; set; }  // Or use a specific type if needed
    }

    public class EnrollCrowdDepositRequestDto
    {
        public int OfferId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? RolloverUntilDate { get; set; }
    }
    
    // You can add other DTOs as needed for update, rollover, etc.
}