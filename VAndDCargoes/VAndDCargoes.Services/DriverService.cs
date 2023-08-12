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

    public async Task<decimal> GetDriverBalance(string userId)
    {
        Driver? driver = await this.dbContext.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            return Math.Round(driver.Ballance, 2);
        }

        return 0m;
    }

    public async Task<string> GetTheFullNameOfDriverByUserIdAsync(string userId)
    {
        Driver? driver = await this.dbContext.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            return $"{driver.FirstName} {driver.SecondName} {driver.LastName}";
        }

        return string.Empty;
    }

    public async Task<bool> IsTheUseAlreadyDriverByIdAsync(string userId)
    {
        if (await this.dbContext.Drivers.FirstOrDefaultAsync(x => x.UserId.ToString() == userId) != null)
        {
            return true;
        }

        return false;
    }
}

