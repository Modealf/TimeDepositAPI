using TimeDepositAPI.Data;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Models;

namespace TimeDepositAPI.Services;

public interface IAdminService
{
    public Task<string> CreateCrowdDepositOfferAsync(CreateCrowdDepositOfferDto createCrowdDepositOfferDto);
    public Task<string> UpdateCrowdDepositOfferAsync(UpdateCrowdDepositOfferDto updateCrowdDepositOfferDto);
    public Task<string> DeleteCrowdDepositOfferAsync(DeleteCrowdDepositOfferDto deleteCrowdDepositOfferDto);

}

public class AdminService : IAdminService
{
    private readonly AppDbContext _context;

    public AdminService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateCrowdDepositOfferAsync(CreateCrowdDepositOfferDto createCrowdDepositOfferDto)
    {
        var crowdDepositOffer = new CrowdDepositOffer{
            Amount = createCrowdDepositOfferDto.Amount,
            APY = createCrowdDepositOfferDto.APY,
            StartDate = createCrowdDepositOfferDto.StartDate,
            MaturityDate = createCrowdDepositOfferDto.MaturityDate,
            Status = Status.Waiting
    };
        
        _context.CrowdDepositOffers.Add(crowdDepositOffer);
        await _context.SaveChangesAsync();
        return "Crowd deposit offer created";
    }

    // status should automatically change based on dates
    public async Task<string> UpdateCrowdDepositOfferAsync(UpdateCrowdDepositOfferDto updateCrowdDepositOfferDto)
    {
        
        var crowdDepositOffer = _context.CrowdDepositOffers.Find(updateCrowdDepositOfferDto.Id);
        if (crowdDepositOffer == null)
            throw new BadHttpRequestException("Crowd deposit offer not found");
        crowdDepositOffer.Amount = updateCrowdDepositOfferDto.Amount;
        crowdDepositOffer.APY = updateCrowdDepositOfferDto.APY;
        crowdDepositOffer.StartDate = updateCrowdDepositOfferDto.StartDate;
        crowdDepositOffer.MaturityDate = updateCrowdDepositOfferDto.MaturityDate;
        await _context.SaveChangesAsync();
        return "Crowd deposit offer updated";
    }

    public async Task<string> DeleteCrowdDepositOfferAsync(DeleteCrowdDepositOfferDto deleteCrowdDepositOfferDto)
    {
        var crowdDepositOffer = _context.CrowdDepositOffers.Find(deleteCrowdDepositOfferDto.Id);
        if (crowdDepositOffer == null)
            throw new BadHttpRequestException("Crowd deposit offer not found");
        crowdDepositOffer.Status = Status.Deleted;
        await _context.SaveChangesAsync();
        return "Crowd deposit offer successfully deleted";
    }
}
