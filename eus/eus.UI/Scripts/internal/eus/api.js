var eusAPI = {};   // create global object variable

(function () {

    //$document.ready(function(){




    /////////////////////////////////////////////////
    //////////////// GET ////////////////////////////
    /////////////////////////////////////////////////
    // isAsync parameter is optional
    eusAPI.get = function (url, isAsync) {
        try {
            var response = null;
            if (isAsync === null || isAsync === undefined) {
                isAsync = false;
            }

            $.ajax({
                type: "get",
                url: url,
                contentType: "application/json; charset=UTF-8",
                async: isAsync,
                dataType: "json"
            })
               .done(function (result) {
                   response = result
               })
        }

        catch (ex) {
            response = ex.message;
            eusAPI.errorHandle(ex.message);
        };

        return response;
    };


    /////////////////////////////////////////////////
    //////////////// POST ///////////////////////////
    /////////////////////////////////////////////////
    eusAPI.post = function (url, data, isAsync) {
        try {
            var response = null;
            if (isAsync === null || isAsync === undefined) {
                isAsync = false;
            }

            $.ajax({
                type: "post",
                url: url,
                data: data,
                contentType: "application/json; charset=UTF-8",
                async: isAsync,
                dataType: "json"
            })
               .done(function (result) {
                   response = result
               })
        }

        catch (ex) {
            response = ex.message;
            eusAPI.errorHandle(ex.message);
        };

        return response;
    };

    // Handle errors by logging in eusCommon.dbo.Admin_ErrorLog
    eusAPI.errorHandle = function (data) {
        try {
            var response = null;
            var url = '/Common';
            var jsonData = JSON.stringify({ "Filter": "ErrorHandle", "FilterData": data });

            $.ajax({
                type: "post",
                url: url,
                data: jsonData,
                contentType: "application/json; charset=UTF-8",
                dataType: "json"
            })
               .done(function (result) {
                   response = result
               })
        }

        catch (ex) {
            response = ex;
        };

        return response;
    };

    //});
})();