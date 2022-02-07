const dropList = document.querySelectorAll(".drop-list select");

    //fromCurrency = document.querySelector(".from select"),
    //toCurrency = document.querySelector(".to select"),
    //getButton = document.querySelector("form button");



/*Loop through the list of countries to display the countries the CONSOLE*/
for (let i = 0; i < dropList.length; i++) {
    for (let currency_code in country_list) {
        console.log(currency_code)
    }
}
