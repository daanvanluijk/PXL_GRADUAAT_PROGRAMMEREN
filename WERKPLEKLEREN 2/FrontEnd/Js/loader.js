function LoadingScreen (event = "load")
{
        let loader = document.getElementById("preloader");

        window.addEventListener(event, function (){
                loader.style.display = "none";
        })
}
console.log(document.getElementById("loader"));
//let autorun = document.getElementById("loader").getAttribute("data-autorun");
if(!document.getElementById("loader")){
        LoadingScreen();
}