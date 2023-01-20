let adres = document.getElementById('email');
let email;
let firstName;
let lastName;
postPaswoordVeranderenRequestGet();
function sendEmail() {
    if (document.getElementById('paswoord').value !== document.getElementById('paswoord2').value){
        document.getElementById('message').innerText = 'Wachtwoorden komen niet overeen !';
        return
    }
    if (document.getElementById('paswoord').value.length < 8 || document.getElementById('paswoord2').value.length < 8){
        document.getElementById('message').innerText = 'Wachtwoord moet minstens 8 karakters lang zijn';
        return
    }
    localStorage.setItem('passwordTemp', document.getElementById('paswoord').value);
    let code = "";
    for (let i = 0; i < 4; i++) {
        code += Math.floor(Math.random() * 10);
    }
    localStorage.setItem('code', code);
    console.log(localStorage.getItem('passwordTemp') + ' ' +  localStorage.getItem('code'));
    Email.send({
        SecureToken: "bef2da7d-f86f-47a1-891f-f0a905692c65",
        To: email,
        From: "boekershotels@outlook.com",
        Subject: "Wachtwoord veranderen",
        Body: `Beste ${firstName} ${lastName}. <br/> Dit is je geheime code: ${code} <br/> Geef deze in op de website om je nieuw wachtwoord te bevestigen.`,
    })
        .then(
            result => location.href='Paswoord%20bevestigen.html'
        )
}

function postPaswoordVeranderenRequestGet() {
    let user = {
        userId: localStorage.getItem('userId'),
        password: localStorage.getItem('password')
    };
    postRequest('PaswoordVeranderen/Get', user,'handlePaswoordVeranderenFetchRequest');
}

function handlePaswoordVeranderenFetchRequest(result) {
    console.log(result);
    try{
        jsonResult = JSON.parse(result);
        email = jsonResult.email;
        lastName = jsonResult.lastName;
        firstName = jsonResult.firstName;
        adres.value = email;
    }
    catch (e){

    }
}