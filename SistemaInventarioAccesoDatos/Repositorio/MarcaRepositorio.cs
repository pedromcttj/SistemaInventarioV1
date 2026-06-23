using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioModelos;
using SistemaInventarioV1.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SistemaInventarioAccesoDatos.Repositorio
{
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public MarcaRepositorio(ApplicationDbContext db) : base(db) {

            _db = db;
        }
        public void Actualizar(Marca marca)
        {
            //v actualizar el registro de bodga que envie 
            var marcaBD = _db.Marcas.FirstOrDefault(b => b.Id == marca.Id); 
            if (marcaBD != null) 
            {
                marcaBD.Nombre= marca.Nombre;
                marcaBD.Descripcion = marca.Descripcion;
                marcaBD.Estado = marca.Estado;
                _db.SaveChanges();
            }
        }

    
    }
}
