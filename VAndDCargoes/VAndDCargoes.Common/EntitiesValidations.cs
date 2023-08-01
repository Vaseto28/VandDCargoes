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
        public const int NameMaxLength = 15;
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
        public const int EmailMaxLength = 100;
    }
}

