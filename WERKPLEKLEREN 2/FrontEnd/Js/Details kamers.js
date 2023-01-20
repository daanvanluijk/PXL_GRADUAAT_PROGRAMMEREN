localStorage.removeItem('roomPrice');
let newOrderUser = {
    userId: parseInt(localStorage.getItem("userId"))
}

//Kalender vullen met datum van vandaag
let todayDate = new Date().toISOString().slice(0, 10);
let d1 = document.getElementById("myDate1");
let d2 = document.getElementById("myDate2");
d1.min = todayDate;
d1.value = todayDate;
d2.min = todayDate;
d2.value = todayDate;


let datesWanted = [];
let datesBooked = [];

fetchJSON('ProductCard','fillRoom');
fetchJSON('RoomImages','fillThumbnails');
fetchJSON('Icons','fillIcons');
fetchJSON('Order','checkIfBooked');

let price;
let maxCap = parseInt(localStorage.getItem('maxCapacity'));
let numberOfAdults = 1;
let numberOfKids = 0;
let currentCap;
let adults;
let kids = 0;

document.getElementById('knop').addEventListener('click', callArrayFunction);
document.getElementById('knop').addEventListener('click',callCompareFunction);
document.getElementById('myDate1').addEventListener('focusout', disableDates);
document.getElementById('adults').addEventListener('change', function (){
    numberOfAdults = parseInt(this.value) + 1;
});

document.getElementById('kids').addEventListener('change', function (){
    numberOfKids = parseInt(this.value);
});
document.getElementById('buttonBook').addEventListener('click',saveRoomInfo);

function notAvailable(result){
    try{
        JSON.parse(result);
    }
    catch(e){
        document.getElementById('prijs').innerHTML = result;
    }
    postRequest('Order/NotPaidCount',newOrderUser,'updateShoppingCartDetails');
}

//Zorgt dat je uitcheckdatum nooit vroeger als je incheckdatum kan zijn
function disableDates() {
    d2.min = d1.value;
    d2.value = d1.value;
}

function callArrayFunction(){
    return addDatesToArray(datesWanted,d1.value,d2.value);
}

function callCompareFunction(){
    return compareDates(datesWanted,datesBooked);
}


//Berekent het verschil in dagen tussen in- en uitcheckdatum
function getNumberOfDays(dateCheckIn,dateCheckOut) {
    let date1 = new Date(dateCheckIn);
    let date2 = new Date(dateCheckOut);
    let difference = date2.getTime() - date1.getTime();
    let days = difference / (1000 * 60 * 60 * 24);
    return days;
}

//Vult een array met alle data tussen in- en uitcheckdatum
function addDatesToArray(array,dateIn,dateOut) {
    let randomArray = [];
    array.length = 0;
    let date = new Date(dateIn);

    for (let i = 0; i < getNumberOfDays(dateIn,dateOut) + 1; i++) {
        let datum1ISO = date.toISOString();
        let datumSlice = datum1ISO.slice(0, 10);
        array.push(datumSlice);
        let nextDay = new Date(date.getTime() + 24 * 60 * 60 * 1000);
        date = nextDay;
    }
    console.log(array);
    array = randomArray;
}

//Kijkt of de data die geselecteerd zijn beschikbaar zijn
function compareDates(array1,array2) {
    let booked = false;
    for (let i = 0; i < array1.length; i++) {
        for (let j = 0; j < array2.length; j++) {
            if (array1[i] === array2[j]){
                document.getElementById('prijs').innerHTML = "Data zijn niet beschikbaar";
                booked = true;
                return;
            }

        }

    }
    currentCap = numberOfAdults + numberOfKids;
    if (currentCap > maxCap){
        document.getElementById('prijs').innerHTML = `Maximum aantal personen mag niet meer als ${maxCap} zijn`;
        console.log(currentCap);
        return;
    }
    if (!booked) {
        calculatePrice();
    }
}



