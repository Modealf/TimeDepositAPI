namespace TimeDepositAPI.DTOs
{

        public class CreateCrowdDepositOfferDto
        {
                public decimal Amount { get; set; }
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
}