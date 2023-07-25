using Microsoft.AspNetCore.Identity;

namespace VAndDCargoes.Data.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        this.Id = Guid.NewGuid();
    }
}

