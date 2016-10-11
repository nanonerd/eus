var eusCommon = {};   // create object variable

(function () {
    "use strict";

    eusCommon.urlError = '/'

    //alert(document.getElementById('url').value);

    // eusCommon.url = 'boo'; //document.getElementById('url').value;


    // The point values for user ratings.
    eusCommon.rating = function (value) {
        switch (value) {
            case '0.5':
                return 10;
            case '1':
                return 20;
            case '1.5':
                return 30;
            case '2':
                return 40;
            case '2.5':
                return 50;
            case '3':
                return 60;
            case '3.5':
                return 70;
            case '4':
                return 80;
            case '4.5':
                return 90;
            case '5':
                return 100;
            default:
                return -1;
        }
    };

    // If logged in, store UserID and UserName in local session storage. 

}());