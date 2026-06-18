using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SistemaInventarioAccesoDatos.Repositorio.IRepositorio
{
    public  interface IRepositorio <T> where T : class //SE CREA UNA INTERFACE GENERICA , PARA QUE TRABAJE CUALQUIER OBJETO DE CUALQUIER CLASE 
    {
       Task <T> Obtener(int id); //PARA INDICAR QUE SEA ASINCORN , SE AGREGA Task<T> a todos

       Task <IEnumerable<T>> ObtenerTodos(
            Expression<Func<T,bool>> filtro =null,
            Func<IQueryable<T>,IOrderedQueryable<T>>orderBy = null,
            string incluirPropiedades = null, 
            bool isTracking = true);


        Task<T> ObtenerPrimero(
            Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null,
            bool isTracking = true);

        Task Agregar(T entidad);

        void Remover(T entidad);

        void RemoverRango(IEnumerable<T> entidad);
    }
}
