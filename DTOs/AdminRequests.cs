using TimeDepositAPI.Models;

namespace TimeDepositAPI.DTOs
{

        public class CreateCrowdDepositOfferDto
        {
                public decimal Amount { get; set; }
                public Period Period { get; set; }
                public DateTime StartDate { get; set; }
                public DateTime MaturityDate { get; set; }
                public double APY { get; set; }       
        }

        public class UpdateCrowdDepositOfferDto
        {
                public int Id { get; set; }
                public decimal Amount { get; set; }
                public DateTime StartDate { get; set; }
                public DateTime MaturityDate { get; set; }
                public double APY { get; set; }
        }

        public class DeleteCrowdDepositOfferDto
        {
                public int Id { get; set; }
        }

        public class ApproveCustomDepositRequestDto
        {
                public int DepositId { get; set; }
                public DateTime StartDate { get; set; }
                public DateTime MaturityDate { get; set; }
                public double APY { get; set; }
        }

        public class RejectCustomDepositRequestDto
        {
                public int DepositId { get; set; }
        }
}