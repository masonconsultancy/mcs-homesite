// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function hideDisplayVos(table, visible = true) {
  table.find('[data-vos="true"]').each(function (e) {
    if (visible)
      $(this).removeClass('visually-hidden');
    else
      $(this).addClass('visually-hidden');
  });
}

function getCrudMenuWithId(control, page, handler, id) {
  control.load(page + "?handler=" + handler + "&id=" + id);
}