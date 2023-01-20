window.onload = function (){
    console.log(localStorage.getItem("userId") + " " + localStorage.getItem("password"));
    postProfielRequestGet();
    let shoppingImage = document.getElementById('shopping_cart_image');
    shoppingImage.src = '../../Images/basket.png';
}

function postProfielRequest() {
    let Voornaam = document.getElementById('voornaam').value;
    let Achternaam = document.getElementById('achternaam').value;
    let Email = document.getElementById('email').value;
    let Country = document.getElementById('country').value;
    let Adres = document.getElementById('straat').value;
    let Postcode = document.getElementById('postcode').value;
    let Plaats = document.getElementById('gemeente').value;
    let Gsm = document.getElementById('tel').value;

    let newPerson = {studentId: localStorage.getItem("userId"),
        lastName: Achternaam,
        firstName: Voornaam,
        email: Email,
        country: Country,
        adress: Adres,
        phone: Gsm,
        zipcode: Postcode,
        placeName: Plaats};

    postRequest('Profile/' + localStorage.getItem("userId"), newPerson);
    postProfielRequestGet();
}

function postProfielRequestGet() {
    let user = {userId: localStorage.getItem("userId"),
                password: localStorage.getItem("password")};
    postRequest('Profile/Get', user, 'handleProfielFetchRequest');
}

function handleProfielFetchRequest(result) {
    console.log("TEST INCOMING");
    jsonResult = JSON.parse(result);
    console.log(jsonResult);

    document.getElementById('gebruiker').innerText = "Welkom " + jsonResult.firstName;
    document.getElementById('voornaam').value = jsonResult.firstName;
    document.getElementById('achternaam').value = jsonResult.lastName;
    document.getElementById('email').value = jsonResult.email;
    document.getElementById('country').value = jsonResult.country;
    document.getElementById('straat').value = jsonResult.adress;
    document.getElementById('postcode').value = jsonResult.zipcode;
    document.getElementById('gemeente').value = jsonResult.placeName;
    document.getElementById('tel').value = jsonResult.phone;
}

