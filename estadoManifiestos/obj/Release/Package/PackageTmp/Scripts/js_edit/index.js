$(function() {
  $("#btnRefres").on("click", function(e) {
    actualizar();
  });

  //evento que permite mostrar un collapse y ocultar los dem�s que est�n abiertos.
  $(".collapse").on("show.bs.collapse", function(e) {
    var dataID = e.target.id;
    $(".collapse").collapse("hide");
    $(dataID).collapse();
  });

  /***********EVENTOS CONSULTA DE ESTADOS MIN.T************/
  $("table").on("click", ".bagdEnviadoMt", function(e) {
    var manifiesto = this.parentNode.parentNode.cells["0"].innerText;
    var $elemento = this.parentNode.children[1];
    var entidad = 1;
    ejecutaPost(manifiesto, entidad, $elemento);
  });

  $("table").on("click", ".bagdRechazadoMt", function(e) {
    var manifiesto = this.parentNode.parentNode.cells["0"].innerText;
    var $elemento = this.parentNode.children[1];
    var entidad = 1;
    ejecutaPost(manifiesto, entidad, $elemento);
  });
  /***********EVENTOS CONSULTA DE ESTADOS DEST.S************/
  $("table").on("click", ".bagdEnviadoDs", function(e) {
    var manifiesto = this.parentNode.parentNode.cells["0"].innerText;
    var $elemento = this.parentNode.children[1];
    var entidad = 2;
    ejecutaPost(manifiesto, entidad, $elemento);
  });

  $("table").on("click", ".bagdRechazadoDs", function(e) {
    var manifiesto = this.parentNode.parentNode.cells["0"].innerText;
    var $elemento = this.parentNode.children[1];
    var entidad = 2;
    ejecutaPost(manifiesto, entidad, $elemento);
  });
  /***********EVENTOS CONSULTA DE ESTADOS OSP************/
  $("table").on("click", ".bagdEnviadoOsp", function(e) {
    var manifiesto = this.parentNode.parentNode.cells["0"].innerText;
    var $elemento = this.parentNode.children[1];
    var entidad = 3;
    ejecutaPost(manifiesto, entidad, $elemento);
  });

  $("table").on("click", ".bagdRechazadoOsp", function(e) {
    var manifiesto = this.parentNode.parentNode.cells["0"].innerText;
    var $elemento = this.parentNode.children[1];
    var entidad = 3;
    ejecutaPost(manifiesto, entidad, $elemento);
  });
  /***********EVENTOS CONSULTA DE ESTADOS************/
  function ejecutaPost(manifiesto, entidad, $elemento) {
    $.post("/manifiestos/estadomManifiesto", {
      planilla: manifiesto,
      entidad: entidad
    })
      .done(function(response) {
        renderRespuesta(response, $elemento);
      })
      .fail(function() {
        alert("error");
      });
  }

  function renderRespuesta(response, $elemento) {
    var $coll = $elemento;
    var resultado = null;
    if (response["0"].idMin == "") {//rechazado
      resultado = $(
        '<div class="card card-body">' +
          '<dl class="row" style="margin: 0;">' +
          '<dt class="col-5 marginTituloDesc">Oficina</dt>' +
          '<dd class="col-5 marginDescTtulo">' +
          response["0"].oficina +
          "</dd>" +
          '<dt class="col-5 marginTituloDesc">Fecha Envio</dt>' +
          '<dd class="col-5 marginDescTtulo">' +
          response["0"].fecha +
          "</dd>" +
          '<dt class="col-5 marginTituloDesc">Respuesta. Prov.</dt>' +
          '<dd class="col-5 marginDescTtulo">' +
          response["0"].respuesta +
          "</dd>" +
          "</dl>" +
          "</div>"
      );
    }else{
      resultado = $(
        '<div class="card card-body">' +
          '<dl class="row" style="margin: 0;">' +
          '<dt class="col-5 marginTituloDesc">Oficina</dt>' +
          '<dd class="col-5 marginDescTtulo">' +
          response["0"].oficina +
          "</dd>" +
          '<dt class="col-5 marginTituloDesc">Fecha Envio</dt>' +
          '<dd class="col-5 marginDescTtulo">' +
          response["0"].fecha +
          "</dd>" +
          '<dt class="col-5 marginTituloDesc">Nro. Aprov.</dt>' +
          '<dd class="col-5 marginDescTtulo">' +
          response["0"].idMin +
          "</dd>" +
          "</dl>" +
          "</div>"
      );
    }

    $($coll).html("");
    $($coll).append(resultado);
  }
  /*busqueda por demanda*/
  let _ultimaPlanilla = null;
  $("#searchManifiesto").on("submit", function(e) {
    e.preventDefault();
    var manifiesto = $("#inputManifiesto")
      .val()
      .toUpperCase();
    _ultimaPlanilla = manifiesto;
    $("#inputManifiesto").val("");
    postDemanda(manifiesto);
  });

  $("#repeatSearch").on("click", function(e) {
    postDemanda(_ultimaPlanilla);
  });

  $("#clearSearch").on("click", function() {
    _ultimaPlanilla = null;
    $("#tablaSearch tbody").html("");
    $("#cajatablasearch").addClass("visible");
  });

  function postDemanda(manifiesto) {
    $.post("/manifiestos/demanda", {
      planilla: manifiesto
    })
      .done(function(response) {
        renderTable(response);
      })
      .fail(function() {
        alert("error");
      });
  }

  function renderTable(response) {
    if (response.length != 0) {
      var $destino = $("#tablaSearch tbody");
      for (let index = 0; index < response.length; index++) {
        var _registro = response[index];
        var nrPlanilla = _registro.nroPlanilla;
        var fecgen = _registro.fechaGen;
        var oficina = _registro.oficina;

        var stmint = _registro.estMinisterio;
        var stdests = _registro.estDestseguro;
        var stosp = _registro.estOsp;

        var tdMint, tdDsts, tdOsp;

        switch (stmint) {
          case 1: //enviado
            tdMint =
              '<span class="badge badge-pill badge-success bagdEnviadoMt" data-toggle="collapse" href="#enMt' +
              nrPlanilla +
              '"' +
              'role="button" aria-expanded="false">Enviado</span>' +
              '<div class="collapse" id="enMt' +
              nrPlanilla +
              '"></div>';
            break;
          case 2: //Pendiente
            tdMint =
              '<span class="badge badge-pill badge-warning bagdPendienteMt"  data-toggle="collapse" href="#pMt' +
              nrPlanilla +
              '" role="button" aria-expanded="false">Pendiente</span>' +
              '<div class="collapse" id="pMt' +
              nrPlanilla +
              '">' +
              '<div class="card card-body">' +
              "<p>Envío en proceso, el tiempo estimado de envió es de 5 minutos por favor espere, en caso de persistir este estado por favor <br />" +
              '<a href="mailto:soporte@transer.com.co?Subject=Manifiesto ' +
              nrPlanilla +
              ' tarda en subir">informar a sistemas</a> ' +
              "</p>" +
              "</div>" +
              "</div>";
            break;
          case 3: //rechazado
            tdMint =
              '<span class="badge badge-pill badge-danger bagdRechazadoMt" data-toggle="collapse" href="' +
              "#rcMt" +
              nrPlanilla +
              '"' +
              ' role="button" aria-expanded="false">Rechazado</span>' +
              '<div class="collapse" id="' +
              "rcMt" +
              nrPlanilla +
              '"' +
              ">" +
              "</div>";
            break;
          case 4: //desconocido
            tdMint =
              '<span class="badge badge-pill badge-danger bagdncMt" data-toggle="collapse" href="#nrMt' +
              nrPlanilla +
              '" role="button" aria-expanded="false">No Catalogado</span>' +
              '<div class="collapse" id="' +
              nrPlanilla +
              '">' +
              '<div class="card card-body">' +
              "<p>Error descnocido, por favor <br />" +
              '<a href="@("mailto:soporte@transer.com.co?Subject=" + "Manifiesto "' +
              nrPlanilla +
              '" retorna error desconocido para el ministerio")">informar a sistemas</a>' +
              "</p>" +
              "</div>" +
              "</div>";
            break;
          case 5: //urbano
            tdMint =
              '<span class="badge badge-pill badge-light bagdNaMt" data-toggle="collapse" href="#naMt' +
              nrPlanilla +
              '" role="button" aria-expanded="false">Urbano</span>' +
              '<div class="collapse" id="naMt' +
              nrPlanilla +
              '">' +
              '<div class="card card-body">' +
              "<p>Los viajes urbanos no son reportados al Ministerio</p>" +
              "</div>" +
              "</div>";
            break;
        }
        switch (stdests) {
          case 1: //enviado
            tdDsts =
              '<span class="badge badge-pill badge-success bagdEnviadoDs" data-toggle="collapse" href="#enDs' +
              nrPlanilla +
              '" role="button" aria-expanded="false">Enviado</span>' +
              '<div class="collapse" id="enDs' +
              nrPlanilla +
              '">' +
              "</div>";
            break;
          case 2: //pendiente
            tdDsts =
              '<span class="badge badge-pill badge-warning bagdPendienteDs"  data-toggle="collapse" href="#pDs' +
              nrPlanilla +
              '" role="button" aria-expanded="false">Pendiente</span>' +
              '<div class="collapse" id="pDs' +
              nrPlanilla +
              '">' +
              '<div class="card card-body">' +
              "<p>Envío en proceso, el tiempo estimado de envió es de 5 minutos por favor espere, en caso de persistir este estado por favor <br />" +
              '<a href="mailto:soporte@transer.com.co?Subject=Manifiesto ' +
              nrPlanilla +
              ' tarda en subir">informar a sistemas</a> ' +
              "</p></div></div>";
            break;
          case 3: //rechazado
            tdDsts =
              '<span class="badge badge-pill badge-danger bagdRechazadoDs" data-toggle="collapse" href="#rcDs' +
              nrPlanilla +
              '" role="button" aria-expanded="false">Rechazado</span>' +
              '<div class="collapse" id="rcDs' +
              nrPlanilla +
              '"></div>';
            break;
          case 4: //desconocido
            tdDsts =
              '<span class="badge badge-pill badge-danger bagdncDs" data-toggle="collapse" href="#nrDs' +
              nrPlanilla +
              '" role="button" aria-expanded="false">No Catalogado</span>' +
              '<div class="collapse" id="nrDs' +
              nrPlanilla +
              '">' +
              '<div class="card card-body">' +
              "<p>Error descnocido, por favor <br />" +
              '<a href="@("mailto:soporte@transer.com.co?Subject=" + "Manifiesto "' +
              nrPlanilla +
              '" retorna error desconocido para el ministerio")">informar a sistemas</a>' +
              "</p></div></div>";
            break;
          case 6: //Propio
            tdDsts =
              '<span class="badge badge-pill badge-light bagdnaDs" data-toggle="collapse" href="#naDs' +
              nrPlanilla +
              '" role="button" aria-expanded="false">Propio</span>' +
              '<div class="collapse" id="naDs' +
              nrPlanilla +
              '">' +
              '<div class="card card-body">' +
              "<p>Los viajes de vehículos Propios no son reportados a Destino Seguro</p>" +
              "</div></div>";
            break;
        }
        switch (stosp) {
          case 1: //enviado
            tdOsp =
              '<span class="badge badge-pill badge-success bagdEnviadoOsp" data-toggle="collapse" href="#enOsp' +
              nrPlanilla +
              '" role="button" aria-expanded="false">Enviado</span>' +
              '<div class="collapse" id="enOsp' +
              nrPlanilla +
              '"></div>';
            break;
          case 2: //pendiente
            tdOsp =
              '<span class="badge badge-pill badge-warning bagdPendienteOsp"  data-toggle="collapse" href="#pOsp' +
              nrPlanilla +
              '" role="button" aria-expanded="false">Pendiente</span>' +
              '<div class="collapse" id="pOsp' +
              nrPlanilla +
              '">' +
              '<div class="card card-body">' +
              "<p>Envío en proceso, el tiempo estimado de envió es de 5 minutos por favor espere, en caso de persistir este estado por favor <br />" +
              '<a href="@("mailto:soporte@transer.com.co?Subject=" + "Manifiesto "' +
              nrPlanilla +
              '" tarda en subir")">informar a sistemas</a>' +
              "</p></div></div>";
            break;
          case 3: //rechazado
            tdOsp =
              '<span class="badge badge-pill badge-danger bagdRechazadoOsp" data-toggle="collapse" href="#rcOsp' +
              nrPlanilla +
              '" role="button" aria-expanded="false">Rechazado</span>' +
              '<div class="collapse" id="rcOsp' +
              nrPlanilla +
              '"></div>';
            break;
          case 4: //descnocido
            tdOsp =
              '<span class="badge badge-pill badge-danger bagdncOsp" data-toggle="collapse" href="#nrOsp' +
              nrPlanilla +
              '" role="button" aria-expanded="false">No Catalogado</span>' +
              '<div class="collapse" id="nrOsp' +
              nrPlanilla +
              '">' +
              '<div class="card card-body">' +
              "<p>Error descnocido, por favor <br />" +
              '<a href="@("mailto:soporte@transer.com.co?Subject=" + "Manifiesto " ' +
              nrPlanilla +
              '" retorna error desconocido para el ministerio")">informar a sistemas</a></p></div></div>';
            break;
          case 6:
            tdOsp =
              '<span class="badge badge-pill badge-light bagdnaOsp" data-toggle="collapse" href="#naOsp' +
              nrPlanilla +
              '" role="button" aria-expanded="false">Tercero</span>' +
              '<div class="collapse" id="naOsp' +
              nrPlanilla +
              '">' +
              '<div class="card card-body">' +
              "<p>Los viajes de vehículos terceros no son reportados a OSP</p></div></div>";
            break;
        }

        var tr = $(
          "<tr>" +
            "<td>" +
            _registro.nroPlanilla +
            "</td>" +
            "<td>" +
            _registro.fechaGen +
            "</td>" +
            "<td>" +
            _registro.oficina +
            "</td>" +
            "<td>" +
            tdMint +
            "</td>" +
            "<td>" +
            tdDsts +
            "</td>" +
            "<td>" +
            tdOsp +
            "</td>" +
            "</tr>"
        );
      }
      $destino.html("");
      $destino.append(tr);
      $("#cajatablasearch").removeClass("visible");
    } else {
      $("#cajatablasearch").addClass("visible");
    }
  }
});

function actualizar() {
  $("#btnRefres").addClass("disabled");
  var estado = $(
    '<div class="alert alert-primary" role="alert">' +
      "<strong>Procesando consulta, por favor espere...</strong>" +
      '</div>"'
  );
  $("#estData").append(estado);
}
