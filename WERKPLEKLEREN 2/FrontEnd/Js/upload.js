const apiUrl = 'https://localhost:44387/api/'
const image_input = document.querySelector("#image-input");
image_input.addEventListener("change", function() {
    //console.log(this.files);
    const reader = new FileReader();
    reader.addEventListener("load", () => {
        const uploaded_image = reader.result;
        document.querySelector("#display-image").style.backgroundImage = `url(${uploaded_image})`;

    });
    reader.readAsDataURL(this.files[0]);
    console.log(this.files);
    PostImage("Images", reader.result);
});


function PostImage (controller, image)
{
    const messageHeaders = new Headers({ // (1)
        'Content-Type': 'application/json'
    })
    fetch(apiUrl + controller, {
        method: 'POST',
        body: JSON.stringify(image),
        headers: messageHeaders
    })
        .then(value => value.json())
        .then(value => handelImage(value))
}

function handelImage(value)
{
    console.log(value);
}