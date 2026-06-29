using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaInventarioAccesoDatos.Repositorio.IRepositorio
{
    public interface IunidadTrabajo : IDisposable//Permite desahacer de cualquier recurso  que haya obtenido el sistema o recursos que no se usen 
    {
        IBodegaRepositorio Bodega { get; }
        ICategoriaRepositorio Categoria { get; }

        IMarcaRepositorio Marca { get; }

        IProductoRepositorio Producto { get; }

        IUsuarioAplicacionRepositorio UsuarioAplicacion { get; }
        
        Task Guardar();
    }
}
