window.onload = BuscarAlumnos();

function BuscarAlumnos(){

    $("#tbody-alumno").empty();
    $.ajax({
        // la URL para la petición
        url : '../../Alumnos/BuscarAlumnos',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { },
    
        // especifica si será una petición POST o GET
        type : 'POST',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(alumnos) {
            console.log(alumnos)
            $("#tbody-alumno").empty();
            $.each(alumnos, function( index, alumno){

                    let Eliminaralumno = 'table-success'
                    let boton = '<buttom type="button"   class="btn btn-warning btn-sm" onClick="BuscarAlumno(' + alumno.alumnoID + ')">Editar </buttom> ' +
                    '<buttom type="button"   class="btn btn-danger btn-sm" onClick="Deshabilitar(' + alumno.alumnoID + ')">Deshabilitar </buttom> '
                    
                    if (alumno.eliminado) {
                        Eliminaralumno  = 'table-danger';
                        boton =  '<buttom type="button"   class="btn btn-success btn-sm" onClick="Deshabilitar(' + alumno.alumnoID + ')">Activar </buttom> '
                    }

                    $("#tbody-alumno").append('<tr class=' + Eliminaralumno + '>' +
                    '<td>' + alumno.nombre + '</td>' +
                    '<td>' + alumno.nombreCarrera + '</td>' +
                    '<td>' + alumno.nacimientoAlumnoString + '</td>' +
                    '<td>' + boton + '</td>' +
                    '</tr>');
            }
            )
        },

       
        error : function(xhr, status) {
            alert('Error al cargar Alumnos');
        },
   
        // código a ejecutar sin importar si la petición falló o no
        complete : function(xhr, status) {
            //alert('Petición realizada');
        }

    });
}

function VaciarFormulario(){
    $("#AlumnoID");
    $("#Nombre").val('');
    $("#CarreraID").val('');

    let fecha = new Date();
    let anioActual = fecha.getFullYear();
    let mesActual = fecha.getMonth() + 1;
    if(mesActual < 10){
        mesActual = "0" + mesActual;
    }

    let hoy = fecha.getDate();

    if(hoy < 10){
        hoy = "0" + hoy;
    }

    let hora = fecha.getHours(); 

    if(hora < 10){
        hora = "0" + hora;
    }

    let min = fecha.getMinutes(); 

    if(min < 10){
        min = "0" + min;
    }

    $("#NacimientoAlumno").val(anioActual + "-" + mesActual + "-" + hoy);
   
}

function BuscarAlumno(AlumnoID) {
    $.ajax({
        
   
        url: '../../Alumnos/BuscarAlumnos',
 
        data: { AlumnoID: AlumnoID },
     
        type: 'GET',
 
        dataType: 'json',
    
        success: function (alumnos) {

            if (alumnos.length == 1) {
                let alumno12 = alumnos[0];
              
                $("#Nombre").val(alumno12.nombreAlumno);
                $("#NacimientoAlumno").val(alumno12.nacimientoAlumnoStringInput);
                $("#CarreraID").val(alumno12.carreraID);
                
                $("#ModalAlumno").modal("show");
            }
        },

    
        error: function (xhr, status) {
            alert('Error al cargar alumno');
        },

        // código a ejecutar sin importar si la petición falló o no
        complete: function (xhr, status) {
            //alert('Petición realizada');
        }
    });
}

function GuardarAlumno() {
    
 
    
    let nombre = document.getElementById("Nombre").value;
    let nacimientoAlumno = document.getElementById("NacimientoAlumno").value;
   
    let carreraID = $("#CarreraID").val();

    $.ajax({

        url: '../../Alumnos/GuardarAlumno',
   
        data: { Nombre: nombre, CarreraID: carreraID, NacimientoAlumno: nacimientoAlumno },
    
        type: 'POST',
     
        dataType: 'json',
    
        success: function (resultado) {

            if (resultado) {
                $("#ModalAlumno").modal("hide");
                BuscarAlumnos();
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


function Deshabilitar (alumnoID) {
   
    $.ajax({
        // la URL para la petición
        url : '../../Alumnos/Deshabilitar',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { AlumnoID : alumnoID },
    
        // especifica si será una petición POST o GET
        type : 'GET',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(resultado) {
            if (resultado == 1) {
              
                BuscarAlumnos();
                
            }
            else { 
                    alert("error la deshabilitar la carrera")
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