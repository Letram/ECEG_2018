// Usuarios » Row Command (Select)
$(document).on("click", ".all_grammar_table tr:not(:first-child)", function () {

    var indiceFila = $(this).index();
    console.log({ index: indiceFila })
    //javascript: __doPostBack('ctl00$MainContent$TablaUsuarios', 'Select$' + (indiceFila - 1));

});

var animateButton = function (e) {

    e.preventDefault;
    //reset animation
    e.target.classList.remove('animate');

    e.target.classList.add('animate');
    setTimeout(function () {
        e.target.classList.remove('animate');
    }, 700);
};

var bubblyButtons = document.getElementsByClassName("bubbly-button");

for (var i = 0; i < bubblyButtons.length; i++) {
    bubblyButtons[i].addEventListener('click', animateButton, false);
}

// When the user clicks on the button, scroll to the top of the document
$(".top-button").click(() => {
    $("html, body").animate({ scrollTop: $("#top").offset().top }, 800, function () {
        window.location.hash = "top";
    });
})