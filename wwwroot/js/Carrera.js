window.onload = BuscarCarreras();

function BuscarCarreras(){

    $("#tbody-carrera").empty();
    $.ajax({
        // la URL para la petición
        url : '../../Carreras/BuscarCarreras',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { },
    
        // especifica si será una petición POST o GET
        type : 'POST',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(carreras) {

            $("#tbody-carrera").empty();
            $.each(carreras, function( index, carrera){

                    let EliminarCarrera = 'table-primary'
                    let boton = '<buttom type="button"   class="btn btn-warning btn-sm" onClick="Buscarcarrera(' + carrera.carreraID + ')">Editar </buttom> ' +
                    '<buttom type="button"   class="btn btn-danger btn-sm" onClick="Deshabilitar(' + carrera.carreraID + ')">Deshabilitar </buttom> '
                    
                    if (carrera.eliminado) {
                        EliminarCarrera  = 'table-danger';
                        boton =  '<buttom type="button"   class="btn btn-success btn-sm" onClick="Deshabilitar(' + carrera.carreraID + ')">Activar </buttom> '
                    }

                    $("#tbody-carrera").append('<tr class=' + EliminarCarrera + '>' +
                    '<td>' + carrera.nombreCarrera + '</td>' +
                    '<td>' + carrera.duracion + '</td>' +
                    '<td>' + boton + '</td>' +
                    '</tr>');
            }
            )
        },

       
        error : function(xhr, status) {
            alert('Error al cargar');
        },
   
        // código a ejecutar sin importar si la petición falló o no
        complete : function(xhr, status) {
            //alert('Petición realizada');
        }

    });
}

function VaciarFormulario(){
    $("#CarreraID").val(0);
    $("#NombreCarrera").val('');
    $("#Duracion").val('');
   
}

function Buscarcarrera(CarreraID) {
    $.ajax({
        
   
        url: '../../Carreras/BuscarCarreras',
 
        data: { CarreraID: CarreraID },
     
        type: 'GET',
 
        dataType: 'json',
    
        success: function (carreras) {

            if (carreras.length == 1) {
                let carrera12 = carreras[0];
              
                $("#NombreCarrera").val(carrera12.nombreCarrera);
                $("#CarreraID").val(carrera12.carreraID);
                $("#Duracion").val(carrera12.duracion);
                $("#Modalcarreras").modal("show");
            }
        },

    
        error: function (xhr, status) {
            alert('error al buscar la carrera');
        },

        // código a ejecutar sin importar si la petición falló o no
        complete: function (xhr, status) {
            //alert('Petición realizada');
        }
    });
}

function GuardarCarrera() {
 
    let nombreCarrera = document.getElementById("NombreCarrera").value;
    let duracion = document.getElementById("Duracion").value;
    let carreraID = $("#CarreraID").val();

    $.ajax({

        url: '../../Carreras/GuardarCarrera',
   
        data: { NombreCarrera: nombreCarrera, Duracion: duracion, CarreraID: carreraID },
    
        type: 'POST',
     
        dataType: 'json',
    
        success: function (resultado) {

            if (resultado) {
                $("#Modalcarreras").modal("hide");
                BuscarCarreras();
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


function Deshabilitar (carreraID) {
   
    $.ajax({
        // la URL para la petición
        url : '../../Carreras/Deshabilitar',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { CarreraID : carreraID },
    
        // especifica si será una petición POST o GET
        type : 'GET',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(resultado) {
            if (resultado == 1) {
              
                BuscarCarreras();
                
            }
            else { 
                    alert("No puedes deshabilitar la carrera porque esta en uso")
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