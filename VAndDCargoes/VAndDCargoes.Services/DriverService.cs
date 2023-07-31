using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Driver;

namespace VAndDCargoes.Services;

public class DriverService : IDriverService
{
    private readonly VAndDCargoesDbContext dbContext;

    public DriverService(VAndDCargoesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task BecomeDriverAsync(BecomeDriverViewModel model, string userId)
    {
        Driver driver = new Driver()
        {
            FirstName = model.FirstName,
            SecondName = model.SecondName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            Age = model.Age,
            UserId = Guid.Parse(userId)
        };

        await this.dbContext.Drivers.AddAsync(driver);
        await this.dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsTheUserAlreadyDriver(string userId)
    {
        if (await this.dbContext.Drivers.AnyAsync(x => x.UserId.ToString().Equals(userId)))
        {
            return true;
        }

        return false;
    }
}

