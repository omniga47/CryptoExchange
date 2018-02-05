var t = new Date();
var table1Count = 0;
var table2Count = 0;

function diff_minutes(dt2, dt1) {

    var diff = (dt2.getTime() - dt1.getTime()) / 1000;
    diff /= 60;
    return Math.abs(Math.round(diff));

}

function diff_seconds(dt2, dt1) {

    var diff = (dt2.getTime() - dt1.getTime()) / 1000;


    return Math.abs(diff);

}


function sidebarToggleOnClick() {
    $('#sidebar-toggle-button').on('click', function () {
        $('#sidebar').toggleClass('sidebar-toggle');
        $('#page-content-wrapper').toggleClass('page-content-toggle');
        renderAllCharts();
    });
}

function LoadAltKraData() {

    var dataSet = [];

    sidebarToggleOnClick();

    var params = {};
    params.USDRate = $("#usdExchangeRate").val();
    params.AltCoinRate = $("#altCoinRate").val();
    params.KrakenRate = $("#krakenRate").val();


    $.ajax({
        url: "../handler/CurrencyHandler.ashx",
        async: false,
        type: "POST",
        dataType: "json",
        data: { "t": t.getMilliseconds(), "function": "GetAltCoinTraderData", "param": JSON.stringify(params) },
        success: function (data) {

            if (data.MessageId == 0) {

                //go and get the last updated stats
                $.ajax({
                    url: "../handler/CurrencyHandler.ashx",
                    async: true,
                    type: "POST",
                    dataType: "json",
                    data: { "t": t.getMilliseconds(), "function": "GetLastUpdatedAltCoinTraderPage", "param": JSON.stringify(params) },
                    success: function (data) {
                        if (data.MessageId == 0) {

                            var split = data.Result.split('|');

                            $("#LastUpdateKra").text(split[0]);
                            $("#LastUpdateAlt").text(split[1]);

                            var today = new Date();
                            var krakenDate = new Date(split[0]);
                            var altCoinDate = new Date(split[0]);
                            //var diffMinsKraken = Math.round(((diffMsKraken % 86400000) % 3600000) / 60000); // minutes
                            //var diffMinsAltCoin = Math.round(((diffMsAltCoin % 86400000) % 3600000) / 60000); // minutes
                            var diffMinsKraken = diff_minutes(krakenDate, today); //Math.round(diffMsKraken / 60000); // minutes
                            var diffMinsAltCoin = diff_minutes(altCoinDate, today); //Math.round(diffMsAltCoin / 60000); // minutes
                            var diffMinsAltCoinSec = diff_seconds(altCoinDate, today);

                            if (diffMinsAltCoinSec < 60) {
                                $("#LastUpdateAltMin").text("[" + Math.round(diffMinsAltCoinSec) + " seconds]");
                            }
                            else {
                                $("#LastUpdateAltMin").text("[" + diffMinsAltCoin + " min]");
                            }

                            $("#LastUpdateKraMin").text("[" + diffMinsKraken + " min]");
                            
                        }
                    }
                });


                if (data.Result != null) {
                    for (var i = 0; i < data.Result.length; i++) {
                        dataSet.push(data.Result[i]);
                    }
                }

                table = $('#AltKrakenUSDData1').DataTable({
                    "destroy": true,
                    "processing": true,
                    //"sDom": '<"top"lif>rt<"bottom"p><"clear">',
                    "sDom": '<"top">rt<"bottom"p><"clear">',
                    scrollY: 450,
                    scrollCollapse: true,
                    paging: false,
                    "bAutoWidth": true,
                    //"bLengthChange": false,
                    //"pageLength": 15,
                    //"pagingType": "full_numbers",
                    "data": dataSet,
                    "columns": [
			    {
			        'className': 'details-control',
			        'orderable': false,
			        'data': null,
			        'defaultContent': '',
			    },
                            { "title": "Crypto Coin", "data": "CryptoCoin", "orderable": "false" },
                            { "title": "Ask price (FX)", "data": "AskPrice" },

                            { "title": "USP (ZAR) [25k]", "data": "Less25k" },
                            { "title": "OV (Crypto) [25k]", "data": "OrderVolumeLess25k" },
                            { "title": "OC (ZAR) [25k]", "data": "OrderCostLess25k" },
                            { "title": "Mrg % [25k]", "data": "MrgLess25k" },

                            { "title": "USP (ZAR) [25-50k]", "data": "Btw25k50k" },
                            { "title": "OV (Crypto) [25-50k]", "data": "OrderVolumeBtw25k50k" },
                            { "title": "OC (ZAR) [25-50k]", "data": "OrderCostBtw25k50k" },
                            { "title": "Mrg % [25-50k]", "data": "MrgBtw25k50k" },


                            { "title": "USP (ZAR) [50-100k]", "data": "Btw50k100k" },
                            { "title": "OV (Crypto) [50-100k]", "data": "OrderVolumeBtw50k100k" },
                            { "title": "OC (ZAR) [50-100k]", "data": "OrderCostBtw50k100k" },
                            { "title": "Mrg % [50-100k]", "data": "MrgBtw50k100k" },

                    ],
                    rowCallback: function (row, data, index) {

                        var i = 0;
                        if (table1Count == 0)
                            $("#AltKrakenUSDData1_wrapper").children().children().children().children().children().prepend("<thead class='child-div'>some text</thead>");


                        var highlightVal = $("#hightlightMrg").val();
                        var hasHighlighted = false;

                        if (highlightVal == '')
                            highlightVal = 5;

                        if (data.MrgLess25k >= new Number(highlightVal)) {
                            $(row).find('td:eq(6)').css('color', 'red');
                            $(row).find('td:eq(6)').css('font-weight', 'bold');
                            hasHighlighted = true;
                        }
                        if (data.MrgBtw25k50k >= new Number(highlightVal)) {
                            $(row).find('td:eq(10)').css('color', 'red');
                            $(row).find('td:eq(10)').css('font-weight', 'bold');
                            hasHighlighted = true;
                        }
                        if (data.MrgBtw50k100k >= new Number(highlightVal)) {
                            $(row).find('td:eq(14)').css('color', 'red');
                            $(row).find('td:eq(14)').css('font-weight', 'bold');
                            hasHighlighted = true;
                        }

                        if (hasHighlighted) {
                            $(row).find('td:eq(1)').css('color', 'green');
                            $(row).find('td:eq(1)').css('font-weight', 'bold');

                            var currExchange = $(row).find('td:eq(0)');
                            var html = "<img src='../Images/alert_icon.png'>";
                            $(currExchange).prepend(html);
                        }

                        table1Count++;
                    }
                });

                table2 = $('#AltKrakenUSDData2').DataTable({
                    "destroy": true,
                    "processing": true,
                    //"sDom": '<"top"lif>rt<"bottom"p><"clear">',
                    "sDom": '<"top">rt<"bottom"p><"clear">',
                    scrollY: 450,
                    scrollCollapse: true,
                    paging: false,
                    "bAutoWidth": true,
                    //"bLengthChange": false,
                    //"pageLength": 15,
                    //"pagingType": "full_numbers",
                    "data": dataSet,
                    "columns": [
			    {
			        'className': 'details-control',
			        'orderable': false,
			        'data': null,
			        'defaultContent': '',
			    },
                            { "title": "Crypto Coin", "data": "CryptoCoin", "orderable": "false" },
                            { "title": "Ask price (FX)", "data": "AskPrice" },

                            { "title": "USP (ZAR) [100-150k]", "data": "Btw100k150k" },
                            { "title": "OV (Crypto) [100-150k]", "data": "OrderVolumeBtw100k150k" },
                            { "title": "OC (ZAR) [100-150k]", "data": "OrderCostBtw100k150k" },
                            { "title": "Mrg % [100-150k]", "data": "MrgBtw100k150k" },

                            { "title": "USP (ZAR) [150-200k]", "data": "Btw150k200k" },
                            { "title": "OV (Crypto) [150-200k]", "data": "OrderVolumeBtw150k200k" },
                            { "title": "OC (ZAR) [150-200k]", "data": "OrderCostBtw150k200k" },
                            { "title": "Mrg % [150-200k]", "data": "MrgBtw150k200k" },

                            { "title": "USP (ZAR) [>200k]", "data": "Grt200k" },
                            { "title": "OV (Crypto) [>200k]", "data": "OrderVolumeGrt200k" },
                            { "title": "OC (ZAR) [>200k]", "data": "OrderCostGrt200k" },
                            { "title": "Mrg % [>200k]", "data": "MrgGrt200k" },

                    ],
                    rowCallback: function (row, data, index) {

                        // if (table2Count == 0)
                        //    $("#AltKrakenUSDData2_wrapper").children().children().children().children().children().prepend("<thead class='child-div'>some text</thead>");

                        var i = 0;
                        var highlightVal = $("#hightlightMrg").val();
                        var hasHighlighted = false;
                        if (highlightVal == '')
                            highlightVal = 5;

                        if (data.MrgBtw100k150k >= new Number(highlightVal)) {
                            $(row).find('td:eq(6)').css('color', 'red');
                            $(row).find('td:eq(6)').css('font-weight', 'bold');
                            hasHighlighted = true;
                        }
                        if (data.MrgBtw150k200k >= new Number(highlightVal)) {
                            $(row).find('td:eq(10)').css('color', 'red');
                            $(row).find('td:eq(10)').css('font-weight', 'bold');
                            hasHighlighted = true;
                        }
                        if (data.MrgGrt200k >= new Number(highlightVal)) {
                            $(row).find('td:eq(14)').css('color', 'red');
                            $(row).find('td:eq(14)').css('font-weight', 'bold');
                            hasHighlighted = true;
                        }

                        if (hasHighlighted) {
                            $(row).find('td:eq(1)').css('color', 'green');
                            $(row).find('td:eq(1)').css('font-weight', 'bold');
                            var currExchange = $(row).find('td:eq(0)');
                            var html = "<img src='../Images/alert_icon.png'>";
                            $(currExchange).prepend(html);
                        }

                        table2Count++;
                    }
                });

                //update the time
                var now = new Date();

                var min = now.getMinutes();
                if (min.toString().length == 1)
                    min = "0" + min;

                var sec = now.getSeconds();
                if (sec.toString().length == 1)
                    sec = "0" + sec;

                var day = now.getDate();
                if (day.toString().length == 1)
                    day = "0" + day;

                var mon = now.getMonth() + 1;
                if (mon.toString().length == 1)
                    mon = "0" + mon;

                var hours = now.getHours();
                if (hours.toString().length == 1)
                    hours = "0" + hours;

                var inres = mon + "/" + day + "/" + now.getFullYear() + " " + hours + ":" + min + ":" + sec + " ";

                $("#AltKraTable").text(inres);



                // Add event listener for opening and closing details
                //$('#AltKrakenUSDData').unbind().on('click', 'td.details-control', function () {
                //    var tr = $(this).closest('tr');

                //    var selectedEntity = tr[0].id;
                //    //open popup
                //    //CreatePopup("../Controls/User/AddEditUser.aspx", 'Edit User', 400, 2, selectedEntity, 'saveUserPopup', 750, 2);
                //});
            } else {

                HideProgressAnimation();

                alert('An error has occurred, please contact your administrator.');
                return;
            }
        }
    });
}

