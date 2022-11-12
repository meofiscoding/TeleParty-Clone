"use strict";
//document ready

$(document).ready(function () {
    debugger;
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build(); 
     

});