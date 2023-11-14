window.onload = BuscarProfesores();

function BuscarProfesores(){

    $("#tbody-profesor").empty();
    $.ajax({
        // la URL para la petición
        url : '../../Profesores/BuscarProfesores',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { },
    
        // especifica si será una petición POST o GET
        type : 'POST',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(profesores) {

            $("#tbody-profesor").empty();
            $.each(profesores, function( index, profesor){

                    let EliminarProfesor = 'table-success'
                    let boton = '<buttom type="button"   class="btn btn-warning btn-sm" onClick="BuscarProfesor(' + profesor.profesorID + ')">Editar </buttom> ' +
                    '<buttom type="button"   class="btn btn-danger btn-sm" onClick="Deshabilitar(' + profesor.profesorID + ')">Deshabilitar </buttom> ' +
                    '<buttom type="button"   class="btn btn-success btn-sm" onClick="VerMaterias(' + profesor.profesorID + ')">Materias </buttom> '
                    
                    if (profesor.eliminado) {
                        EliminarProfesor  = 'table-danger';
                        boton =  '<buttom type="button"   class="btn btn-success btn-sm" onClick="Deshabilitar(' + profesor.profesorID + ')">Activar </buttom> '
                    }

                    $("#tbody-profesor").append('<tr class=' + EliminarProfesor + '>' +
                    '<td>' + profesor.nombre + '</td>' +
                    '<td>' + profesor.direccion + '</td>' +
                    '<td>' + profesor.dniProfesor + '</td>' +
                    '<td>' + profesor.correoElectronico + '</td>' +
                    '<td>' + profesor.nacimientoProfesorString + '</td>' +
                    '<td>' + boton + '</td>' +
                    '</tr>');
            }
            )
        },

       
        error : function(xhr, status) {
            alert('Error al cargar boxes');
        },
   
        // código a ejecutar sin importar si la petición falló o no
        complete : function(xhr, status) {
            //alert('Petición realizada');
        }

    });
}

function VaciarFormulario(){
    $("#ProfesorID").val(0);
    $("#Nombre").val('');
    $("#Direccion").val('');
    $("#DniProfesor").val('');
    $("#CorreoElectronico").val('');
    $("#NacimientoProfesor").val('');

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

    $("#NacimientoProfesor").val(anioActual + "-" + mesActual + "-" + hoy);
   
}

function BuscarProfesor(profesorID) {
    $.ajax({
        
   
        url: '../../Profesores/BuscarProfesores',
 
        data: { ProfesorID: profesorID },
     
        type: 'GET',
 
        dataType: 'json',
    
        success: function (profesores) {

            if (profesores.length == 1) {
                let profesores12 = profesores[0];
              
                $("#Nombre").val(profesores12.nombre);
                $("#ProfesorID").val(profesores12.profesorID);
                $("#DniProfesor").val(profesores12.dniProfesor);
                $("#Direccion").val(profesores12.direccion);
                document.getElementById("CorreoElectronico").disabled = true;
                $("#CorreoElectronico").val(profesores12.correoElectronico);
                $("#NacimientoProfesor").val(profesores12.nacimientoProfesorStringInput);

                $("#ModalProfesor").modal("show");
            }
        },

    
        error: function (xhr, status) {
            alert('ERROR AL CARGAR PROFESOR');
        },

        // código a ejecutar sin importar si la petición falló o no
        complete: function (xhr, status) {
            //alert('Petición realizada');
        }
    });
}

