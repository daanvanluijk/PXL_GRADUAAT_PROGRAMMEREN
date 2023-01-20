function confirmPassword() {
    if (document.getElementById('code').value !== localStorage.getItem('code')) {
        document.getElementById('message').innerHTML = 'Code is niet juist!';
        return
    }
    postPaswoordBevestigenRequestGet();

}

function postPaswoordBevestigenRequestGet() {
    let user = {
        userId: localStorage.getItem('userId'),
        password: localStorage.getItem('password')
    };
    postRequest('PaswoordBevestigen/Get', user,'handlePaswoordBevestigenFetchRequest');

}

function handlePaswoordBevestigenFetchRequest(result) {
    console.log(result);
    try {
        jsonResult = JSON.parse(result);
        console.log(jsonResult);
        let user = jsonResult;
        user.password = localStorage.getItem('passwordTemp');
        postPaswoordBevestigenRequest(user);
    } catch (e) {

    }
}

function postPaswoordBevestigenRequest(user){
    postRequest('PaswoordBevestigen/' + localStorage.getItem('userId'), user,'handlePaswoordUpdate');
}

function handlePaswoordUpdate(result){
    console.log(result);
    try {
        jsonResult = JSON.parse(result);
        localStorage.removeItem('userId');
        localStorage.removeItem('password');
        location.href='../Index.html';

    }
    catch (e){

    }
}