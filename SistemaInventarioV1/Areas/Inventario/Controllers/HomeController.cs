using Microsoft.AspNetCore.Mvc;
using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioModelos;
using SistemaInventarioModelos.ErrorViewModel;
using SistemaInventarioModelos.ViewModels;
using System.Diagnostics;

namespace SistemaInventarioV1.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IunidadTrabajo _unidadTrabajo;

        public HomeController(ILogger<HomeController> logger, IunidadTrabajo unidadTrabajo) { 
         
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }
        
        public async Task<IActionResult> Index()
        { 
            IEnumerable<Producto> productoLista = await _unidadTrabajo.Producto.ObtenerTodos();
            return View(productoLista);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