function GuardarProfesor() {
 
    let profesorID = $("#ProfesorID").val();
    let nombre = $("#Nombre").val();
    let correoElectronico = $("#CorreoElectronico").val();
    let nacimientoProfesor = $("#NacimientoProfesor").val();
    let dniProfesor = $("#DniProfesor").val();
    let direccion = $("#Direccion").val();
    document.getElementById("CorreoElectronico").disabled = false;

    $.ajax({

        url: '../../Profesores/GuardarProfesor',
   
        data: { ProfesorID: profesorID, Nombre: nombre, CorreoElectronico: correoElectronico, NacimientoProfesor: nacimientoProfesor, DniProfesor: dniProfesor, Direccion: direccion },
    
        type: 'POST',
     
        dataType: 'json',
    
        success: function (resultado) {

            if (resultado) {
                $("#ModalProfesor").modal("hide");
                BuscarProfesores();
                alert("Profesor Guardado")
            }
            else {
               
                 alert("Ya hay un profesor con ese dni")
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

function Deshabilitar (profesorID) {
   
    $.ajax({
        // la URL para la petición
        url : '../../Profesores/Deshabilitar',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { ProfesorID : profesorID },
    
        // especifica si será una petición POST o GET
        type : 'GET',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(resultado) {
            if (resultado == 1) {
              
                BuscarProfesores();
                
            }
            else { 
                    alert("error al deshabilitar profesores")
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

//PARTE DE MATERIAS

function VerMaterias(profesorID) {
    $.ajax({
        
   
        url: '../../Profesores/BuscarProfesores',
 
        data: { ProfesorID: profesorID },
     
        type: 'GET',
 
        dataType: 'json',
    
        success: function (profesores) {

            if (profesores.length == 1) {
                let profesores12 = profesores[0];
                
                $("#ProfesorAsignatruaID").val(profesores12.profesorID);
                 BuscarMaterias();
                $("#ModalMaterias").modal("show");
            }
        },

    
        error: function (xhr, status) {
            alert('error al cargar las materias');
        },

        // código a ejecutar sin importar si la petición falló o no
        complete: function (xhr, status) {
            //alert('Petición realizada');
        }
    });
}


function BuscarMaterias(){

    let asignaturaProfesorID = $("#ProfesorAsignatruaID").val();

    $("#tbody-Materias").empty();
    $.ajax({
        // la URL para la petición
        url : '../../Profesores/BuscarMaterias',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { AsignaturaProfesorID: asignaturaProfesorID },
    
        // especifica si será una petición POST o GET
        type : 'POST',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(asignaturaProfesorMostrar) {

            $("#tbody-Materias").empty();
            $.each(asignaturaProfesorMostrar, function( index, asignatura){

                    let boton = '<buttom type="button"   class="btn btn-danger btn-sm" onClick="EliminarMateria(' + asignatura.asignaturaProfesorID + ')">Eliminar </buttom> ' 
                  
                   

                   
                    $("#tbody-Materias").append('<tr>' +

                    '<td>' + asignatura.nombreAsignatura + '</td>' +
                   
                    '<td>' + boton + '</td>' +
                    '</tr>');
            }
            )
        },

       
        error : function(xhr, status) {
            alert('Error al buscar profesores');
        },
   
        // código a ejecutar sin importar si la petición falló o no
        complete : function(xhr, status) {
            //alert('Petición realizada');
        }

    });
}

function GuardarMateria() {
 
    let profesorID = $("#ProfesorAsignatruaID").val();
    let asignaturaID = $("#AsignaturaID").val();
   

    $.ajax({

        url: '../../Profesores/GuardarMateria',
   
        data: {AsignaturaID: asignaturaID, ProfesorID: profesorID },
    
        type: 'POST',
     
        dataType: 'json',
    
        success: function (resultado) {

            if (resultado) {
                $("#ModalMaterias").modal("show");
                BuscarMaterias();
               
            }
            else {
               
                 alert("No se puede guardar")
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

function EliminarMateria (asignaturaProfesorID) {
   
    $.ajax({
        // la URL para la petición
        url : '../../Profesores/EliminarMateria',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { AsignaturaProfesorID : asignaturaProfesorID },
    
        // especifica si será una petición POST o GET
        type : 'GET',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(resultado) {
            if (resultado == true) {
              
                BuscarMaterias();
                
            }
            else { 
                    alert("error al deshabilitar la materia")
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