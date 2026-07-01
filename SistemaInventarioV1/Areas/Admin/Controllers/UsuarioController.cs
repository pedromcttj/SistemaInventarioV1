using Microsoft.AspNetCore.Mvc;
using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV1.AccesoDatos.Data;
using Microsoft.EntityFrameworkCore;

namespace SistemaInventarioV1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsuarioController : Controller
    {

        private readonly IunidadTrabajo _unidadTrabajo;
        private readonly ApplicationDbContext _db;

        public UsuarioController(IunidadTrabajo unidadTrabajo, ApplicationDbContext db)
        {
            _unidadTrabajo = unidadTrabajo;
            _db = db;

        }
        public IActionResult Index()
        {
            return View();
        }

        #region API 
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            // 1. Traemos la lista completa de usuarios

            //var usuarioLista = await _unidadTrabajo.UsuarioAplicacion.ObtenerTodos();
            // CORRECCIÓN: En lugar de usar _unidadTrabajo, consultamos directamente la tabla Users de Identity
            // que hereda de ApplicationUser, asegurando leer de la tabla 'AspNetUsers'
            var usuarioLista = await _db.Users.ToListAsync();

            // 2. Traemos las tablas de Identity usando la librería de EF Core correcta
            var userRole = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();

            // 3. Mapeamos los roles de forma segura
            foreach (var usuario in usuarioLista)
            {
                // Si encontramos la relación, buscamos el nombre del rol físico
                //var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                //usuario.Role = roles.FirstOrDefault(u => u.Id == roleId).Name; 
                // Buscamos la relación del usuario con su rol
                var relacionUsuarioRol = userRole.FirstOrDefault(u => u.UserId == usuario.Id);

                if (relacionUsuarioRol != null)
                {
                    // Si encontramos la relación, buscamos el nombre del rol físico
                    var rolEncontrado = roles.FirstOrDefault(u => u.Id == relacionUsuarioRol.RoleId);
                    usuario.Role = rolEncontrado != null ? rolEncontrado.Name : "Sin Rol";
                }
                else
                {
                    usuario.Role = "Sin Rol"; // Evita que truene si el usuario no tiene rol asignado
                }
            }

            return Json(new { data = usuarioLista });

        }

        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id)
        {
            var usuario = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null) 
            {
                return Json(new { success = false, message = "Error de Usuario" });
            
            }
            if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
            {
                //Usuario Bloqueado 
                usuario.LockoutEnd = DateTime.Now;

            }
            else 
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(1000);
            
            }
            await _unidadTrabajo.Guardar();

            return Json(new { success = true, message = "Operacion Exitosa" });
        }
        #endregion
    }
}
