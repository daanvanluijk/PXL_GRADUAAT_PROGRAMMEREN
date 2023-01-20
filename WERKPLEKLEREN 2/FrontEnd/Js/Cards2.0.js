let url = "https://localhost:5001/api/";
let iconClickte = false;
let roomdata = []; // Alle date van de Rooms uit de database
let iconsArray = []; // Alle icons per RoomId
let roomIdArray = []; // Filter voor dezelde RoomId  deze worden eruit gehaald
let filterArray = []; // Hier komt de uiteindelijke data in die gezien word
let loader = document.getElementById("preloader");

const createFinished = new Event("createFinished");
LoadingScreen("createFinished");

GetAllIcons("Icons");// Get al icons
ApiGetAllProductData("ProductCard");// Get product cards

function ApiGetAllProductData(controller) {
    fetch(url + controller)
        .then(value => value.json())
        .then(value => logResult(value))
        .then(result => cardData(result))

}

function GetAllIcons(controller) {
    fetch(url + controller)
        .then(value => value.json())
        .then(value => logResult(value))
        .then(result => IconData(result))
}

function logResult(value) {
    console.log(value);
    return value;
}


//We halen uit de DB hier alle icons*****************************
function IconData(value) {
    for (let i = 0; i < value.length; i++) {
        const iconsObj = {
            IconId: value[i].roomID,
            IconUrl: value[i].iconsUrl,
            BedType: value[i].bedTypeName
        }
        iconsArray.push(iconsObj);
    }

}

function cardData(value) {
    for (let i = 0; i < value.length; i++) {
        if (roomIdArray.includes(value[i].roomId)) {
            continue;
        }
        const obj = {
            RoomId: value[i].roomId,
            RoomTitle: value[i].roomTitle,
            ImageUrl: value[i].imageUrl,
            RoomPrice: value[i].roomPrice,
            RoomDiscription: value[i].roomDescription,
            RoomIconsUrl: value[i].iconUrl,
        }
        roomdata.push(obj);
        roomIdArray.push(value[i].roomId);
    }

    CreateCard();
}


//Het maken van de productCards ************************************************************
function CreateCard() {
    if (iconClickte === false) {
        for (let i = 0; i < roomdata.length; i++) {
            filterArray.push(roomdata[i]);
        }
    }
    let cardContainer = document.createElement("div");
    cardContainer.className = "cardsContainer";
    document.getElementById('contentbox').appendChild(cardContainer);
    for (let i = 0; i < filterArray.length; i++) {
        let productCard = document.createElement("div");
        productCard.className = "productcard";
        let cardImageContent = document.createElement("div");
        cardImageContent.className = "card-image-content";
        let image = document.createElement("img");
        image.className = "card-image";
        image.src = "data:image/png;base64," + filterArray[i].ImageUrl;
        let cardContent = document.createElement("div");
        cardContent.className = "card-content";
        cardContent.innerHTML = `<h2>${filterArray[i].RoomTitle}</h2>
                                 <p>${filterArray[i].RoomDiscription}</p>`;


        let facilIcons = document.createElement("div");
        facilIcons.className = "facil_icon";
        cardImageContent.appendChild(image);
        productCard.appendChild(cardImageContent);
        cardContent.appendChild(facilIcons);

        for (j = 0; j < iconsArray.length; j++) {
            if (iconsArray[j].IconId === filterArray[i].RoomId) {
                let iconImg = document.createElement("img");
                iconImg.className = "svg_icon";
                iconImg.src = iconsArray[j].IconUrl;

                facilIcons.appendChild(iconImg);

            }
        }


        let cardPriceContent = document.createElement("div");
        cardPriceContent.className = "card-price-content";
        cardPriceContent.innerHTML = `<h2 class="card-price">€${filterArray[i].RoomPrice}/Nacht</h2>
                                        <button id="${filterArray[i].RoomId}" onclick="storeRoomId(this)" class="card-shoppingcart">Boeken</button>`;


        productCard.appendChild(cardContent);
        productCard.appendChild(cardPriceContent);
        document.getElementsByClassName("cardsContainer")[0].appendChild(productCard);
    }
    iconClickte = false;
    dispatchEvent(createFinished);
}


