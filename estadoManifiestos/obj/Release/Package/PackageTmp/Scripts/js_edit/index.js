
$(function () {
    var dates = new Date();
    var dia = dates.getDay();
    var mes = dates.getMonth();
    var ano = dates.getFullYear();
    var n = dates.getSeconds();
    var hora = dates.getHours();
    var minutos = dates.getMinutes();
    var seg = dates.getSeconds();


    var _fecha = dia + "/" + mes + "/" + ano + "  " + hora + ":" + minutos + ":" + seg;
    document.getElementById("date").innerHTML = _fecha;
    $("#btnRefres").on("click", function (e) {
        $(this).addClass("disabled");


        var estado = $('<div class="alert alert-primary" role="alert">'
       + '<strong>Procesando consulta, por favor espere...</strong>'
      + '</div>"');
        $("#estData").append(estado);
    });
});