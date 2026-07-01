

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

            "url": "/Admin/Usuario/ObtenerTodos"
        },
        "columns": [
            { "data": "email", },
            { "data": "nombres" },
            { "data": "apellidos" },
            { "data": "phoneNumber" },
            { "data": "role" },
           
            {
                "data": { //para trabajar  con 2 columnas 
                    id: "id", lockoutEnd: "lockoutEnd"  

                },
                "render": function (data) { 
                    //declaraciones de varoabñes
                    let hoy = new Date().getTime();
                    let bloqueo = new Date(data.lockoutEnd).getTime();
                    if (bloqueo > hoy) {
                        //usuario esta Bloqueado
                        return `
                        <div class="text-center">

                            <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-danger text-white" style="cursor:pointer" , width : 150px>
                            <i class="bi bi-unlock2-fill"></i>Desbloquear

                            </a>
                        </div>
                    `;
                    }
                    else
                    { 
                        return `
                        <div class="text-center">

                            <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-success text-white" style="cursor:pointer" , width : 150px>
                            <i class="bi bi-lock-fill"></i>Bloquear

                            </a>
                        </div>
                    `;


                    }

                }, "width": "20%"
            }
        ]

    });

    
}


function BloquearDesbloquear(id) {

    $.ajax({
        type: "POST", 
        url: "/Admin/Usuario/BloquearDesbloquear",
        data: JSON.stringify(id),
        contentType: "application/json",
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