//Berekent de prijs van de kamer aan de hand van het aantal dagen en aantal personen
function calculatePrice() {
    let pricePerNight = parseInt(localStorage.getItem('roomPrice'));
    adults = parseFloat(document.getElementById('adults').value) / 10 * pricePerNight;
    if (parseFloat(document.getElementById('kids').value) > 0) {
        kids = parseFloat(document.getElementById('kids').value) / 20 * pricePerNight;
    }
    price = Math.round((getNumberOfDays(d1.value,d2.value) * pricePerNight) + adults + kids);
    document.getElementById('prijs').innerHTML = `€${price}`;
    kids = 0;
}

//Zorgt dat de grote afbeelding vervangen wordt door de thumbnails
function expandImage(imgs) {
    let expandImg = document.getElementById("expandedImg");
    expandImg.src = imgs.src;
}



//Vult de pagina met info kamer
function fillRoom(value){
    for (let i = 0; i < value.length; i++){
        if (parseInt(localStorage.getItem('roomId')) === value[i].roomId){
            localStorage.setItem('roomPrice',value[i].roomPrice.toString());
            localStorage.setItem('maxCapacity',value[i].roomCapacity.toString());
            document.getElementById('prijs').innerHTML = `€${value[i].roomPrice}/Nacht`;
            document.getElementsByClassName('roomTitle')[0].innerHTML = value[i].roomTitle;
            document.getElementsByClassName('infoKamer')[0].innerHTML = value[i].roomDescription;
            document.getElementById('expandedImg').src = "data:image/png;base64," + value[i].imageUrl;
        }
    }
}


//Vult de thumbnails met images
function fillThumbnails(value){
    for (let i = 0; i < value.length; i++) {
        if (parseInt(localStorage.getItem('roomId')) === value[i].roomId){
            let division = document.createElement('div');
            division.className = 'column';
            division.innerHTML = `<img src="data:image/png;base64,${value[i].imageUrl}" alt="image kamer" class="thumbnail" onclick="expandImage(this);">`
            document.getElementsByClassName('row')[0].appendChild(division);
        }
    }
}

//Creeërt icons
function fillIcons(value)
{
    for (let i = 0; i < value.length; i++) {
        if(parseInt(localStorage.getItem('roomId')) === value[i].iconsId){
            let divisie = document.createElement('div');
            divisie.className = 'icon';
            divisie.innerHTML = `<img src="${value[i].iconsUrl}" alt="roomimg">`;
            document.getElementsByClassName('icons')[0].appendChild(divisie);
        }
    }
}

//Slaat alle info op in koekjes en redirect u naar de bestellingsoverzicht
function saveRoomInfo(){
    postRequestOrder();
}

function postRequestOrder(){
    postRequest('DetailsKamer',composeOrder(), 'notAvailable');
    document.getElementById('prijs').innerHTML = 'Kamer toegevoegd aan winkelmand';
}


function composeOrder(){
    let UserID = parseInt(localStorage.getItem('userId'));
    let CheckInDate = new Date(d1.value);
    let CheckOutDate = new Date(d2.value);
    let PaymentDate = todayDate;
    let AdultAmount = parseInt(document.getElementById('adults').value) + 1;
    let KidsAmount = parseInt(document.getElementById('kids').value);
    let RoomID = parseInt(localStorage.getItem('roomId'));

    let newOrder = {
        userID : UserID,
        checkinDate : CheckInDate,
        checkoutDate : CheckOutDate,
        paymentDate : PaymentDate,
        adultAmount : AdultAmount,
        childrenAmount : KidsAmount,
        roomID : RoomID
    }
    return newOrder;
}


function checkIfBooked(value){
    for (let i = 0; i < value.length; i++) {
        if (parseInt(localStorage.getItem('roomId')) === value[i].RoomID){
            dateOne = value[i].CheckinDate;
            dateTwo = value[i].CheckoutDate;
            addDatesToArray(datesBooked,dateOne,dateTwo);
        }
    }
}

function updateShoppingCartDetails(value){
    let jsonCart = JSON.parse(value);
    console.log(jsonCart);
    document.getElementById('number_of_rooms').innerHTML = jsonCart;
}