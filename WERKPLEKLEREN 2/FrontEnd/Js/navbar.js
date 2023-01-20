let currentPage = location.pathname;
console.log(currentPage);
let currentID = localStorage.getItem('userId');
console.log(currentID + " " + localStorage.getItem("password"));

let Navbar = '<div class="desktop_navbar">\n';
if (currentPage.endsWith("Index.html")) {
    Navbar +=
        '    <img class="img_logo" src="../Images/Boekers.png" alt="Logo boekers">\n';
} else {
    Navbar +=
        '    <img class="img_logo" src="../../Images/Boekers.png" alt="Logo boekers">\n';
}
if(currentPage.endsWith("Profiel.html") || currentPage.endsWith("Boekingen.html")){
    Navbar +=
        '    <ul class="list_items">\n' +
        '        <li><a class="list_item" href="../../Index.html">Home</a></li>\n' +
        '        <li><a class="list_item" href="../Boeken.html">Boeken</a></li>\n';
}
else if(currentPage.endsWith("BudgetKamer.html") || currentPage.endsWith("StandaardKamer.html") || currentPage.endsWith("LuxeKamer.html")){
    Navbar +=
        '    <ul class="list_items">\n' +
        '        <li><a class="list_item" href="../../Index.html">Home</a></li>\n' +
        '        <li><a class="list_item" href="../Boeken.html">Boeken</a></li>\n';
}
else if(currentPage.endsWith("Boeken.html") || currentPage.endsWith("Checkout.html") || currentPage.endsWith("Details%20kamers.html") || currentPage.endsWith("Bestelling%20overzicht.html")){
    Navbar +=
        '    <ul class="list_items">\n' +
        '        <li><a class="list_item" href="../Index.html">Home</a></li>\n' +
        '        <li><a class="list_item" href="./Boeken.html">Boeken</a></li>\n';
}
else{
    Navbar +=
        '    <ul class="list_items">\n' +
        '        <li><a class="list_item" href="#">Home</a></li>\n' +
        '        <li><a class="list_item" href="#carousel-img">Galerij</a></li>\n' +
        '        <li><a class="list_item" href="#KamersID">Kamers</a></li>\n' +
        '        <li><a class="list_item" href="#facilities">Faciliteiten</a></li>\n' +
        '        <li><a class="list_item" href="#hotelinfo">Info</a></li>\n' +
        '        <li><a class="list_item" href="./Html/Boeken.html">Boeken</a></li>\n';
}

if (currentID != null && currentPage.endsWith("Index.html")) {
    Navbar +=
        '    <li><a class="list_item" href="./html/Profiel/Profiel.html">Profiel</a></li>\n';
}
else if(currentID != null && currentPage.endsWith("BudgetKamer.html") || currentPage.endsWith("StandaardKamer.html") || currentPage.endsWith("LuxeKamer.html")){
    Navbar +=
        '    <li><a class="list_item" href="../Profiel/Profiel.html">Profiel</a></li>\n';
}
else if (currentID != null && currentPage.endsWith("Profiel.html") || currentPage.endsWith("Boekingen.html")) {
    Navbar +=
        '    <li><a class="list_item" href="#">Profiel</a></li>\n';
}
else if(currentID != null && currentPage.endsWith("Boeken.html") || currentPage.endsWith("Checkout.html") || currentPage.endsWith("Details%20kamers.html") || currentPage.endsWith("Bestelling%20overzicht.html")){
    Navbar +=
        '    <li><a class="list_item" href="./Profiel/Profiel.html">Profiel</a></li>\n';
}
Navbar +=
    '    </ul>\n';
if (currentID == null) {
    Navbar +=
        '    <a class="list_item_login">Login</a>\n';
}
else if (currentID != null && currentPage.endsWith("Index.html")){
    Navbar +=
        `<div class="log_out_plus_cart">
            <a class="list_item_login">Afmelden</a>
            <a href="Html/Bestelling%20overzicht.html" class="list_item_shopping_cart"><p id="number_of_rooms">${localStorage.getItem('shoppingCart')}</p><img id="shopping_cart_image" src="../Images/basket.png" alt="shopping-basket"></a>
        </div>`;
}
else if (currentID != null && currentPage.endsWith("Profiel.html") || currentPage.endsWith("Boekingen.html")){
    Navbar +=
        `<div class="log_out_plus_cart">
            <a class="list_item_login">Afmelden</a>
            <a href="../Bestelling%20overzicht.html" class="list_item_shopping_cart"><p id="number_of_rooms">${localStorage.getItem('shoppingCart')}</p><img id="shopping_cart_image" src="../../Images/basket.png" alt="shopping-basket"></a>
        </div>`;
}
else {
    Navbar +=
        `<div class="log_out_plus_cart">
            <a class="list_item_login">Afmelden</a>
            <a href="../Html/Bestelling%20overzicht.html" class="list_item_shopping_cart"><p id="number_of_rooms">${localStorage.getItem('shoppingCart')}</p><img id="shopping_cart_image" src="../Images/basket.png" alt="shopping-basket"></a>
        </div>`;
        //'    <a class="list_item_login">Afmelden</a>\n' +
        //'    <a class="list_item_shopping_cart"><p class="number_of_rooms">0</p><img src="../Images/basket.png" alt="shopping-basket"></a>\n';

};
Navbar +=
    '</div>\n' +
    '<div class="navBar_mobile">\n' +
    '   <div class="hamburger_box" onclick="Change(this)">\n' +
    '       <div class="bar1"></div>\n' +
    '       <div class="bar2"></div>\n' +
    '       <div class="bar3"></div>\n' +
    '   </div>\n';
