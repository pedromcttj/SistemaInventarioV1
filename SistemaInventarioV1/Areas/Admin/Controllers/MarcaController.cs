using Microsoft.AspNetCore.Mvc;
using SistemaInventarioAccesoDatos.Repositorio;
using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioModelos;
using SistemaInventarioUtilidades;

namespace SistemaInventarioV1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MarcaController : Controller
    {
        //Hererdar nuestra unidad de trabajo 
        private readonly IunidadTrabajo _unidadTrabajo;

        //creacion de constructor
        public MarcaController(IunidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Marca marca = new Marca();

            if (id == null)
            {
                //Crear nueva marca
                marca.Estado = true;
                return View(marca);
            }
            //actualizamos marca
            marca = await _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
            if (marca == null) {

                return NotFound();
            }
            return View(marca);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]//sirve para falsificacion de solicitudes 
        public async Task<IActionResult> Upsert(Marca marca) {

            if (ModelState.IsValid) {
                if (marca.Id == 0) {

                    await _unidadTrabajo.Marca.Agregar(marca);
                    TempData[DS.Error] = "Marca creada exitosamanete";
                }
                else {
                    _unidadTrabajo.Marca.Actualizar(marca);
                    TempData[DS.Error] = "Marca actualizada exitosamanete";
                }

                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));

            }
            TempData[DS.Error] = "Error al grabar Marca";
            return View(marca);

        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Marca.ObtenerTodos();
            return Json(new { data = todos });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id) {
            var marcaDb = await _unidadTrabajo.Marca.Obtener(id);
            if (marcaDb == null) {
                return Json(new { success = false, message = "Error al borrar Categoria" });
            }

            _unidadTrabajo.Marca.Remover(marcaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoria borrada extosamente" });


        }
        [ActionName("ValidarNombre")]

        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0) {
            bool valor = false;
            var lista = await _unidadTrabajo.Marca.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }

            if (valor) {
                return Json(new { data = true });
            
            }

            return Json(new { data = false });
        
        }

        #endregion
    }
}
