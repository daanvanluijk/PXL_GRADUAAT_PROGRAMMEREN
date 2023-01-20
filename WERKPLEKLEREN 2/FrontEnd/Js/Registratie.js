let boolcheck = false;

function postRegistratieRequest() {
    postRequest('Registration', composeUser(), 'handlePostRegistratieResponse');
}

function handlePostRegistratieResponse(result) {
    try {
        const jsonResult = JSON.parse(result);
        console.log(jsonResult);
        console.log(jsonResult.userID);
        localStorage.setItem('userId', jsonResult.userID)
        localStorage.setItem('password', jsonResult.Password);
        location.href = '../Index.html';

    } catch (e) {
        alert(result);
    }


}

function composeUser() {
    let Paswoord = document.getElementById('wachtwoord').value;
    let Voornaam = document.getElementById('voornaam').value;
    let Achternaam = document.getElementById('lastname').value;
    let Email = document.getElementById('email').value;
    let Adres = document.getElementById('straat').value;
    let Postcode = document.getElementById('postcode').value;
    let Plaats = document.getElementById('gemeente').value;
    let Gsm = document.getElementById('tel').value;
    let Land = document.getElementById('country').value;

    let newPerson = {
        studentId: 0,
        password: Paswoord,
        lastName: Achternaam,
        firstName: Voornaam,
        email: Email,
        adress: Adres,
        phone: Gsm,
        zipcode: Postcode,
        placeName: Plaats,
        country: Land
    };

    return newPerson;
}

let saveElement = document.getElementById('save');
saveElement.addEventListener("click", () => {
    if (boolcheck === true) {
        postRegistratieRequest();
    } else {
        document.getElementById("wrong_number").innerHTML = "Geen geldig nummer";
        return;
    }
})


const wwinput = document.querySelector("#wachtwoord");
wwinput.addEventListener("focusout", () => {
    verifyPassword()
});

const wwinput2 = document.querySelector("#wachtwoord2");
wwinput2.addEventListener("focusout", () => {
    validatePassword()
});

function validatePassword() {
    console.log(wwinput.value, wwinput2.value);
    if (wwinput.value !== wwinput2.value) {
        document.getElementById("message2").innerHTML = "Wachtwoorden komen niet overeen. Probeer het opnieuw.";
        return false;
    } else {
        document.getElementById("message2").style.display = "none";
        boolcheck = true;
    }
}

function verifyPassword() {
    var pw = document.getElementById("wachtwoord").value;
    //check empty password field
    if (pw == "") {
        document.getElementById("message").innerHTML = "Het paswoord mag niet leeg zijn!";
        return false;
    }
//minimum password length validation
    if (pw.length < 8) {
        document.getElementById("message").innerHTML = "Wachtwoord moet minstens 8 karakters bevatten";
        return false;
    }

//maximum length of password validation
    if (pw.length > 15) {
        document.getElementById("message").innerHTML = "Wachtwoord mag niet meer dan 15 karakters bevatten";
        return false;
    } else {
        document.getElementById("message").style.display = "none";
    }
}