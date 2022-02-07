const dropList = document.querySelectorAll(".drop-list select");

    //fromCurrency = document.querySelector(".from select"),
    //toCurrency = document.querySelector(".to select"),
    //getButton = document.querySelector("form button");



/*Loop through the list of countries to display the countries the CONSOLE*/
for (let i = 0; i < dropList.length; i++) {
    for (let currency_code in country_list) {

        //By default set currency selected to USD and NPR
        let selected = i == 0 ? currency_code == "USD" ? "selected" : "" : currency_code == "NPR" ? "selected" : "";

        //Dropdown box to see all the currencies available
        let optionTag = `<option value="${currency_code}" ${selected}>${currency_code}</option>`;
        dropList[i].insertAdjacentHTML("beforeend", optionTag);
    }
    dropList[i].addEventListener("change", e => {
        loadFlag(e.target);
    })
};