$(document).ready(function () {

    $('.filter').each(function () {
        let input = this;
        let table = document.getElementById(input.dataset.table);

        input.onkeyup = function (event) {

            if (event.key === "Enter") {
                event.preventDefault();
                event.stopPropagation();
                return false;
            }

            let value = input.value.toUpperCase();
            let rows = table.getElementsByClassName('search-row');

            for (let i = 0; i < rows.length; i++) {
                let row = rows[i];

                let match =
                    new RegExp(value, "i").test(row.textContent) ||
                    value === '';

                if (match) {
                    row.style.display = 'table-row';
                } else {
                    row.style.display = 'none';
                }
            }
        }; // keyup
    })


}); // ready