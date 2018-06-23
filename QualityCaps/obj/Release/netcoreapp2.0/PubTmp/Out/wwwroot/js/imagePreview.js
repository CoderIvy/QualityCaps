// JS used to display image preview for creating and editing bag items.

var fileInput = document.querySelector('#uploadImg');
var previewFrame = document.querySelector('#imgPreview');

fileInput.addEventListener('change', function () {

    //Remove current image 
    previewFrame.setAttribute("src", "");

    if (!fileInput.value) {
        return;
    }

    //Get the image
    var file = fileInput.files[0];
    //Check file type 
    if (file.type !== 'image/jpeg' && file.type !== 'image/png' && file.type !== 'image/gif' && file.type !== 'image/bmp') {
        alert("Please upload image file");
        return;
    }

    //Read and load image to preview
    var reader = new FileReader();
    reader.addEventListener('load', function (e) {
        var data = e.target.result;
        previewFrame.setAttribute('src', data);
    });
    reader.readAsDataURL(file);
});