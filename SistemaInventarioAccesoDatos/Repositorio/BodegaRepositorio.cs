using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioModelos;
using SistemaInventarioV1.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SistemaInventarioAccesoDatos.Repositorio
{
    public class BodegaRepositorio : Repositorio<Bodega>, IBodegaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public BodegaRepositorio(ApplicationDbContext db) : base(db) {

            _db = db;
        }
        public void Actualizar(Bodega bodega)
        {
            //v actualizar el registro de bodga que envie 
            var bodegaBD = _db.Bodegas.FirstOrDefault(b => b.Id == bodega.Id); 
            if (bodegaBD != null) 
            {
                bodegaBD.Nombre= bodega.Nombre;
                bodegaBD.Descripcion = bodega.Descripcion;
                bodegaBD.Estado = bodega.Estado;
                _db.SaveChanges();
            }
        }

    
    }
}
