using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaInventarioModelos.Especificiaciones
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)//recibe lista generiaca 
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)//Por ejemplo si el resultado de total da 1.5 lo transforma a 2 

            }; 
            AddRange(items); //Agrega los elementos de la coleccion al final de la lista
                
        }

        // Recibe tres parámetros:
        // 1. "entidad": La lista completa de datos (de tipo genérico <T> para que sirva con Productos, Clientes, etc.).
        // 2. "pageNumber": El número de la página que el usuario quiere ver (ej. Página 2).
        // 3. "pagesize": Cuántos elementos se deben mostrar por cada página (ej. 3 productos por página).
        public static PagedList<T> ToPagedList(IEnumerable<T> entidad, int pageNumber, int pagesize)
        {
            // 1. Cuenta el total de registros existentes en la base de datos antes de recortarlos.
            // Esto es vital para que la vista sepa cuántos botones de páginas (1, 2, 3...) debe dibujar.
            var count = entidad.Count();

            // 2. Aquí ocurre la magia matemática de la paginación usando LINQ:
            //    - "Skip(...)": Salta (ignora) los registros de las páginas anteriores.
            //      Fórmula: Si estás en la página 2 y el tamaño es 3 -> (2 - 1) * 3 = 3. Se salta los primeros 3 registros.
            //    - "Take(pagesize)": Después de saltar los anteriores, toma solo la cantidad permitida para esta página (ej. toma 3).
            //    - "ToList()": Ejecuta la consulta y convierte ese pequeño bloque recortado en una lista real en memoria.
            var items = entidad.Skip((pageNumber - 1) * pagesize).Take(pagesize).ToList();

            // 3. Envía los datos calculados al constructor de la clase 'PagedList':
            //    Le pasa el bloque de elementos de esta página, el total general, la página actual y el tamaño.
            return new PagedList<T>(items, count, pageNumber, pagesize);
        }

    }
}
