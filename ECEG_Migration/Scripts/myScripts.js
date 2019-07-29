function enableBootstrapSelect() {
    $(".multi").selectpicker();
}

enableBootstrapSelect();

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

/*********************************scroll top button management*************************************/
showTopButton();


    $(document).scroll(function () {
        showTopButton();
    });

    function showTopButton() {
        if (document.body.scrollTop > 150 || document.documentElement.scrollTop > 150)
            $(".top-button").css({ "opacity": "1", "bottom": "5%"});


        //$(".top-button").css("opacity", "1");
        else
            $(".top-button").css({ "opacity": "0", "bottom": "-5%" });
            //$(".top-button").css("opacity", "0");
    }

    // When the user clicks on the button, scroll to the top of the document
    $(".top-button").click(() => {
        $("html, body").animate({ scrollTop: 0 }, 250);
    })