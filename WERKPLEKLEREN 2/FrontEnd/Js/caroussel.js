const data = [
    {roomName:'one',
        photos:['src1','src2']
    }
]
let image = document.querySelector("#carousel-img");
let fade = document.getElementsByClassName("fade");
let dotIndex = 0;
let slideTextNumber = document.querySelector(".caroussel-slide-number");
let selectedDots = document.getElementsByClassName("caroussel-dot");

selectedDots[0].style.backgroundColor = "black";
showImage(1);

let currentUpdateDotsTimeout = setTimeout(updateDots, 2000);

function updateDots()
{
    dotIndex++;
    dotIndex %= selectedDots.length;
    console.log(dotIndex);
    selectedDots[dotIndex].style.backgroundColor = "black";
    selectedDots[(dotIndex + selectedDots.length - 1)%selectedDots.length].style.backgroundColor = "#bbb";

    showImage(dotIndex + 1);
    clearTimeout(currentUpdateDotsTimeout);
    currentUpdateDotsTimeout = setTimeout(updateDots, 2000);
}
function clearDots() {
    for (let i = 0; i < selectedDots.length; i++) {
        selectedDots[i].style.backgroundColor = "#bbb";
    }
}
<!--Dots selector-->
for (let i = 0; i < selectedDots.length; i++)
{
    selectedDots[i].addEventListener("click", () =>
    {
        showImage(i + 1);
        clearDots();
        selectedDots[i].style.backgroundColor = "black";
        dotIndex = i;
        clearTimeout(currentUpdateDotsTimeout);
        currentUpdateDotsTimeout = setTimeout(updateDots, 3000);
        console.log("clicked");
    })
}
function showImage(index)
{
    image.src = `/Images/Caroussel/foto${index}.jpg`;


}