if (currentPage.endsWith("Index.html")) {
    Navbar +=
        '    <img class="img_logo" src="../Images/Boekers.png" alt="Logo boekers">\n';
} else {
    Navbar +=
        '    <img class="img_logo" src="../../Images/Boekers.png" alt="Logo boekers">\n';
}
if (currentID == null) {
    Navbar +=
        '    <a class="list_item_login">Login</a>\n';
}
else if (currentID != null && currentPage.endsWith("Index.html")){
    Navbar +=
        `<div class="log_out_plus_cart">
            <a class="list_item_login">Afmelden</a>
            <a href="html/Bestelling%20overzicht.html" class="list_item_shopping_cart"><p id="number_of_rooms">${localStorage.getItem('shoppingCart')}</p><img id="shopping_cart_image" src="../../Images/basket.png" alt="shopping-basket"></a>
        </div>`;
}
else if (currentID != null && currentPage.endsWith("Profiel.html") || currentPage.endsWith("Boekingen.html")){
    Navbar +=
        `<div class="log_out_plus_cart">
            <a class="list_item_login">Afmelden</a>
            <a href="../Bestelling%20overzicht.html" class="list_item_shopping_cart"><p id="number_of_rooms">${localStorage.getItem('shoppingCart')}</p><img id="shopping_cart_image" src="../../Images/basket.png" alt="shopping-basket"></a>
        </div>`;
}
else {
    Navbar +=
        `<div class="log_out_plus_cart">
            <a class="list_item_login">Afmelden</a>
            <a href="../Html/Bestelling%20overzicht.html" class="list_item_shopping_cart"><p id="number_of_rooms">${localStorage.getItem('shoppingCart')}</p><img id="shopping_cart_image" src="../Images/basket.png" alt="shopping-basket"></a>
        </div>`;
}

if(currentPage.endsWith("Profiel.html") || currentPage.endsWith("Boekingen.html")){
    Navbar +=
        '</div>\n' +
        '<ul class="list_items_mobile">\n' +
        '    <li><a class="list_item" href="../../Index.html">Home</a></li>\n' +
        '    <li><a class="list_item" href="#">Account</a></li>\n' +
        '    <li><a class="list_item" href="../Paswoord%20veranderen.html">Paswoord</a></li>\n' +
        '    <li><a class="list_item" href="./Boekingen.html">Boekingen</a></li>\n' +
        '    <li><a class="list_item" href="../Boeken.html">Boeken</a></li>\n';
}
else if(currentPage.endsWith("BudgetKamer.html") || currentPage.endsWith("StandaardKamer.html") || currentPage.endsWith("LuxeKamer.html")){
    Navbar +=
        '    <ul class="list_items_mobile">\n' +
        '        <li><a class="list_item" href="../../Index.html">Home</a></li>\n' +
        '        <li><a class="list_item" href="../Boeken.html">Boeken</a></li>\n';
}
else if (currentPage.endsWith("Boeken.html") || currentPage.endsWith("Details%20kamers.html") || currentPage.endsWith("Checkout.html") || currentPage.endsWith("Bestelling%20overzicht.html")){
    Navbar +=
        '</div>\n' +
        '<ul class="list_items_mobile">\n' +
        '    <li><a class="list_item" href="../Index.html">Home</a></li>\n' +
        '    <li><a class="list_item" href="./Boeken.html">Boeken</a></li>\n';
}
else{
    Navbar +=
        '</div>\n' +
        '<ul class="list_items_mobile">\n' +
        '    <li><a class="list_item" href="#">Home</a></li>\n' +
        '    <li><a class="list_item" href="#carousel-img">Galerij</a></li>\n' +
        '    <li><a class="list_item" href="#KamersID">Kamers</a></li>\n' +
        '    <li><a class="list_item" href="#facilities">Faciliteiten</a></li>\n' +
        '    <li><a class="list_item" href="#hotelinfo">Info</a></li>\n' +
        '    <li><a class="list_item" href="../Html/Boeken.html">Boeken</a></li>\n';
}

