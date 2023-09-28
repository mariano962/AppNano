window.onload = BuscarAsignaturas();

function BuscarAsignaturas(){

    $("#tbody-asignatura").empty();
    $.ajax({
        // la URL para la petición
        url : '../../Asignaturas/BuscarAsignatura',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { },
    
        // especifica si será una petición POST o GET
        type : 'POST',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(asignaturas) {

            $("#tbody-asignatura").empty();
            $.each(asignaturas, function( index, asignatura){

                    let eliminarAsignatura = 'table-success'
                    let boton = '<buttom type="button"   class="btn btn-warning btn-sm" onClick="BuscarAsignatura(' + asignatura.asignaturaID + ')">Editar </buttom> ' +
                    '<buttom type="button"   class="btn btn-danger btn-sm" onClick="Deshabilitar(' + asignatura.asignaturaID + ')">Deshabilitar </buttom> '
                    
                    if (asignatura.eliminado) {
                        EliminarCarrera  = 'table-danger';
                        boton =  '<buttom type="button"   class="btn btn-success btn-sm" onClick="Deshabilitar(' + asignatura.asignaturaID + ')">Activar </buttom> '
                    }

                    $("#tbody-asignatura").append('<tr class=' + eliminarAsignatura + '>' +
                    '<td>' + asignatura.nombreAsignatura + '</td>' +
                    '<td>' + asignatura.nombreCarrera + '</td>' +
                    '<td>' + boton + '</td>' +
                    '</tr>');
            }
            )
        },

       
        error : function(xhr, status) {
            alert('Error al cargar la asignatura');
        },
   
        // código a ejecutar sin importar si la petición falló o no
        complete : function(xhr, status) {
            //alert('Petición realizada');
        }

    });
}

function VaciarFormulario(){
    $("#AsignaturaID").val(0);
    $("#NombreAsignatura").val('');
    $("#CarreraID").val('');
   
}

function BuscarAsignatura(asignaturaID) {
    $.ajax({

        url: '../../Asignaturas/BuscarAsignatura',
 
        data: { AsignaturaID: asignaturaID },
     
        type: 'GET',
 
        dataType: 'json',
    
        success: function (carreras) {

            if (carreras.length == 1) {
                let carrera12 = carreras[0];
              
                $("#NombreAsignatura").val(carrera12.nombreAsignatura);
                $("#CarreraID").val(carrera12.carreraID);
                $("#AsignaturaID").val(carrera12.asignaturaID);
                $("#ModalAsignatura").modal("show");
            }
        },

    
        error: function (xhr, status) {
            alert('La carrera que intentas eliminar esta en uso');
        },

        // código a ejecutar sin importar si la petición falló o no
        complete: function (xhr, status) {
            //alert('Petición realizada');
        }
    });
}

function GuardarAsignatura() {
 
    let nombreAsignatura = document.getElementById("NombreAsignatura").value;
    let asignaturaID = document.getElementById("AsignaturaID").value;
    let carreraID = $("#CarreraID").val();

    $.ajax({

        url: '../../Asignaturas/GuardarAsignatura',
   
        data: { NombreAsignatura: nombreAsignatura, AsignaturaID: asignaturaID, CarreraID: carreraID },
    
        type: 'POST',
     
        dataType: 'json',
    
        success: function (resultado) {

            if (resultado) {
                $("#ModalAsignatura").modal("hide");
                BuscarAsignaturas();
            }
            else {
               
                 alert("Este campo ya existe")
            }
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            alert('Disculpe, existió un problema');
        }
    });
}

function Deshabilitar (asignaturaID) {
   
    $.ajax({
        // la URL para la petición
        url : '../../Asignaturas/Deshabilitar',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { AsignaturaID : asignaturaID },
    
        // especifica si será una petición POST o GET
        type : 'GET',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(resultado) {
            if (resultado == 1) {
              
                BuscarAsignaturas();
                
            }
            else { 
                    alert("error la deshabilitar la Asignatura")
            } 
        },
    
        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error : function(xhr, status) {
            alert('Disculpe, existió un problema');
        },
    
        // código a ejecutar sin importar si la petición falló o no
        // complete : function(xhr, status) {
        //     alert('Petición realizada');
        // }
    });
}