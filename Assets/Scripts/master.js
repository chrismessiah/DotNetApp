window.$ = require('jquery');
var sr = require('scrollreveal')();

$(document).ready(function() {
  console.log("Hello World!");
  sr.reveal('.reveal', { duration: 800 });
})