if (currentID != null && currentPage.endsWith("Index.html")) {
    Navbar +=
        '    <li><a class="list_item" href="html/Profiel/Profiel.html">Profiel</a></li>\n';
}
else if(currentID != null && currentPage.endsWith("BudgetKamer.html") || currentPage.endsWith("StandaardKamer.html") || currentPage.endsWith("LuxeKamer.html")){
    Navbar +=
        '    <li><a class="list_item" href="../Profiel/Profiel.html">Profiel</a></li>\n';
}
else if (currentID != null && currentPage.endsWith("Boeken.html") || currentPage.endsWith("Details%20kamers.html") || currentPage.endsWith("Checkout.html") || currentPage.endsWith("Bestelling%20overzicht.html")){
    Navbar +=
        '    <li><a class="list_item" href="./Profiel/Profiel.html">Profiel</a></li>\n';
}
Navbar +=
    '</ul>\n' +
    '\n' +
    '<div class="mobile_login_form">\n' +
    '    <label for="email_input">E-mail</label>\n' +
    '    <div class="row_images">\n';
if (currentPage.endsWith("Index.html")) {
    Navbar +=
        '        <img src="../Images/user-solid.svg" alt="">\n';
} else {
    Navbar +=
        '        <img src="../../Images/user-solid.svg" alt="">\n';
}
Navbar +=
    '        <input  id="email_input" type="email">\'        </div>\n' +
    '    <label for="pass_input">Paswoord</label>\n' +
    '    <div class="row_images">\n';
if (currentPage.endsWith("Index.html")) {
    Navbar +=
        '        <img src="../Images/lock-solid.svg" alt="">\n';
} else {
    Navbar +=
        '        <img src="../../Images/lock-solid.svg" alt="">\n';
}
Navbar +=
    '        <input id="pass_input"  type="password">\n' +
    '    </div>\n' +
    '    <p id="notfound"></p>' +
    '    <div class="login_links">\n' +
    '        <a href="#">Paswoord vergeten</a>\n' +
    '        <a href="Html/Registratie.html">Register</a>\n' +
    '    </div>\n' +
    '    <button id="open_loginform" >Login</button>\n' +
    '</div>';

document.getElementById("nav").innerHTML = Navbar;


const btnOpenLoginForms = document.getElementsByClassName("list_item_login");
const loginForm = document.querySelector(".mobile_login_form");
console.log(btnOpenLoginForms);
CloseLoginForm();// sluit login form

// styling hamburer menu
function Change (x)
{
    x.classList.toggle("change");
}
// open and close Hamburger menu
const btnHamburtMenu = document.querySelector(".hamburger_box");
const listItems = document.querySelector(".list_items_mobile");
CloseHamburgerMenu();

btnHamburtMenu.addEventListener("click",() =>
{
    if (listItems.style.display === "none")
    {
        OpenHamburgerMenu();
    }
    else
    {
        CloseHamburgerMenu();
    }
})
function OpenHamburgerMenu ()
{
    listItems.style.display = "flex";
}
function CloseHamburgerMenu ()
{
    listItems.style.display = "none";
}
// Login form function
for (let i = 0; i < btnOpenLoginForms.length; i++) {
    btnOpenLoginForms.item(i).addEventListener("click", () =>
    {
        if (currentID == null) {
            if (loginForm.style.display === "none")
            {
                OpenLoginForm();
            }
            else
            {
                CloseLoginForm();
            }
            console.log("LOGIN CLICK");
        } else {
            localStorage.removeItem("userId");
            localStorage.removeItem("password");
            if (location.pathname.endsWith("Profiel.html")) {
                location.href = "../../Index.html";
            } else {
                location.reload();
            }
        }
    })
}

function OpenLoginForm ()
{
    loginForm.style.display = "flex";
}
function CloseLoginForm ()
{
    loginForm.style.display = "none";
}

let newOrderNavbar = {
    userId: parseInt(localStorage.getItem("userId"))
}

postRequest('Order/NotPaidCount',newOrderNavbar,'updateShoppingCart');

function updateShoppingCart(value){
    let jsonCart = JSON.parse(value);
    setTimeout(setShoppingCart, 100, jsonCart)
}

function setShoppingCart(jsonCart){
    console.log(jsonCart);
    let nodelist = document.getElementsByClassName('list_item_shopping_cart');
    for (let i = 0; i < nodelist.length; i++) {
        console.log(nodelist[i].childNodes);
        for (let j = 0; j < nodelist[i].childNodes.length; j++) {
            nodelist[i].childNodes[j].innerHTML = jsonCart;
        }
    }
}

