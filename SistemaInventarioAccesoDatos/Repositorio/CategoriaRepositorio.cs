using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioModelos;
using SistemaInventarioV1.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SistemaInventarioAccesoDatos.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepositorio(ApplicationDbContext db) : base(db) {

            _db = db;
        }
        public void Actualizar(Categoria categoria)
        {
            //v actualizar el registro de bodga que envie 
            var categoriaBD = _db.Categorias.FirstOrDefault(b => b.Id == categoria.Id); 
            if (categoriaBD != null) 
            {
                categoriaBD.Nombre= categoria.Nombre;
                categoriaBD.Descripcion = categoria.Descripcion;
                categoriaBD.Estado = categoria.Estado;
                _db.SaveChanges();
            }
        }

    
    }
}
