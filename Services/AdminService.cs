using Microsoft.EntityFrameworkCore;
using TimeDepositAPI.Data;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Models;

namespace TimeDepositAPI.Services;

public interface IAdminService
{
    public Task<string> CreateCrowdDepositOfferAsync(CreateCrowdDepositOfferDto createCrowdDepositOfferDto);
    public Task<string> UpdateCrowdDepositOfferAsync(UpdateCrowdDepositOfferDto updateCrowdDepositOfferDto);
    public Task<string> DeleteCrowdDepositOfferAsync(DeleteCrowdDepositOfferDto deleteCrowdDepositOfferDto);
    public Task<string> ApproveCustomDepositAsync(ApproveCustomDepositRequestDto request);
    public Task<string> RejectCustomDepositAsync(RejectCustomDepositRequestDto request);
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
            Period = createCrowdDepositOfferDto.Period,
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
    
    public async Task<string> ApproveCustomDepositAsync(ApproveCustomDepositRequestDto approveCustomDepositRequestDto)
    {
        CustomDepositRequest? customDepositRequest = await _context.CustomDepositRequests.FirstOrDefaultAsync(c => c.Id == approveCustomDepositRequestDto.DepositId);
        if (customDepositRequest is null)
            throw new BadHttpRequestException("Custom deposit request not found");

        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == customDepositRequest.UserId);
        if (user is null)
            throw new BadHttpRequestException("No User matching this deposit request has been found");
            
        Deposit deposit = new Deposit
        {
            UserId = customDepositRequest.UserId,
            Amount = customDepositRequest.Amount,
            Type = DepositType.Custom,
            StartDate = approveCustomDepositRequestDto.StartDate,
            MaturityDate = approveCustomDepositRequestDto.MaturityDate,
            APY = approveCustomDepositRequestDto.APY,
            User = user
        };
        await _context.Deposits.AddAsync(deposit);
        await _context.SaveChangesAsync();
            
        return "Custom deposit request has been approved";
    }

    public async Task<string> RejectCustomDepositAsync(RejectCustomDepositRequestDto request)
    {
        var customDepositRequest = await _context.CustomDepositRequests.FirstOrDefaultAsync(c => c.Id == request.DepositId);
        if (customDepositRequest is null)
            throw new BadHttpRequestException("Custom deposit request not found");
        customDepositRequest.Status = CustomDepositStatus.Rejected;
        await _context.SaveChangesAsync();
        return "Custom deposit request has been rejected";
    }
}
