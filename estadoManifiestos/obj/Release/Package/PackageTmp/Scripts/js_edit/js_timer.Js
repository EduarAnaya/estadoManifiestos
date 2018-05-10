$(function() {
  $("._ContAccion").idleTimer(15 * 60000);
  $("._ContAccion").on("idle.idleTimer", function() {
    $(location).attr("href", "/");
  });
});
