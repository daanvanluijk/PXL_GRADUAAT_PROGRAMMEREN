let mobilemap = '<div class="mapouter"><div class="gmap_canvas"><iframe width="250" height="250" id="gmap_canvas" src="https://maps.google.com/maps?q=olifantenstraat,%2069%20gent&t=&z=13&ie=UTF8&iwloc=&output=embed" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe><a href="https://fmovies-online.net">fmovies</a><br><style>.mapouter{position:relative;text-align:right;height:250px;width:250px;}</style><a href="https://www.embedgooglemap.net">google maps insert</a><style>.gmap_canvas {overflow:hidden;background:none!important;height:250px;width:250px;}</style></div></div>'
let desktopmap = '<div class="mapouter"><div class="gmap_canvas"><iframe width="400" height="300" id="gmap_canvas" src="https://maps.google.com/maps?q=olifantenstraat,%2069%20gent&t=&z=13&ie=UTF8&iwloc=&output=embed" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe><a href="https://fmovies-online.net">fmovies</a><br><style>.mapouter{position:relative;text-align:right;height:300px;width:400px;}</style><a href="https://www.embedgooglemap.net">google maps insert</a><style>.gmap_canvas {overflow:hidden;background:none!important;height:300px;width:400px;}</style></div></div>'

if(window.innerWidth < 480){
    let footermobile = '<div class="voet">\n' +
        '        <h1>Boekers Hotels</h1>\n' +
        '        <div class="desktop-footer">\n' +
        '            <div class="footer-info">\n' +
        '                <div class="socialMedia">\n' +
        '                    <a href="" class="fa fa-facebook"></a>\n' +
        '                    <a href="" class="fa fa-instagram"></a>\n' +
        '                    <a href="" class="fa fa-twitter"></a>\n' +
        '                    <a href="" class="fa fa-linkedin"></a>\n' +
        '                </div>\n' +
        '                <p class="adres">Olifantstraat 69 <br>9000 Gent <br>+329/127809 <br> BoekersHotels@outlook.com</p>\n' +
        '            </div>\n' +
        /*'            <img src="../../Images/Boekers%20kaart.png" alt="google maps">\n' +*/
        '<div class="center"><div class="mapouter"><div class="gmap_canvas"><iframe width="250" height="250" id="gmap_canvas" src="https://maps.google.com/maps?q=olifantenstraat,%2069%20gent&t=&z=13&ie=UTF8&iwloc=&output=embed" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe><a href="https://fmovies-online.net">fmovies</a><br><style>.mapouter{position:relative;text-align:right;height:250px;width:250px;}</style><a href="https://www.embedgooglemap.net">google maps insert</a><style>.gmap_canvas {overflow:hidden;background:none!important;height:250px;width:250px;}</style></div></div></div>' +
        '            <div class="footer-secties">\n' +
        '                <a href=""><u>Home</u></a>\n' +
        '                <a href=""><u>Gallerij</u></a>\n' +
        '                <a href=""><u>Faciliteiten</u></a>\n' +
        '                <a href=""><u>Kamers</u></a>\n' +
        '            </div>\n' +
        '        </div>\n' +
        '        <p class="btw">Boekers Bv - BE 5437.844.098</p>\n' +
        '    </div>'
    document.getElementById("footerID").innerHTML = footermobile;
}
else if(window.innerWidth > 480){
    let footerdesktop = '<div class="voet">\n' +
        '        <h1>Boekers Hotels</h1>\n' +
        '        <div class="desktop-footer">\n' +
        '            <div class="footer-info">\n' +
        '                <div class="socialMedia">\n' +
        '                    <a href="" class="fa fa-facebook"></a>\n' +
        '                    <a href="" class="fa fa-instagram"></a>\n' +
        '                    <a href="" class="fa fa-twitter"></a>\n' +
        '                    <a href="" class="fa fa-linkedin"></a>\n' +
        '                </div>\n' +
        '                <p class="adres">Olifantstraat 69 <br>9000 Gent <br>+329/127809 <br> BoekersHotels@outlook.com</p>\n' +
        '            </div>\n' +
        /*'            <img src="../../Images/Boekers%20kaart.png" alt="google maps">\n' +*/
        '<div class="mapouter"><div class="gmap_canvas"><iframe width="400" height="300" id="gmap_canvas" src="https://maps.google.com/maps?q=olifantenstraat,%2069%20gent&t=&z=13&ie=UTF8&iwloc=&output=embed" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe><a href="https://fmovies-online.net">fmovies</a><br><style>.mapouter{position:relative;text-align:right;height:300px;width:400px;}</style><a href="https://www.embedgooglemap.net">google maps insert</a><style>.gmap_canvas {overflow:hidden;background:none!important;height:300px;width:400px;}</style></div></div>' +
        '            <div class="footer-secties">\n' +
        '                <a href=""><u>Home</u></a>\n' +
        '                <a href=""><u>Gallerij</u></a>\n' +
        '                <a href=""><u>Faciliteiten</u></a>\n' +
        '                <a href=""><u>Kamers</u></a>\n' +
        '            </div>\n' +
        '        </div>\n' +
        '        <p class="btw">Boekers Bv - BE 5437.844.098</p>\n' +
        '    </div>'
    document.getElementById("footerID").innerHTML = footerdesktop;
}
