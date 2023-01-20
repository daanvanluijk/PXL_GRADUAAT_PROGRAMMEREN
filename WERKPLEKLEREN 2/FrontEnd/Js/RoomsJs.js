let expandImg = document.getElementById("expandedImg");
let smalImg = document.getElementsByClassName("thumbnail");


for (let i = 0; i < smalImg.length; i++) {

    smalImg[i].addEventListener("click", () =>
        {
            expandImg.src = smalImg[i].src;
        }
    )
}
