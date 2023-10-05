window.onload = BuscarTareas();

function BuscarTareas(){

    $("#tbody-tareas").empty();
    $.ajax({
        // la URL para la petición
        url : '../../Tareas/BuscarTareas',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { },
    
        // especifica si será una petición POST o GET
        type : 'GET',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(tareas) {
           
            $("#tbody-tareas").empty();
            $.each(tareas, function( index, tarea){

                    let EliminarTarea = 'table-success'
                    let boton = '<buttom type="button"   class="btn btn-warning btn-sm" onClick="BuscarTarea(' + tarea.tareaID + ')">Editar </buttom> ' +
                    '<buttom type="button"   class="btn btn-danger btn-sm" onClick="Deshabilitar(' + tarea.tareaID + ')">Deshabilitar </buttom> '
                    
                    if (tarea.eliminado) {
                        EliminarTarea  = 'table-danger';
                        boton =  '<buttom type="button"   class="btn btn-success btn-sm" onClick="Deshabilitar(' + tarea.tareaID + ')">Activar </buttom> '
                    }

                    $("#tbody-tareas").append('<tr class=' + EliminarTarea + '>' +
                    '<td >' + tarea.titulo + '</td>' +
                    '<td >' + tarea.descripcion + '</td>' +
                    '<td >' + tarea.fechaCargaString+ '</td>' +
                    '<td >' + tarea.fechaVencimientoString + '</td>' +
                    '<td>' + tarea.nombreProfesor + '</td>' +
                    '<td>' + tarea.nombreAsignatura + '</td>' +
                    '<td>' + boton + '</td>' +
                    '</tr>');
            }
            )
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error : function(xhr, status) {
            alert('Error al cargar tareas');
        },
   
        // código a ejecutar sin importar si la petición falló o no
        complete : function(xhr, status) {
            //alert('Petición realizada');
        }

    });
}


function VaciarFormulario(){
    $("#tituloTareas").text("Ingrese los datos de la tarea");
    $("#TareaID").val(0);
    $("#Descripcion").val('');
    $("#Titulo").val('');
    

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

    $("#FechaCarga").val(anioActual + "-" + mesActual + "-" + hoy);

    $("#FechaVencimiento").val(anioActual + "-" + mesActual + "-" + hoy);

    
}



function BuscarTarea(tareaID){

    $.ajax({
     
        url : '../../Tareas/BuscarTareas',
    
        data : { TareaID: tareaID },
    
    
        type : 'GET',
    
   
        dataType : 'json',

        success : function(tareas) {
            
            if (tareas.length == 1){
                let tarea = tareas[0];
                $("#tituloTareas").text("Edite los datos de la tarea");

               
                $("#Descripcion").val(tarea.descripcion);
                $("#Titulo").val(tarea.titulo);
                $("#TareaID").val(tarea.tareaID);
                $("#FechaCarga").val(tarea.fechaCargaStringInput);
                $("#FechaVencimiento").val(tarea.fechaVencimientoStringInput);
                $("#AsignaturaID").val(tarea.asignaturaID);
                $("#ProfesorID").val(tarea.profesorID);

                $("#ModalTarea").modal("show");
            }
            

        },

        error : function(xhr, status) {
            alert('Error al editar las tareas');
        },
    
        
        complete : function(xhr, status) {
            
        }
    });
}

function GuardarTarea(){
    let tareaID = $("#TareaID").val();
    let descripcion = $("#Descripcion").val();
 
    let fechaCarga = $("#FechaCarga").val();
    let fechaVencimiento = $("#FechaVencimiento").val();
    let Titulo = $("#Titulo").val();
    let asignaturaID = $("#AsignaturaID").val();
    let profesorID = $("#ProfesorID").val();

    $.ajax({
     
        url : '../../Tareas/GuardarTarea',
    
        data : { TareaID: tareaID, Descripcion: descripcion, FechaCarga: fechaCarga, FechaVencimiento: fechaVencimiento, Titulo: Titulo, AsignaturaID: asignaturaID, ProfesorID: profesorID },
    
    
        type : 'GET',
    
   
        dataType : 'json',

        success : function(resultado) {
            if (resultado) {

                $("#ModalTarea").modal("hide");
                BuscarTareas();
                
            }
            else{
                alert ("Ya existe esta tarea")
            }

        },

        // error : function(xhr, status) {
        //     alert('Error al cargar las tareas');
        // },
    });
}


function Deshabilitar (tareaID) {
   
    $.ajax({
        // la URL para la petición
        url : '../../Tareas/Deshabilitar',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { TareaID : tareaID },
    
        // especifica si será una petición POST o GET
        type : 'GET',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(resultado) {
            if (resultado == 1) {
              
                BuscarTareas();
                
            }
            else { 
                    alert("error al deshabilitar Tarea")
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