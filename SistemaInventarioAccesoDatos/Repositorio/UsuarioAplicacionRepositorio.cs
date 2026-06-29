using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioModelos;
using SistemaInventarioV1.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SistemaInventarioAccesoDatos.Repositorio
{
    public class UsuarioAplicacionRepositorio : Repositorio<UsuarioAplicacion>, IUsuarioAplicacionRepositorio
    {
        private readonly ApplicationDbContext _db;

        public UsuarioAplicacionRepositorio(ApplicationDbContext db) : base(db) {

            _db = db;
        }


    
    }
}
