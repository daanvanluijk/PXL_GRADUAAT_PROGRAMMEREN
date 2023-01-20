window.onload = function () {
    console.log(localStorage);
    console.log(localStorage.getItem("userId") + " " + localStorage.getItem("password"));
    postProfielRequestGet();
    let shoppingImage = document.getElementById('shopping_cart_image');
    shoppingImage.src = '../../Images/basket.png';

    document.getElementById('totalPrice').innerText = "€" + localStorage.getItem('totalPrice')
}

function postProfielRequest() {
    let Voornaam = document.getElementById('voornaam').value;
    let Achternaam = document.getElementById('achternaam').value;
    let Email = document.getElementById('email').value;
    let Country = document.getElementById('land').value;
    let Adres = document.getElementById('straat').value;
    let Postcode = document.getElementById('postcode').value;
    let Plaats = document.getElementById('gemeente').value;
    let Gsm = document.getElementById('tel').value;

    let newPerson = {
        studentId: localStorage.getItem("userId"),
        lastName: Achternaam,
        firstName: Voornaam,
        email: Email,
        country: Country,
        adress: Adres,
        phone: Gsm,
        zipcode: Postcode,
        placeName: Plaats
    };

    postProfielRequestGet();
}

function postProfielRequestGet() {
    let user = {
        userId: localStorage.getItem("userId"),
        password: localStorage.getItem("password")
    };
    postRequest('Profile/Get', user, 'handleProfielFetchRequest');
}

function handleProfielFetchRequest(result) {
    console.log("Fetch works");
    jsonResult = JSON.parse(result);
    console.log(jsonResult);
    console.log(jsonResult.firstName);

    document.getElementById('voornaam').textContent = jsonResult.firstName;
    document.getElementById('achternaam').textContent = jsonResult.lastName;
    document.getElementById('email').textContent = jsonResult.email;
    document.getElementById('land').textContent = jsonResult.country;
    document.getElementById('straat').textContent = jsonResult.adress;
    document.getElementById('postcode').textContent = jsonResult.zipcode;
    document.getElementById('gemeente').textContent = jsonResult.placeName;
    document.getElementById('tel').textContent = jsonResult.phone;
}

document.getElementById("btnadres").onclick = function () {
    location.href = "Profiel/Profiel.html";
};

document.getElementById("btnbetalen").onclick = function () {
    document.getElementById("betalingVoltooid").innerText = "Betaling voltooid";
};

let ckbID = document.getElementById('cbGegevensOpslaan');

ckbID.addEventListener('change', () => {
    localStorage.setItem('precheck1', ckbID.checked);

})

document.getElementById('naam').addEventListener('change', koekjes);
document.getElementById('kaartnum').addEventListener('change', koekjes);
document.getElementById('expmaand').addEventListener('change', koekjes);
document.getElementById('expjaar').addEventListener('change', koekjes);
document.getElementById('cvv').addEventListener('change', koekjes);
ckbID.addEventListener('change', koekjes);


function koekjes() {
    if (!ckbID.checked) {
        localStorage.setItem("name", "");
        localStorage.setItem("kaartnum", "");
        localStorage.setItem("month", "");
        localStorage.setItem("year", "");
        localStorage.setItem("cvv", "");
        return;
    }
    let naam = document.getElementById('naam').value;
    let kaartnum = document.getElementById('kaartnum').value;
    let vervalMaand = document.getElementById('expmaand').value;
    let vervalJaar = document.getElementById('expjaar').value;
    let cvv = document.getElementById('cvv').value;

    localStorage.setItem("name", naam);
    localStorage.setItem("kaartnum", kaartnum);
    localStorage.setItem("month", vervalMaand);
    localStorage.setItem("year", vervalJaar);
    localStorage.setItem("cvv", cvv);
}

document.getElementById('naam').value = localStorage.getItem("name");
document.getElementById('kaartnum').value = localStorage.getItem("kaartnum");
document.getElementById('expmaand').value = localStorage.getItem("month");
document.getElementById('expjaar').value = localStorage.getItem("year");
document.getElementById('cvv').value = localStorage.getItem("cvv");
console.log(localStorage.getItem("precheck1"));
document.getElementById('cbGegevensOpslaan').checked = (localStorage.getItem("precheck1") === "true");

document.getElementById('btnbetalen').addEventListener('click', function () {

    postRequest("Betaal", {userId: localStorage.getItem('userId')})

    Email.send({
        SecureToken: "bef2da7d-f86f-47a1-891f-f0a905692c65",
        To: jsonResult.email,
        From: "boekershotels@outlook.com",
        Subject: "Order Bevestigen",
        Body: `Beste ${jsonResult.firstName} ${jsonResult.lastName}. <br /> <br /> 
        U reservatie is compleet!<br />
        Wij kijken uit naar u verblijf. <br /><br />
        Als u deze reservatie wilt annuleren, gelieve contact met ons op te nemen via boekekershotels@outlook.com <br />
        Annulaties moeten minstens 48u voor de checkin datum gebeuren.
        <br /> <br />
        Met vriendelijke groeten <br />
        Boekers.com`,
    })

    let currentdate = new Date();
    let datetime = currentdate.getDate() + "/"
        + (currentdate.getMonth() + 1) + "/"
        + currentdate.getFullYear() + " "
        + currentdate.getHours() + ":"
        + currentdate.getMinutes() + ":"
        + currentdate.getSeconds();

    localStorage.setItem('paymentdate', datetime);

    console.log(localStorage.getItem('paymentdate'));

    localStorage.setItem('shoppingCart', '0');
    document.getElementById('number_of_rooms').innerHTML = localStorage.getItem('shoppingCart');
})



