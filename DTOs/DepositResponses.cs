using TimeDepositAPI.Models;

namespace TimeDepositAPI.DTOs
{
    public class DepositDetailsResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public double APY { get; set; }
        public DateTime? RolloverUntil { get; set; }
    }
    
    public class GetAvailableCrowdDepositsResponseDto{
    public List<CrowdDepositOffer> CrowdDepositOffers { get; set; }
    }
}