using Microsoft.AspNetCore.Mvc;
using SistemaInventarioAccesoDatos.Repositorio;
using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioModelos;
using SistemaInventarioUtilidades;

namespace SistemaInventarioV1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BodegaController : Controller
    {
        //Hererdar nuestra unidad de trabajo 
        private readonly IunidadTrabajo _unidadTrabajo;

        //creacion de constructor
        public BodegaController(IunidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Bodega bodega = new Bodega();

            if (id == null)
            {
                //Crear nueva bodega
                bodega.Estado = true;
                return View(bodega);
            }
            //actualizamos bodega
            bodega = await _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());
            if (bodega == null) {

                return NotFound();
            }
            return View(bodega);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]//sirve para falsificacion de solicitudes 
        public async Task<IActionResult> Upsert(Bodega bodega) {

            if (ModelState.IsValid) {
                if (bodega.Id == 0) {

                    await _unidadTrabajo.Bodega.Agregar(bodega);
                    TempData[DS.Error] = "Bodega creada exitosamanete";
                }
                else {
                    _unidadTrabajo.Bodega.Actualizar(bodega);
                    TempData[DS.Error] = "Bodega actualizada exitosamanete";
                }

                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));

            }
            TempData[DS.Error] = "Error al grabar";
            return View(bodega);

        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new { data = todos });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id) {
            var bodegaDb = await _unidadTrabajo.Bodega.Obtener(id);
            if (bodegaDb == null) {
                return Json(new { success = false, message = "Error al borrar Bodega" });
            }

            _unidadTrabajo.Bodega.Remover(bodegaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Bodega borrada extosamente" });


        }
        [ActionName("ValidarNombre")]

        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0) {
            bool valor = false;
            var lista = await _unidadTrabajo.Bodega.ObtenerTodos();
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
