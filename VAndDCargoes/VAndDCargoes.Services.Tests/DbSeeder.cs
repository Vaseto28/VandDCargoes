using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Web.ViewModels.Driver;
using VAndDCargoes.Web.ViewModels.Truck;

namespace VAndDCargoes.Services.Tests;

public static class DbSeeder
{
    public static ApplicationUser DriverUser;
    public static ApplicationUser User;
    public static Driver Driver;

	public static TruckAddViewModel TruckAddViewModel;

	public static ApplicationUser UserBecomingDriver;
	public static BecomeDriverViewModel DriverToAdd;

	public static Truck Truck;

	public static void SeedDb(VAndDCargoesDbContext dbCtx)
	{
		DriverUser = new ApplicationUser()
		{
			UserName = "Pesho",
			NormalizedUserName = "PESHO",
			Email = "pesho@abv.bg",
			NormalizedEmail = "PESHO@ABV.BG",
			EmailConfirmed = true,
			PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
			SecurityStamp = "3b0b75d4-d21b-465d-a738-27d6b583da1c",
			ConcurrencyStamp = "35f4c6c1-1851-42ea-a68f-e15f1b2c7b37",
			TwoFactorEnabled = false
		};
		User = new ApplicationUser()
        {
            UserName = "Gosho",
            NormalizedUserName = "GOSHO",
            Email = "gosho@abv.bg",
            NormalizedEmail = "GOSHO@ABV.BG",
            EmailConfirmed = true,
            PasswordHash = "481f6cc0511143ccdd7e2d1b1b94faf0a700a8b49cd13922a70b5ae28acaa8c5",
            SecurityStamp = "ce88a08a-9bc5-4277-b972-1cb87cde7f4b",
            ConcurrencyStamp = "380cfe84-f9f3-4b89-83de-0893fc40f248",
            TwoFactorEnabled = false
        };
		Driver = new Driver()
		{
			FirstName = "Petar",
			SecondName = "Magdalenov",
			LastName = "Ivanov",
			Age = 34,
			PhoneNumber = "0899872653",
			User = DriverUser
		};
        Truck = new Truck()
        {
            Make = "Iveco",
            Model = "XCC",
            RegistrationNumber = "P 2346 EH",
            Condition = (TruckCondition)2,
            FuelCapacity = 800,
            TraveledDistance = 1000,
            ImageUrl = "https://www.daf.co.uk/-/media/images/daf-trucks/trucks/euro-6/daf-xf/2017035-daf-xf.jpg?mw=1200&rev=e21fa83ff62d405b9db95e6914c636e1&hash=5102BA118103505BCA802B2D812C22BD",
            CreatedOn = DateTime.Now,
            CreatorId = new Guid("2f546a89-be24-4111-bff8-769bbfe1132a")
        };

        UserBecomingDriver = new ApplicationUser()
        {
            UserName = "Angel",
            NormalizedUserName = "ANGEL",
            Email = "angel@abv.bg",
            NormalizedEmail = "ANGEL@ABV.BG",
            EmailConfirmed = true,
            PasswordHash = "8d969eef6ecad3c29a3a629120e686cf0c3f5d5a86aff3ca12020c923adc6c92",
            SecurityStamp = "1b0b75d4-d21b-465d-a738-27d6b583da1c",
            ConcurrencyStamp = "25f4c6c1-1851-42ea-a68f-e15f1b2c7b37",
            TwoFactorEnabled = false
        };
        DriverToAdd = new BecomeDriverViewModel()
        {
            FirstName = "Angel",
            SecondName = "Ivanov",
            LastName = "Atanasov",
            PhoneNumber = "0987635423",
            Age = 25
        };

        TruckAddViewModel = new TruckAddViewModel()
		{
			Make = "Daf",
			Model = "XF-23",
			RegistrationNumber = "PK 2876 TH",
			Condition = 2,
			FuelCapacity = 800,
			TravelledDistance = 1000,
			ImageUrl = "https://www.daf.co.uk/-/media/images/daf-trucks/trucks/euro-6/daf-xf/2017035-daf-xf.jpg?mw=1200&rev=e21fa83ff62d405b9db95e6914c636e1&hash=5102BA118103505BCA802B2D812C22BD",
			CreatedOn = "12/23/2009"
		};

		dbCtx.Users.Add(DriverUser);
		dbCtx.Users.Add(User);
        dbCtx.Trucks.Add(Truck);

		dbCtx.Drivers.Add(Driver);

		dbCtx.SaveChanges();
    }
}