/*

function saveUserPopup() {
    var params = {};
    var dataSet = [];
    
    var popupDoc = GetDoc(popupDialog[0].children[0]);
    var holdingText = 'PopupPagePlaceHolder_ctl01_';

    params.EntityGuid = popupDoc.getElementById('EntityGuid').value;
    params.Username = popupDoc.getElementById(holdingText + 'txtUsername').value;
    params.FirstName = popupDoc.getElementById(holdingText + 'txtFirstName').value;
    params.Surname = popupDoc.getElementById(holdingText + 'txtSurname').value;
    params.UserType = popupDoc.getElementById(holdingText + 'ddlUserType').value;
    params.Active = popupDoc.getElementById(holdingText + 'ddlActive').value;
    params.Password = popupDoc.getElementById(holdingText + 'txtPassword').value;
    params.PasswordConfirm = popupDoc.getElementById(holdingText + 'txtConfirmPassword').value;

    $.ajax({
        url: "../Handlers/SaveHandler.ashx",
        async: false,
        type: "POST",
        dataType: "json",
        data: { "t": t.getMilliseconds(), "function": "SaveUser", "param": JSON.stringify(params) },
        success: function (data) {

            if (data.MessageId == 0) {

                //load data table with the data just uploaded
                $("[id$=hfDataImportGuid]").val(params.EntityGuid);
                LoadUsers();

                //close the dialog
                RemovePopup();

                HideProgressAnimation(true);
            }
            else if (data.MessageId == 2) {
                alert('password miss match');
            }
            else {
                alert('Did not update');
            }
        }
    });

}


*/