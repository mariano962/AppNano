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
                    '<td >' + alumno.nombre + '</td>' +
                    '<td >' + alumno.direccion + '</td>' +
                    '<td >' + alumno.dniAlumno + '</td>' +
                    '<td >' + alumno.correo + '</td>' +
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
    $("#AlumnoID").val(0);
    $("#Nombre").val('');
    $("#CarreraID").val('');
    $("#DniAlumno").val('');
    $("#Correo").val('');
    $("#Direccion").val('');

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

function BuscarAlumno(alumnoID) {
    console.log(alumnoID);
    $.ajax({
        
   
        url: '../../Alumnos/BuscarAlumnos',
 
        data: { AlumnoID: alumnoID },
     
        type: 'GET',
 
        dataType: 'json',
    
        success: function (alumnos) {

            if (alumnos.length == 1) {
                let alumno12 = alumnos[0];
              
                $("#AlumnoID").val(alumno12.alumnoID);
                $("#Nombre").val(alumno12.nombre);
                $("#NacimientoAlumno").val(alumno12.nacimientoAlumnoStringInput);
                $("#CarreraID").val(alumno12.carreraID);
                $("#DniAlumno").val(alumno12.dniAlumno);
                $("#Correo").val(alumno12.correo);
                $("#Direccion").val(alumno12.direccion);
                

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
    let alumnoID = $("#AlumnoID").val();
    let dniAlumno = $("#DniAlumno").val();
    let correo = $("#Correo").val();
    let direccion = $("#Direccion").val();

    $.ajax({

        url: '../../Alumnos/GuardarAlumno',
   
        data: { Nombre: nombre, CarreraID: carreraID, NacimientoAlumno: nacimientoAlumno, AlumnoID: alumnoID, DniAlumno: dniAlumno, Correo: correo, Direccion: direccion},
    
        type: 'POST',
     
        dataType: 'json',
    
        success: function (resultado) {

            if (resultado) {
                $("#ModalAlumno").modal("hide");
                BuscarAlumnos();
                
            }
            else {
               
                 alert("ya existe un alumno con ese Dni")
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
                    alert("error al deshabilitar alumno")
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

function Imprimir() {
    var doc = new jsPDF();
    //var doc = new jsPDF('l', 'mm', [297, 210]);

    var totalPagesExp = "{total_pages_count_string}";
    var pageContent = function (data) {

        var pageHeight = doc.internal.pageSize.height || doc.internal.pageSize.getHeight();
        var pageWidth = doc.internal.pageSize.width || doc.internal.pageSize.getWidth();

        // FOOTER
        var str = "Pagina " + data.pageCount;
        // Total page number plugin only available in jspdf v1.0+
        if (typeof doc.putTotalPages == 'function') {
            str = str + " de " + totalPagesExp;
        }

        doc.setLineWidth(8);
        doc.setDrawColor(238, 238, 238);
        doc.line(14, pageHeight - 11, 196, pageHeight - 11);

        doc.setFontSize(10);


        doc.setFontStyle('bold');

        doc.text(str, 17, pageHeight - 10);

    };


    var elem = document.getElementById("tabla-imprimir");
    var res = doc.autoTableHtmlToJson(elem);
    doc.autoTable(res.columns, res.data,
        {
            addPageContent: pageContent,
            theme: 'grid',
            //styles: { fillColor: [255, 0, 0] }, //COLOR ENCABEZADO
            columnStyles: {
                0: { halign: 'center',
                     cellWidth: 100,
                     fontSize: 7,
                     //fillColor: [0, 255, 0]
                   },
                1: {  fontSize: 7, overflow: 'hidden' }
            }, // Celdas de la primera columna centradas y verdes
            margin: { top: 10 },
        }
    );

    // ESTO SE LLAMA ANTES DE ABRIR EL PDF PARA QUE MUESTRE EN EL PDF EL NRO TOTAL DE PAGINAS. ACA CALCULA EL TOTAL DE PAGINAS.
    if (typeof doc.putTotalPages === 'function') {
        doc.putTotalPages(totalPagesExp);
    }

    //doc.save('InformeSistema.pdf')

    var string = doc.output('datauristring');
    var iframe = "<iframe width='100%' height='100%' src='" + string + "'></iframe>"

    var x = window.open();
    x.document.open();
    x.document.write(iframe);
    x.document.close();
}