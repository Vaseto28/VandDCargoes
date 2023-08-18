using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Web.ViewModels.Cargo;
using VAndDCargoes.Web.ViewModels.Driver;
using VAndDCargoes.Web.ViewModels.Truck;
using VAndDCargoes.Web.ViewModels.Trailer;
using VAndDCargoes.Web.ViewModels.Course;

namespace VAndDCargoes.Services.Tests;

public static class DbSeeder
{
    public static ApplicationUser DriverUser;
    public static ApplicationUser User;
    public static Driver Driver;

    public static Truck Truck;
    public static TruckAddViewModel TruckAddViewModel;
    public static TruckEditViewModel TruckEditViewModel;
    public static Truck TruckLowCondition;

    public static ApplicationUser UserBecomingDriver;
    public static BecomeDriverViewModel DriverToAdd;

    public static Cargo Cargo;
    public static CargoAddViewModel CargoAddViewModel;
    public static CargoEditViewModel CargoEditViewModel;

    public static Trailer Trailer;
    public static TrailerAddViewModel TrailerAddViewModel;
    public static TrailerEditViewModel TrailerEditViewModel;

    public static Course Course;
    public static StartCourseViewModel InvalidStartCourseViewModel;
    public static StartCourseViewModel StartCourseViewModel;
    public static StartCourseViewModel LowConditionStartCourseViewModel;

    public static DriversCargoes DriversCargoes;

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
        TruckEditViewModel = new TruckEditViewModel()
        {
            Make = "Daf",
            Model = "XF-23",
            RegistrationNumber = "PK 2876 TH",
            Condition = 3,
            FuelCapacity = 800,
            TravelledDistance = 1000,
            ImageUrl = "https://www.daf.co.uk/-/media/images/daf-trucks/trucks/euro-6/daf-xf/2017035-daf-xf.jpg?mw=1200&rev=e21fa83ff62d405b9db95e6914c636e1&hash=5102BA118103505BCA802B2D812C22BD",
            CreatedOn = "11/23/2009"
        };

        CargoAddViewModel = new CargoAddViewModel()
        {
            Name = "Woods",
            Weight = 22,
            PhysicalState = 1,
            Category = 1,
            Description = "Very very big woods, requiring driver with many expoiriences and very good truckm and trailer",
            ImageUrl = "https://housing.com/news/wp-content/uploads/2023/04/What-is-timber-wood-and-which-are-the-best-types-f.jpg"
        };
        CargoEditViewModel = new CargoEditViewModel()
        {
            Name = "Wood",
            Weight = 20,
            PhysicalState = 1,
            Category = 1,
            Description = "Very big woods, which require driver with many expiriences and very good truck and trailer",
            ImageUrl = "https://housing.com/news/wp-content/uploads/2023/04/What-is-timber-wood-and-which-are-the-best-types-f.jpg"
        };
        Cargo = new Cargo()
        {
            Name = "Wood shavings",
            PhysicalState = CargoPhysicalState.Hard,
            Category = CargoCategory.DryBulkCargo,
            CreatorId = Guid.NewGuid(),
            Description = "jdhaks hajks dhsjka dh jks hajkdhjska dhjska djsa dhjkasdhj kas dhjask hdjka hdjask hdjska hdjksa",
            ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fwww.shutterstock.com%2Fshutterstock%2Fphotos%2F2026545179%2Fdisplay_1500%2Fstock-photo-convoy-of-the-long-hauler-big-rig-white-semi-trucks-tractors-transporting-commercial-cargo-in-dry-2026545179.jpg&tbnid=GwHZE87lObU0QM&vet=12ahUKEwiujv7d6OWAAxUn_bsIHXtuDXUQMygDegQIARBd..i&imgrefurl=https%3A%2F%2Fwww.shutterstock.com%2Fsearch%2Ftrailer&docid=4zAeOm24A6SSFM&w=1500&h=1101&q=trailer%20images&client=safari&ved=2ahUKEwiujv7d6OWAAxUn_bsIHXtuDXUQMygDegQIARBd",
            Weight = 20
        };

        TrailerAddViewModel = new TrailerAddViewModel()
        {
            Capacity = 25,
            Category = 2,
            Condition = 1,
            Dementions = 2,
            ImageUrl = "https://www.kaufmantrailers.com/wp-content/uploads/2013/01/Standard-Equipment-Trailers.jpg"
        };
        TrailerEditViewModel = new TrailerEditViewModel()
        {
            Capacity = 20,
            Category = 1,
            Condition = 2,
            Dementions = 2,
            ImageUrl = "https://www.kaufmantrailers.com/wp-content/uploads/2013/01/Standard-Equipment-Trailers.jpg"
        };
        Trailer = new Trailer()
        {
            Capacity = 20,
            Category = TrailerCategory.Gondola,
            Condition = TrailerCondition.Excellent,
            Dementions = TrailerDemention.Hanger,
            ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.shutterstock.com%2Fsearch%2Ftrailer&psig=AOvVaw1eyxGghpnmsdVFJghV8xDB&ust=1692434708228000&source=images&cd=vfe&opi=89978449&ved=0CBAQjRxqFwoTCKD12d7o5YADFQAAAAAdAAAAABAE",
            CreatorId = Guid.NewGuid()
        };

        TruckLowCondition = new Truck()
        {
            Make = "Iveco",
            Model = "XCC",
            RegistrationNumber = "P 2346 EH",
            Condition = (TruckCondition)9,
            FuelCapacity = 800,
            TraveledDistance = 1000,
            ImageUrl = "https://www.daf.co.uk/-/media/images/daf-trucks/trucks/euro-6/daf-xf/2017035-daf-xf.jpg?mw=1200&rev=e21fa83ff62d405b9db95e6914c636e1&hash=5102BA118103505BCA802B2D812C22BD",
            CreatedOn = DateTime.Now,
            CreatorId = new Guid("2f546a89-be24-4111-bff8-769bbfe1132a")
        };

        Course = new Course()
        {
            DepartureCity = "Sofia",
            ArrivalCity = "Smolyan",
            Distance = 250,
            Reward = 650,
            Driver = Driver,
            Truck = Truck,
            Trailer = Trailer,
            Cargo = Cargo
        };
        InvalidStartCourseViewModel = new StartCourseViewModel()
        {
            DepartureCity = "Plovdiv",
            ArrivalCity = "Burgas",
            Distance = 220,
            TruckId = Guid.NewGuid().ToString(),
            TrailerId = Trailer.Id.ToString(),
            CargoId = Cargo.Id.ToString()
        };
        LowConditionStartCourseViewModel = new StartCourseViewModel()
        {
            DepartureCity = "Plovdiv",
            ArrivalCity = "Burgas",
            Distance = 220,
            TruckId = TruckLowCondition.Id.ToString(),
            TrailerId = Trailer.Id.ToString(),
            CargoId = Cargo.Id.ToString()
        };
        StartCourseViewModel = new StartCourseViewModel()
        {
            DepartureCity = "Plovdiv",
            ArrivalCity = "Burgas",
            Distance = 220,
            TruckId = Truck.Id.ToString(),
            TrailerId = Trailer.Id.ToString(),
            CargoId = Cargo.Id.ToString()
        };

        DriversCargoes = new DriversCargoes()
        {
            CargoId = Cargo.Id,
            DriverId = Driver.Id
        };

        dbCtx.Users.Add(DriverUser);
        dbCtx.Users.Add(User);
        dbCtx.Trucks.Add(Truck);

        dbCtx.Drivers.Add(Driver);

        dbCtx.SaveChanges();
    }
}

