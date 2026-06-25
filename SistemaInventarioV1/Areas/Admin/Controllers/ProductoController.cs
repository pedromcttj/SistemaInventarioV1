using Microsoft.AspNetCore.Mvc;
using SistemaInventarioAccesoDatos.Repositorio;
using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioModelos;
using SistemaInventarioModelos.ViewModels;
using SistemaInventarioUtilidades;

namespace SistemaInventarioV1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductoController : Controller
    {
        //Hererdar nuestra unidad de trabajo 
        private readonly IunidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;//Permite la carga de imagenes.
        //creacion de constructor
        public ProductoController (IunidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment) //se inicializa IWebHostEnvironment
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Categoria"),
                MarcaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Marca"),
                PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Producto")

            };

            if (id == null)
            {
                productoVM.Producto.Estado = true;
                //Crear nuevo Producto
                return View(productoVM);

            }
            else {
                productoVM.Producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (productoVM.Producto == null) {
                    return NotFound();
                }
                return View(productoVM);
            
            }

            
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Upsert(ProductoVM productoVM) {

            if (ModelState.IsValid) {

                var files = HttpContext.Request.Form.Files;
                String webRootPath = _webHostEnvironment.WebRootPath;

                if (productoVM.Producto.Id == 0)
                {

                    //Crear 

                    String upload = webRootPath + DS.ImagenRuta;
                    String fileName = Guid.NewGuid().ToString();
                    String extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productoVM.Producto.ImageUrl = fileName + extension;
                    await _unidadTrabajo.Producto.Agregar(productoVM.Producto);


                }
                else {

                    //actualizar
                    var objProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoVM.Producto.Id, isTracking: false);

                        if (files.Count > 0)
                        {

                            string upload = webRootPath + DS.ImagenRuta;
                            string fileName = Guid.NewGuid().ToString();
                            string extension = Path.GetExtension(files[0].FileName);

                            //Borrar la imagen anterior 
                            var anteriorfile = Path.Combine(upload, objProducto.ImageUrl);
                            if (System.IO.File.Exists(anteriorfile))
                            {

                                System.IO.File.Delete(anteriorfile);

                            }

                            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                            {
                                files[0].CopyTo(fileStream);

                            }
                            productoVM.Producto.ImageUrl = fileName + extension;



                        } //Caso de que no se cargue una nueva imagen
                        else {
                            productoVM.Producto.ImageUrl = objProducto.ImageUrl;
                        
                        
                        }
                        _unidadTrabajo.Producto.Actualizar(productoVM.Producto);


                } 
                   TempData[DS.Exitosa] = "Transaccion Exitosa!";
                    await _unidadTrabajo.Guardar();
                    return View("Index");

            }//if no valid 
            productoVM.CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Categoria");
            productoVM.MarcaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Marca");
            productoVM.PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Producto");
            return View(productoVM);



           
        
        
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "Categoria,Marca");
            return Json(new { data = todos });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id) {
            var productoDb = await _unidadTrabajo.Producto.Obtener(id);
            if (productoDb == null) {
                return Json(new { success = false, message = "Error al borrar Producto" });
            }


            //Remover imagen 
            string upload = _webHostEnvironment.WebRootPath + DS.ImagenRuta;
            var anteriorfile = Path.Combine(upload, productoDb.ImageUrl);
            if (System.IO.File.Exists(anteriorfile)) { 
                System.IO.File.Delete(anteriorfile);
            }

            _unidadTrabajo.Producto.Remover(productoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrado extosamente" });


        }
        [ActionName("ValidarSerie")]

        public async Task<IActionResult> ValidarSerie(string serie, int id = 0) {
            bool valor = false;
            var lista = await _unidadTrabajo.Producto.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.NumeroSerie.ToLower().Trim() == serie.ToLower().Trim());
            }
            else {
                valor = lista.Any(b => b.NumeroSerie.ToLower().Trim() == serie.ToLower().Trim() && b.Id != id);
            }

            if (valor) {
                return Json(new { data = true });
            
            }

            return Json(new { data = false });
        
        }

        #endregion
    }
}
