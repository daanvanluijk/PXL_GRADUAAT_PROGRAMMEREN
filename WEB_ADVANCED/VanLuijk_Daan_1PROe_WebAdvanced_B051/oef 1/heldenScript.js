// schrijf hier de oplossing voor het aanmaken van de html-elementen
let main = document.getElementById("main");
let button = document.createElement("button");
button.innerText = "Superhelden";
button.id = "helden";
main.appendChild(button);
let button2 = document.createElement("button");
button2.innerText = "Avengers Oproepen";
button2.id = "avengers";
main.appendChild(button2);
let div = document.createElement("div");
div.id = "content";
main.appendChild(div);

// schrijf hier de oplossing voor de functie alleKarakters
let list = document.createElement("ul");
function alleKarakters() {
    for (let char of characters.values()) {
        let k = document.createElement("p");
        console.log(char);
        k.innerText = "Naam: " + char.naam + " | Type: " + char.type;
        list.appendChild(k);
    }
    div.appendChild(list);
}
alleKarakters();

// schrijf hier de oplossing voor de functie toonHelden
function toonHelden() {
    list.innerHTML = "";
    for (let char of characters.values()) {
        if (char.type !== "superheld") continue
        let k = document.createElement("p");
        console.log(char);
        k.innerText = "naam: " + char.naam +
            " | kracht: " + char.kracht +
            " | snelheid: " + char.snelheid +
            " | weerstand: " + char.weerstand +
            " | intelligentie: " + char.intelligentie +
            " | type: " + char.type;
        list.appendChild(k);
    }
}
button.addEventListener("click", toonHelden);

// 3 schrijf hier de oplossing voor de functie avengersOproepen
function avengersOproepen() {
    list.innerHTML = "";
    for (let char of characters.values()) {
        if (!(char.snelheid > 8 || char.intelligentie > 9 ||char.kracht === 10) || char.type !== "superheld") continue
        let k = document.createElement("p");
        console.log(char);
        k.innerText = "naam: " + char.naam +
            " | kracht: " + char.kracht +
            " | snelheid: " + char.snelheid +
            " | weerstand: " + char.weerstand +
            " | intelligentie: " + char.intelligentie +
            " | type: " + char.type;
        list.appendChild(k);
    }
}
button2.addEventListener("click", avengersOproepen);