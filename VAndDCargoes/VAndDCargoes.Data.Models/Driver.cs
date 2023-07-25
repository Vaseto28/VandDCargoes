﻿using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Driver;

namespace VAndDCargoes.Data.Models;

public class Driver
{
    public Driver()
    {
        this.Id = Guid.NewGuid();
        this.Trucks = new HashSet<Truck>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(SecondNameMaxLength)]
    public string SecondName { get; set; } = null!;

    [Required]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; set; } = null!;

    public int Age { get; set; }

    [Required]
    [MaxLength(PhoneNumberMaxLength)]
    public string PhoneNumber { get; set; } = null!;

    public Guid UserId { get; set; }

    public virtual ApplicationUser User { get; set; } = null!;

    public virtual ICollection<Truck> Trucks { get; set; }
}

