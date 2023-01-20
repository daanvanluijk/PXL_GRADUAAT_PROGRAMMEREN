let Navbar = '<div class="desktop_navbar">\n' +
    '    <img class="img_logo" src="../Images/Boekers.png" alt="Logo boekers">\n' +
    '    <ul class="list_items">\n' +
    '        <li><a class="list_item" href="#">Home</a></li>\n' +
    '        <li><a class="list_item" href="">Galerij</a></li>\n' +
    '        <li><a class="list_item" href="">Kamers</a></li>\n' +
    '        <li><a class="list_item" href="">Faciliteiten</a></li>\n' +
    '        <li><a class="list_item" href="">Info</a></li>\n' +
    '        <li><a class="list_item" href="../Html/Boeken.html">Boeken</a></li>\n';
if (document.cookie != "null") {
    Navbar +=
        '    <li><a class="list_item" href="../Html/Profiel/Profiel.html">Profiel</a></li>\n';
};
Navbar +=
    '    </ul>\n';
if (document.cookie == "null") {
    Navbar +=
        '    <a class="list_item_login">Login</a>\n';
} else {
    Navbar +=
        '    <a class="list_item_login">Afmelden</a>\n';
};
Navbar +=
    '</div>\n' +
    '<div class="navBar_mobile">\n' +
    '   <div class="hamburger_box" onclick="Change(this)">\n' +
    '       <div class="bar1"></div>\n' +
    '       <div class="bar2"></div>\n' +
    '       <div class="bar3"></div>\n' +
    '   </div>\n' +
    '    <img class="img_logo" src="../Images/Boekers.png" alt="Logo boekers">\n';
if (document.cookie == "null") {
    Navbar +=
        '    <a class="list_item_login">Login</a>\n';
} else {
    Navbar +=
        '    <a class="list_item_login">Afmelden</a>\n';
};
Navbar +=
    '</div>\n' +
    '<ul class="list_items_mobile">\n' +
    '    <li><a class="list_item" href="#">Home</a></li>\n' +
    '    <li><a class="list_item" href="">Galerij</a></li>\n' +
    '    <li><a class="list_item" href="">Kamers</a></li>\n' +
    '    <li><a class="list_item" href="">Faciliteiten</a></li>\n' +
    '    <li><a class="list_item" href="">Info</a></li>\n' +
    '    <li><a class="list_item" href="../Html/Boeken.html">Boeken</a></li>\n';
if (document.cookie != "null") {
    Navbar +=
        '    <li><a class="list_item" href="../Html/Profiel/Profiel.html">Profiel</a></li>\n';
};
Navbar +=
    '</ul>\n' +
    '\n' +
    '<div class="mobile_login_form">\n' +
    '    <label for="email_input">E-mail</label>\n' +
    '    <div class="row_images">\n' +
    '        <img src="../Images/user-solid.svg" alt="">\n' +
    '        <input  id="email_input" type="email">\'        </div>\n' +
    '    <label for="pass_input">Paswoord</label>\n' +
    '    <div class="row_images">\n' +
    '        <img src="../Images/lock-solid.svg" alt="">\n' +
    '        <input id="pass_input"  type="password">\n' +
    '    </div>\n' +
    '    <div class="login_links">\n' +
    '        <a href="#">Paswoord vergeten</a>\n' +
    '        <a href="../Html/Registratie.html">Register</a>\n' +
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
        if (document.cookie == "null") {
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
            document.cookie = "null";
            location.reload();
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