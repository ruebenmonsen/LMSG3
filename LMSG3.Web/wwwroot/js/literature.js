$(function () {
    $("#datepicker").datepicker({
        changeMonth: true,
        changeYear: true
    });
});

function showHideAuthors(x) {

    if (x == 1) {
        document.getElementById("secondAuthor").style.visibility = "visible";
        document.getElementById("firstPlusIcon").style.visibility = "hidden";
        document.getElementById("secondPlusIcon").style.visibility = "visible";
        document.getElementById("firstMinusIcon").style.visibility = "visible";
    } else if (x == 2) {
        document.getElementById("thirdAuthor").style.visibility = "visible";
        document.getElementById("secondPlusIcon").style.visibility = "hidden";
        document.getElementById("firstMinusIcon").style.visibility = "hidden";
        document.getElementById("secondMinusIcon").style.visibility = "visible";
    }
    if (x == -1) {
        document.getElementById("secondAuthor").style.visibility = "hidden";
        document.getElementById("firstPlusIcon").style.visibility = "visible";
        document.getElementById("secondPlusIcon").style.visibility = "hidden";
        document.getElementById("firstMinusIcon").style.visibility = "hidden";
        document.getElementById("secondMinusIcon").style.visibility = "hidden";
    } else if (x == -2) {
        document.getElementById("thirdAuthor").style.visibility = "hidden";
        document.getElementById("secondPlusIcon").style.visibility = "visible";
        document.getElementById("secondMinusIcon").style.visibility = "hidden";
        document.getElementById("firstMinusIcon").style.visibility = "visible";
    }

}
