﻿using static VAndDCargoes.Common.EntitiesValidations.Cargo;
using System.ComponentModel.DataAnnotations;
using VAndDCargoes.Data.Models.Enumerations;

namespace VAndDCargoes.Data.Models;

public class Cargo
{
    public Cargo()
    {
        this.Id = Guid.NewGuid();

        this.DriversCargoes = new HashSet<DriversCargoes>();
        this.Courses = new HashSet<Course>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    public int Weight { get; set; }

    [Required]
    public string ImageUrl { get; set; } = null!;

    [Required]
    public CargoCategory Category { get; set; }

    [Required]
    public CargoPhysicalState PhysicalState { get; set; }

    [Required]
    public Guid CreatorId { get; set; }

    public virtual ApplicationUser Creator { get; set; } = null!;

    public virtual ICollection<DriversCargoes> DriversCargoes { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = null!;
}

