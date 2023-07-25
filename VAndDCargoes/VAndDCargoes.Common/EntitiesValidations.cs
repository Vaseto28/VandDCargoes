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
    }

    public static class Trailer
    {
        public const int CapacityMinLength = 1;
        public const int CapacityMaxLength = 41;
    }

    public static class Cargo
    {
        public const int NameMinLength = 5;
        public const int NameMaxLength = 15;
        public const int DescriptionMinLength = 20;
        public const int DescriptionMaxLength = 200;
        public const int WeightMinLength = 1;
        public const int WeightMaxLength = 40;
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
        public const int EmailMaxLength = 100;
    }
}

