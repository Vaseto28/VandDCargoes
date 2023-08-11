using System.ComponentModel.DataAnnotations;
using VAndDCargoes.Data.Models.Enumerations;

namespace VAndDCargoes.Data.Models;

public class Trailer
{
    public Trailer()
    {
        this.Id = Guid.NewGuid();

        this.DriversTrailers = new HashSet<DriversTrailers>();
        this.Courses = new HashSet<Course>();
    }

    [Key]
    public Guid Id { get; set; }

    public int Capacity { get; set; }

    [Required]
    public string ImageUrl { get; set; } = null!;

    [Required]
    public TrailerCondition Condition { get; set; }

    [Required]
    public TrailerCategory Category { get; set; }

    [Required]
    public TrailerDemention Dementions { get; set; }

    [Required]
    public Guid CreatorId { get; set; }

    public virtual ApplicationUser Creator { get; set; } = null!;

    public virtual ICollection<DriversTrailers> DriversTrailers { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = null!;
}

