using Microsoft.AspNetCore.Identity;

namespace VAndDCargoes.Data.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        this.Id = Guid.NewGuid();
        this.Drivers = new HashSet<Driver>();
        this.Trucks = new HashSet<Truck>();
        this.Trailers = new HashSet<Trailer>();
        this.Cargoes = new HashSet<Cargo>();
    }

    public virtual ICollection<Driver> Drivers { get; set; }

    public virtual ICollection<Truck> Trucks { get; set; }

    public virtual ICollection<Trailer> Trailers { get; set; }

    public virtual ICollection<Cargo> Cargoes { get; set; }
}

