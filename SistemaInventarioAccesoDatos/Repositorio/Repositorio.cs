using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV1.AccesoDatos.Data;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;
//using System.Data.Entity;
using System.Threading.Tasks;

namespace SistemaInventarioAccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet <T> dbSet;

        public Repositorio(ApplicationDbContext db)
        {
          _db = db;
            this.dbSet = _db.Set<T>();
                
        }

        public async Task Agregar(T entidad)
        { 
            //todo metodo asyncrono , debe llevar dentro un await
            await dbSet.AddAsync(entidad); //esto es equivalente a un Insert into Table
        }

        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id); //Select * from (solo por id) 
        }


        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null) { 
                query = query.Where(filtro); //select /* from where..
            }
            if (incluirPropiedades != null) {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp); // Ejemplo "Categoria,Marca"
                }
            }
            if (orderBy != null) {

                query = orderBy(query);
            }

            if (!isTracking) {

                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro); //select /* from where..
            }
            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp); // Ejemplo "Categoria,Marca"
                }
            }

            if (!isTracking)
            {

                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();//REGRESA SOLO EL PRIMER REGISTRO

        }



        public void Remover(T entidad) //Remover nunca  puede quedar asyncrono
        {
            dbSet.Remove(entidad);  
        }

        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad);
        }
    }
}
