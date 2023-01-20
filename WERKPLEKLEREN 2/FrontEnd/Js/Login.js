//Login
if (document.querySelector("#open_loginform"))
{
    document.querySelector("#open_loginform").addEventListener("click", LoginPostRequest) ;
}

function LoginPostRequest ()
{
    let Email = document.getElementById('email_input').value;
    let Paswoord = document.getElementById('pass_input').value;

    let newLogin = {
        email: Email,
        password: Paswoord,
    };
    postRequest('Login', newLogin, 'LoginHandlePostRequestResponse');
}
function LoginHandlePostRequestResponse(result)
{
    console.log(result);
    try {
        let jsonResult = JSON.parse(result);
        localStorage.setItem("userId", JSON.parse(result).userId);
        localStorage.setItem("password", JSON.parse(result).password);
        console.log(localStorage.getItem("userId") + " " + localStorage.getItem("password"));
        location.reload();
    }
    catch (e) {
        document.querySelector("#notfound").innerText = result;
    }
}

