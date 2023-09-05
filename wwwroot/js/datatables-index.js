$(document).ready(function () {

  const ajaxDataDemoTableUrl = $("#ajaxDataDemoTable").data("url");

  const table1 = new DataTable("#defaultDemoTable", {
    "autoWidth": true,
    "deferRender": true,
    columns: [
      {
        data: "id"
      },
      {
        data: "name"
      },
      {
        data: "email"
      },
      {
        data: "userName"
      },
      {
        data: "password"
      }]
  });

  const table2 = new DataTable("#columnRenderDemoTable", {
    columns: [
      {
        data: "id"
      },
      {
        data: "name"
      },
      {
        data: "email"
      },
      {
        data: "userName"
      },
      {
        data: "password",
        render: function (data, type) {
          if (type === "display") {
            return "************";
          }
          return data;
        }
      }]
  });

  const table3 = new DataTable("#ajaxDataDemoTable", {
    "ajax": {
      "url": ajaxDataDemoTableUrl,
      "type": "GET",
      "dataSrc": ""
    },
    columns: [
      {
        data: "id"
      },
      {
        data: "name"
      },
      {
        data: "email"
      },
      {
        data: "userName"
      },
      {
        data: "password",
        render: function (data, type) {
          if (type === "display") {
            return "************";
          }
          return data;
        }
      }]
  });

  const table4 = new DataTable("#userDataTable", {
    "ajax": {
      "url": $("#userDataTable").data("url"),
      "type": "GET",
      "dataSrc": ""
    },
    columns: [
      {
        data: "id"
      },
      {
        data: "name"
      },
      {
        data: "email"
      },
      {
        data: "userName"
      },
      {
        data: "password",
        render: function (data, type) {
          if (type === "display") {
            return "************";
          }
          return data;
        }
      }]
  });

  $('[data-crud-table]').each(function (e, x) {
    const index = $(x).data("crud-index");
    const menuPlaceHolder = "#" + $(x).attr("id");
    const table = $("#" + $(x).data("crud-table"));
    table.on("click", "tbody tr", (tableObject) => {
      let id = 0;
      if ($(tableObject.currentTarget).hasClass("table-active")) {
        $(tableObject.currentTarget).removeClass("table-active").siblings().removeClass("table-active");
      }
      else {
        $(tableObject.currentTarget).addClass("table-active").siblings().removeClass("table-active");
        const node = table.DataTable().rows('.table-active');
        id = table.DataTable().row(node).data().id;
      }
      getCrudMenuWithId($(menuPlaceHolder), "/DataTables/Index", "CrudMenuPartial", id);
    });
    getCrudMenuWithId($(menuPlaceHolder), "/DataTables/Index", "CrudMenuPartial", 0);
  });

  $("#migrateData").on("click", function() {
    $("#migrationDataPlaceholder").load("/DataMigration?handler=MigrationLogsPartial", function() {
      $("#userDataTable").DataTable().ajax.reload();
    });
    
  });

});