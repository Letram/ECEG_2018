// Usuarios » Row Command (Select)
$(document).on("click", ".all_grammar_table tr:not(:first-child)", function () {

    var indiceFila = $(this).index();
    console.log({ index: indiceFila })
    //javascript: __doPostBack('ctl00$MainContent$TablaUsuarios', 'Select$' + (indiceFila - 1));

});