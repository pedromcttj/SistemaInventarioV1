

//Creacion de variables
let datatable;

//funcion para que se ejecute al cargar la pagina.
$(document).ready(function (){

    loadDataTable();

});


function loadDataTable() {

                  //se ingresa el id de la tabla
    datatable = $("#tblDatos").DataTable({
        "language": {//Esto es para cambiar de idioma  la parte de filtro, , titulos, paginados.
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },

        //seccion ajax 
        "ajax": {

            "url": "/Admin/Bodega/ObtenerTodos"
        },
        "columns": [
            { "data": "nombre", "width": "20%" },
            { "data": "descripcion", "width": "40%" },
            {
                "data": "estado",//como es es booleano y regresa true false , se renderiza para poder manejarlo
                "render": function (data) {
                    if (data == true) {
                        return "Activo";
                    }
                    else {
                        return "Inactivo";
                    }
                }, "width": "20%"
            }, 
            { 
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Bodega/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            <a onclick=Delete("/Admin/Bodega/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                            <i class="bi bi-trash3-fill"></i>

                            </a>
                        </div>
                    `;
                }, "width": "20%"
            }
        ]

    });

    
}


function Delete(url) {
    // SweetAlert
    swal({
        title: "¿Está seguro de eliminar la Bodega?",
        text: "Este registro no se podrá recuperar",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => { // <-- Corrección aquí: el paréntesis cierra después de la llave al final
        if (borrar) {
            $.ajax({
                type: "POST", // Se recomienda usar DELETE para eliminar, o déjalo en POST si tu controlador así lo recibe
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload(); // Recarga la tabla de inmediato
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
