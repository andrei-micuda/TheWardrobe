$(document).ready(function () {
    var priceSpinnerProps = {
        decrementButton: "<strong>&minus;</strong>", // button text
        incrementButton: "<strong>&plus;</strong>", // ..
        groupClass: "", // css class of the resulting input-group
        buttonsClass: "btn-outline-secondary",
        buttonsWidth: "2.5rem",
        textAlign: "center", // alignment of the entered number
        autoDelay: 500, // ms threshold before auto value change
        autoInterval: 50, // speed of auto value change
        buttonsOnly: false, // set this `true` to disable the possibility to enter or paste the number via keyboard
        locale: navigator.language, // the locale, per default detected automatically from the browser
        template: // the template of the input
            '<div class="input-group ${groupClass}">' +
            '<div class="input-group-prepend"><span class="input-group-text">$</span></div>' +
            '<input id="Price" name="Price" type="text" inputmode="decimal" style="text-align: ${textAlign}" class="form-control" data-decimals="2" min="1" max="2000" step="1" />' +
            '<div class="input-group-append"><button style="min-width: ${buttonsWidth}" class="btn btn-decrement ${buttonsClass}" type="button">${decrementButton}</button><button style="min-width: ${buttonsWidth}" class="btn btn-increment ${buttonsClass}" type="button">${incrementButton}</button></div>' +
            '</div>'
    }
    $('.price-spinner').inputSpinner(priceSpinnerProps);

    $('#productForm').submit(function (event) {
        var form = $('#productForm');
        event.preventDefault();
        var sizesString = $('#productForm').serializeArray().filter(obj => obj.name == "SizeArray").map(obj => obj.value).join(" ");

        $('#productForm #Size').val(sizesString);
        form.off("submit");
        form.submit();

        return true;
    });
});