// Leegmaken van de volledige checkbox*****************************************************
function uncheckAllFilterItems() {
    var uncheck = false;
    document.querySelectorAll('input[type=checkbox]').forEach(el => el.checked = uncheck);

}

//Slaat roomId op in een koekje zodra je op de button klikt en opent de Details kamers html file
//Het roomId wordt in de ID attribute van de button opgeslagen bij de creatie ervan*
function storeRoomId(button) {
    localStorage.setItem("roomId", button.id);
    console.log(localStorage.getItem('roomId'));
    location.href = 'Details kamers.html';
}


//document.getElementById("clearFilter").addEventListener("click", showAllCards);

function showAllCards() {
    uncheckAllFilterItems();
    filterArray = [];
    let e = document.getElementsByClassName("cardsContainer")[0];
    e.parentElement.removeChild(e);
    CreateCard();
}


//Icon data en room data samenVoegen Voor de filter te laten werken
let checkBoxes = document.querySelectorAll("input[type=checkbox]");
console.log(checkBoxes);
for(let i=0; i < checkBoxes.length; i++)
{
    console.log(checkBoxes[i]);
    checkBoxes[i].addEventListener("change", ()=>
    {
        passFilterArguments();
    });
}

function passFilterArguments() {
    let lookUp = {
        "1p_bed": "1 persoons",
        "2p_bed": "2 persoons",
        "King_Size": "King size",
        "Zetel_bed": "Zetel",
        "Airco": "Airco",
        "Wifi": "Wifi",
        "Douche": "Douche",
        "Bath": "Bad"
    }
    let filterArguments = [];
    let checkBoxes = document.querySelectorAll("input[type=checkbox]");
    console.log(checkBoxes);
    for (let i = 0; i < checkBoxes.length; i++) {
        console.log(checkBoxes[i]);
        if (checkBoxes[i].checked) {
            filterArguments.push(lookUp[checkBoxes[i].id])
        }
    }
    console.log(filterArguments);
    if (filterArguments.length !== 0)
    {
        filterData(filterArguments);
    }else{
        filterArray = [];
        let e = document.getElementsByClassName("cardsContainer")[0];
        e.parentElement.removeChild(e);
        CreateCard();
    }

}


function filterData(arguments = [])
{
    loader.style.display = "initial";
    iconClickte = true;
    filterArray = [];
    let e = document.getElementsByClassName("cardsContainer")[0];
    e.parentElement.removeChild(e);

    let roomBedTypes = {};
    for (let i = 0; i < iconsArray.length; i++) {

        if (arguments.includes(iconsArray[i].BedType)  ) {

            if (roomBedTypes[(iconsArray[i].IconId)] !== undefined) {
                if(!roomBedTypes[iconsArray[i].IconId].includes(iconsArray[i].BedType))
                {
                    roomBedTypes[iconsArray[i].IconId].push(iconsArray[i].BedType);
                }

            } else {
                roomBedTypes[iconsArray[i].IconId] = [iconsArray[i].BedType];
            }
        }
    }
    for (let key in roomBedTypes) {
        if (roomBedTypes[key].length >= arguments.length) {
            for (let j = 0; j < roomdata.length; j++) {

                if (roomdata[j].RoomId == key) {
                    filterArray.push(roomdata[j])
                }
            }
        }
    }

    CreateCard();
}


// Laten zien en hiden van de filter tabel
let  filterTabel = document.querySelector(".filterbox");
document.querySelector(".btnTest").addEventListener("click",() =>
{
    if (filterTabel.style.display === "none")
    {
        openfilterBar();
    }
    else
    {
        closeFilterBar();
    }

})
function openfilterBar()
{
    filterTabel.style.display = "flex";
}
function closeFilterBar()
{
    filterTabel.style.display = "none";
}

/*
}*/
