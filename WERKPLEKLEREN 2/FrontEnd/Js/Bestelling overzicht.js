let counter = 0;
let totalPrice = 0;
let email;
let firstName;
let lastName;
let bool1 = false;
let rooms = [];
let newOrder = {
    userId: parseInt(localStorage.getItem("userId"))
}
postPaswoordVeranderenRequestGet();
//localStorage.setItem('shoppingCart','0');
//document.getElementById('number_of_rooms').innerHTML = localStorage.getItem('shoppingCart');

postRequest('Order/NotPaid',newOrder,'renderRooms');
postRequest('Order/NotPaidCount',newOrder,'updateShoppingCart');

function updateShoppingCart(value){
    document.getElementById('number_of_rooms').innerHTML = JSON.parse(value).amount;
}

function renderRooms(value){
    rooms = JSON.parse(value);
    counter = 0;
    totalPrice = 0;
    console.log(rooms);
    for (let i = 0; i < rooms.length; i++) {
        console.log(rooms[i]);
        let priceAdults = (rooms[i].adultAmount - 1) * (rooms[i].roomPrice * 0.1);
        console.log(priceAdults);
        let priceKids = rooms[i].childrenAmount * (rooms[i].roomPrice * 0.05);
        console.log(priceKids);
        let price = Math.round((getNumberOfDays(rooms[i].checkinDate,rooms[i].checkoutDate) * rooms[i].roomPrice)+ priceAdults + priceKids);
        totalPrice += price;

        let division = document.createElement('div');
        division.id = 'information';
        division.innerHTML = `<h2>Je kamergegevens</h2>
            <div class="columns">
                <div class="checkin">
                    <p>Inchecken</p>
                    <b id="checkinDate">${rooms[i].checkinDate.slice(0,10)}</b>
                    <p>Vanaf 13:00u</p>
                </div>
                <div class="checkOut">
                    <p>Uitchecken</p>
                    <b id="checkoutDate">${rooms[i].checkoutDate.slice(0,10)}</b>
                    <p>Tot 11:00u</p>
                </div>
            </div>
            <div class="info">
                <p>Totale verblijfsduur:</p>
                <b id="nights">${getNumberOfDays(rooms[i].checkinDate,rooms[i].checkoutDate)} nachten</b>
            </div>
            <div class="info">
                <p>Aantal gasten:</p>
                <b id="guests">${rooms[i].adultAmount + rooms[i].childrenAmount}</b>
            </div>
            <div class="info">
                <p>Je hebt geselecteerd:</p>
                <b id="room">1 x ${rooms[i].roomTitle}</b>
            </div>
            <div class="info">
                 <p>Prijs:</p> 
                 <b>€${price}</b>   
                 <p>Toeslag volwassenen:</p>
                 <b>€${Math.round(priceAdults)}</b>   
                 <p>Toeslag kinderen:</p>   
                 <b>€${Math.round(priceKids)}</b>                
            </div>
            <div class="closeButton"><img src="../Images/close.png" alt="close" id="${rooms[i].orderID}" class="xButton" onclick="getImageId(this)"><label>Verwijder kamer</label></div>`;
        document.getElementsByClassName('data')[0].appendChild(division);
        let line = document.createElement('hr');
        line.class = 'line';
        document.getElementsByClassName('data')[0].appendChild(line);
        document.getElementById(rooms[i].orderID).addEventListener('click', sendMail);
        counter++;
    }
    let btw = Math.round(totalPrice * 0.06);
    let priceWoBtw = totalPrice - btw;
    let division2 = document.createElement('div');
    division2.innerHTML = `<h2>Je prijsoverzicht</h2>
        <div class="columns">
            <div>
                <p>${counter} Kamer(s)</p>
                <p>6% BTW</p>
                <b>Prijs</b>
            </div>
            <div>
                <p id="priceWoBtw">€${priceWoBtw}</p>
                <p id="btw">€${btw}</p>
                <b id="totalPrice">€${totalPrice}</b>
            </div>
        </div>`;
    document.getElementsByClassName('data')[0].appendChild(division2);

    localStorage.setItem('totalPrice',totalPrice.toString());

}

document.getElementById('button').addEventListener("click", function (){
    location.href="./Checkout.html";
});

function getImageId(button){
    localStorage.setItem('orderId',button.id);
    console.log(localStorage.getItem('orderId'));
}

function postRequestDeleteOrder(){
    postRequest('DeleteOrder',composeDeleteOrder(),'onOrderDeleted');
    /*let room = parseInt(localStorage.getItem('shoppingCart')) - 1;
    localStorage.setItem('shoppingCart',room.toString());
    document.getElementById('number_of_rooms').innerHTML = localStorage.getItem('shoppingCart');*/

}

function composeDeleteOrder(){
    let OrderID = parseInt(localStorage.getItem('orderId'));
    let newDeleteOrder = {
        orderID: OrderID
    }
    return newDeleteOrder;
}


function postPaswoordVeranderenRequestGet() {
    let user = {
        userId: localStorage.getItem('userId'),
        password: localStorage.getItem('password')
    };
    postRequest('PaswoordVeranderen/Get', user,'handlePaswoordVeranderenFetchRequest');
}

function handlePaswoordVeranderenFetchRequest(result) {
    try{
        console.log()
        jsonResult = JSON.parse(result);
        email = jsonResult.email;
        lastName = jsonResult.lastName;
        firstName = jsonResult.firstName;

    }
    catch (e){
    }
}


function sendMail(event){
    Email.send({
        SecureToken: "bef2da7d-f86f-47a1-891f-f0a905692c65",
        To: email,
        From: "boekershotels@outlook.com",
        Subject: "Item verwijderd",
        Body: `Beste ${firstName} ${lastName}. <br/> U heeft zojuist een item verwijderd uit uw winkelkar!`
    });

    postRequestDeleteOrder();


    /*let roomToRemove = rooms.find(room => {
        return room.orderID === event.target.id;
    })
    let indexObject = rooms.indexOf(roomToRemove);
    rooms.splice(indexObject,1);

    document.getElementsByClassName('data')[0].innerHTML = "";
    renderRooms(rooms.toString());*/
}

function onOrderDeleted(result){
    document.getElementsByClassName('data')[0].innerHTML = "";
    postRequest('Order/NotPaidCount',newOrder,'updateShoppingCart');
    postRequest('Order/NotPaid',newOrder,'renderRooms');
}