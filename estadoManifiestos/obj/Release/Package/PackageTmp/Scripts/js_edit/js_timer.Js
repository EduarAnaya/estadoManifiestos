$(function() {
  //version del codigo per detecta iactividad del usuario y ejecuta una funcion

  /*  $("._ContAccion").idleTimer(15 * 60000);
   $("._ContAccion").on("idle.idleTimer", function() {
     $(location).attr("href", "/");
   });
   $("button").on("click", function(){ */
  /*   }); */

//version del codigo para contador a cero
  var minutosRefres = 15* 60000;
  setTimeout(function() {
    actualizar();
    $(location).attr("href", "/");
  }, minutosRefres);
});
