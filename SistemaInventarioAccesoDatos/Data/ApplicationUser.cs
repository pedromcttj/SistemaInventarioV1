using Microsoft.AspNetCore.Identity;
namespace SistemaInventarioAccesoDatos.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string Nombres { get; set; } = default!;
    public string Apellidos { get; set; } = default!;
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? Pais { get; set; }

 
}
