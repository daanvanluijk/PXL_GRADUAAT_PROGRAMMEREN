// naam: van Luijk Daan

window.addEventListener("load", handleLoad);

function handleLoad () {	
	
	let button_start_rekenen;
	
	// schrijf hier de code voor het initialiseren van de button
	button_start_rekenen = document.querySelectorAll("button")[0];
	
	button_start_rekenen.addEventListener("click",handleClick );
}


function handleClick () {
	let aantal = parseInt(document.getElementById("input_aantal").value);
	console.log(aantal);
	for (let i = 0; i < aantal; i++) {
		let getal1 = parseInt(10 * Math.random());
		let getal2 = parseInt(10 * Math.random());
		let hr = document.createElement("hr");
		document.body.appendChild(hr);
		let div = document.createElement("div");
		document.body.appendChild(div);
		div.innerText = getal1 + " * " + getal2 + " = ";
		let input = document.createElement("input");
		input.type = "text";
		div.appendChild(input);
		input.addEventListener("keyup", handleKeyupInput);
	}


}

function handleKeyupInput(event){
	console.log(event);
	if ((event.keyCode < 96 || event.keyCode > 105) && event.keyCode !== 8) {
		event.target.style.color = "red";
	}
	else {
		event.target.style.color = "black";
	}
}

