$(document).ready(function () {

  let table1 = new DataTable('#defaultDemoTable', {
  });

  let table2 = new DataTable('#columnRenderDemoTable', {
    columns: [
      {
        data: 'id'
      },
      {
        data: 'name'
      },
      {
        data: 'email'
      },
      {
        data: 'userName'
      },
      {
        data: 'password',
        render: function (data, type) {
          if (type === 'display') {
            return '************';
          }
          return data;
        }
      }, {
        data: ''
      }]
  });

});