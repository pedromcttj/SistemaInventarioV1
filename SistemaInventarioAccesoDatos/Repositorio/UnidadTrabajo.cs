using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV1.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaInventarioAccesoDatos.Repositorio
{
    public class UnidadTrabajo : IunidadTrabajo
    {
        private readonly ApplicationDbContext _db; 

        public IBodegaRepositorio Bodega { get; private set; }


        public ICategoriaRepositorio Categoria { get; private set; }

        public IMarcaRepositorio Marca { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepositorio(_db);
            Categoria = new CategoriaRepositorio(_db);
            Marca = new MarcaRepositorio(_db);

        }
        //IBodegaRepositorio IunidadTrabajo.Bodega => throw new NotImplementedException();

        public void Dispose()
        {
           _db.Dispose();
        }

        public async Task Guardar()
        {
           await _db.SaveChangesAsync();
        }
    }
}
