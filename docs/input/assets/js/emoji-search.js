$(document).ready(function () {
    let input = document.getElementById('emoji-search');
    let table = document.getElementById('emoji-results');

    input.onkeyup = function (event) {

        if (event.key === "Enter") {
            event.preventDefault();
            event.stopPropagation();
            return false;
        }

        let value = input.value.toUpperCase();
        let rows = table.getElementsByClassName('emoji-row');

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
}); // ready