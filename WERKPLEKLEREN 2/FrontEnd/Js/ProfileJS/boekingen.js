let newOrder = { userId : parseInt( localStorage.getItem("userId"))};



postRequest('Order/Paid', newOrder, 'renderRoomsBooked');


function renderRoomsBooked (result)
{
    result = JSON.parse(result);
    console.log(result);
    const profiel_pagina = document.querySelector(".profiel_pagina");
   // const profiel_row = document.createElement("div");
    const h1 = document.createElement("h1");

    h1.innerHTML = "Reservatie's";

    for (let i = 0; i < result.length; i++) {
        console.log(parseInt(localStorage.getItem('userId')));
        if(parseInt(localStorage.getItem('userId')) === result[i].userID)
        {
            const roomInfo = document.createElement("p");
            roomInfo.innerHTML = `${result[i].roomTitle}   incheckdatum: ${result[i].checkinDate.slice(0,10)} uitcheckdatum: ${result[i].checkoutDate.slice(0,10)}
             `;
            h1.appendChild(roomInfo);
        }

    }

    profiel_pagina.appendChild(h1)

}

