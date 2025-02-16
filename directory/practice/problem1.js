document.addEventListener("DOMContentLoaded", function () {

functionPlot({
    target: '#plot',
    data: [
        { fn: 'x^2', derivative: { fn: '2*x', updateOnMouseMove: true } }
    ]
});


document.getElementById('test').addEventListener('keydown', function (event) {
    if (event.key === 'Enter') {
        if (document.getElementById("test").value == "2") {
            window.wx_msg.postMessage("true");
        } else {
            window.wx_msg.postMessage("false");
        }
}
});

document.getElementById("test").focus();

});
