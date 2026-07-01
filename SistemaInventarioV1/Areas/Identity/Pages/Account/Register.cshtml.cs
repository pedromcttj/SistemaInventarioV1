// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
//using SistemaInventarioV1.Data;
using SistemaInventarioAccesoDatos.Data;
using SistemaInventarioModelos;
using SistemaInventarioUtilidades;





namespace SistemaInventarioV1.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IEmailSender _emailSender;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        _emailSender = emailSender;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = default!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = default!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Nombres { get; set; }
        [Required]
        public string Apellidos { get; set; }

        public string Direccion { get; set; }

        public string Ciudad { get; set; }

        public string Pais { get; set; }

        public string? Role { get; set; }

        public IEnumerable<SelectListItem>? ListaRol { get; set; }

    }


    public async Task OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        // 1. Traemos de inmediato el contexto de la Base de Datos que sí está activo
        var db = HttpContext.RequestServices.GetRequiredService<SistemaInventarioV1.AccesoDatos.Data.ApplicationDbContext>();

        // 2. Llenamos el modelo leyendo la tabla de roles directamente desde EF Core
        Input = new InputModel()
        {
            ListaRol = db.Roles
                .Where(r => r.Name != DS.Role_Cliente)
                .Select(n => n.Name)
                .Select(l => new SelectListItem()
                {
                    Text = l,
                    Value = l
                }).ToList() // Agregamos .ToList() para cargar los datos de inmediato
        };
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            // CORRECCIÓN 1: Eliminamos la línea "Role = Input.Role" para que compile
            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber,
                Nombres = Input.Nombres,
                Apellidos = Input.Apellidos,
                Direccion = Input.Direccion,
                Ciudad = Input.Ciudad,
                Pais = Input.Pais
            };

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // Usamos un único contexto de Base de Datos para todo el bloque
                var db = HttpContext.RequestServices.GetRequiredService<SistemaInventarioV1.AccesoDatos.Data.ApplicationDbContext>();

                // Creación de roles si no existen en la tabla AspNetRoles
                if (!db.Roles.Any(r => r.Name == "Admin"))
                {
                    db.Roles.Add(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                }

                if (!db.Roles.Any(r => r.Name == "Cliente"))
                {
                    db.Roles.Add(new IdentityRole { Name = "Cliente", NormalizedName = "CLIENTE" });
                }

                if (!db.Roles.Any(r => r.Name == "Inventario"))
                {
                    db.Roles.Add(new IdentityRole { Name = "Inventario", NormalizedName = "INVENTARIO" });
                }

                await db.SaveChangesAsync();

                // CORRECCIÓN 2: Lógica pura de Entity Framework 10 para asignar el rol
                string rolParaAsignar;

                if (string.IsNullOrEmpty(Input.Role))
                {
                    rolParaAsignar = db.Roles.Where(r => r.Name == DS.Role_Cliente).Select(r => r.Id).FirstOrDefault()!;
                }
                else
                {
                    rolParaAsignar = db.Roles.Where(r => r.Name == Input.Role).Select(r => r.Id).FirstOrDefault()!;
                }

                if (!string.IsNullOrEmpty(rolParaAsignar))
                {
                    // Insertamos directo en la tabla intermedia AspNetUserRoles
                    db.UserRoles.Add(new IdentityUserRole<string>
                    {
                        UserId = user.Id,
                        RoleId = rolParaAsignar
                    });

                    await db.SaveChangesAsync();
                }

                // CORRECCIÓN 3: Eliminamos por completo el bloque repetido de AddToRoleAsync que hacía que tronara SQL

                var userId = await _userManager.GetUserIdAsync(user);

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                }
                else
                {
                    // FIX: Evaluamos lo que se seleccionó en el formulario de la página (Input.Role)
                    if (string.IsNullOrEmpty(Input.Role))
                    {
                        // Si no se eligió rol (es un registro público de cliente), se inicia sesión automáticamente
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        // Si hay un rol seleccionado, significa que un Administrador creó al usuario.
                        // Lo redirigimos al catálogo o índice de usuarios sin iniciar sesión con la cuenta nueva.
                        return RedirectToAction("Index", "Usuario", new { area = "Admin" }); // Corrección en el nombre del controlador "Usuario"
                    }

                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // Recarga de roles si el modelo es falso o hay error de contraseña
        var db2 = HttpContext.RequestServices.GetRequiredService<SistemaInventarioV1.AccesoDatos.Data.ApplicationDbContext>();

        Input.ListaRol = db2.Roles
            .Where(r => r.Name != DS.Role_Cliente)
            .Select(n => n.Name)
            .Select(l => new SelectListItem
            {
                Text = l,
                Value = l
            }).ToList();

        return Page();
    }


    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }
}
