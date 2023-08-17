namespace VAndDCargoes.Common;

public static class EntitiesValidations
{
    public static class Truck
    {
        public const int MakeMinLength = 2;
        public const int MakeMaxLength = 15;
        public const int ModelMinLength = 2;
        public const int ModelMaxLength = 20;
        public const int RegistrationNumberMinLength = 6;
        public const int RegistrationNumberMaxLength = 15;
        public const int ConditionLowerBound = 0;
        public const int ConditionUpperBound = 4;
    }

    public static class Trailer
    {
        public const int CapacityMinValue = 1;
        public const int CapacityMaxValue = 41;
        public const int CategoryLowerBound = 0;
        public const int CategoryUpperBound = 5;
        public const int ConditionLowerBound = 0;
        public const int ConditionUpperBound = 4;
        public const int DementionLowerBound = 0;
        public const int DementionUpperBound = 3;
    }

    public static class Cargo
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 30;
        public const int DescriptionMinLength = 20;
        public const int DescriptionMaxLength = 200;
        public const int WeightMinLength = 1;
        public const int WeightMaxLength = 40;
        public const int CategoryLowerBound = 0;
        public const int CategoryUpperBound = 6;
        public const int PhysicalStateLowerBound = 0;
        public const int PhysicalStateUpperBound = 2;
    }

    public static class Driver
    {
        public const int FirstNameMinLength = 3;
        public const int FirstNameMaxLength = 15;
        public const int SecondNameMinLength = 3;
        public const int SecondNameMaxLength = 15;
        public const int LastNameMinLength = 3;
        public const int LastNameMaxLength = 15;
        public const int PhoneNumberMinLength = 4;
        public const int PhoneNumberMaxLength = 15;
        public const int EmailMinLength = 8;
        public const int EmailMaxLength = 40;
    }

    public static class User
    {
        public const int UsernameMinLength = 4;
        public const int UsernameMaxLength = 20;
        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 100;
    }

    public static class Course
    {
        public const int DepartureCityMinLength = 2;
        public const int DepartureCityMaxLength = 40;
        public const int ArrivalCityMinLength = 2;
        public const int ArrivalCityMaxLength = 40;
        public const int PaymentMinValue = 1000;
        public const int PaymentMaxValue = 10000;
        public const int DistanceMinValue = 10;
        public const int DistanceMaxValue = 10000;
    }

    public static class Repairment
    {
        public const int TypeMinLength = 5;
        public const int TypeMaxLength = 30;
        public const int DescriptionMinLength = 25;
        public const int DescriptionMaxLenght = 100;
        public const int CostMinValue = 30;
        public const int CostMaxValue = 2000;
    }
}

