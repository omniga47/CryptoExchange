$(function () {

	var activeUsers = 55,
			pageViewsPerSecondLowerLimit,
			pageViewsPerSecondUpperLimit,
			yValuePageViewsPerSecond,
			sumYValuePageViewsPerSecond = 0,
			numberOfSeconds = 1,
			updateChartsInterval,
			updateChartsIntervalLowerLimit = 4000, // milliseconds
			updateChartsIntervalUpperLimit = 6000, // milliseconds
			timeoutIdUpdateCharts;
	
	var pageViewsPerSecondDataPoints = [];
	var pageViewsPerMinuteDataPoints = [];
	
	// data for demo only
	var initialDataPageViewsPerSecond = [1,2,4,4,3,2,1,5,1,2,2,0,0,1,2,4,5,3,4,2,2,2,2,0,1,2,4,4,4,5,5,1,2,4,1,1,1,0,0,1,2,3,3,5,5,2,0,1,1,0,2,2,2,0,4,1,4,4,2,2];
	var initialDataPageViewsPerMinute = [110,107,122,107,128,108,100,110,118,93,95,112,108,95,96,114,107,105,124,104,131,94,109,110,108,99,104,90,104,109,89,121,118,93,109,113,106,100,101,119,76,137,112,104,98,89,104,96,120,111,108,95,93,100,101,110,98,105,107,135];
	
	// data for demo only
	var data = [
		{
			activeUsers: 55,
			pageViewsPerSecondLowerLimit: 0,
			pageViewsPerSecondUpperLimit: 3,

			device: [
				{ name: "Desktop", users: 39 },
				{ name: "Tablet", users: 5 },
				{ name: "Mobile", users: 11 }
			],
			trafficMedium: [
				{ name: "Organic", users: 38 },
				{ name: "Direct", users: 8 },
				{ name: "Paid", users: 5 },
				{ name: "Referral", users: 4 }
			],
			categories: [
				{ name: "Men Clothing", users: 8 },
				{ name: "Women Clothing", users: 9 },
				{ name: "Gadgets", users: 10 },
				{ name: "Books", users: 5 },
				{ name: "Others", users: 23 }
			],
			states: [
				{ name: "Others", users: 16 },
				{ name: "Pennsylvania", users: 4 },
				{ name: "Florida", users: 5 },
				{ name: "Texas", users: 7 },
				{ name: "New York", users: 11 },
				{ name: "California", users: 12 },
			]
		},
		{
			activeUsers: 56,
			pageViewsPerSecondLowerLimit: 0,
			pageViewsPerSecondUpperLimit: 3,

			device: [
				{ name: "Desktop", users: 40 },
				{ name: "Tablet", users: 5 },
				{ name: "Mobile", users: 11 }
			],
			trafficMedium: [
				{ name: "Organic", users: 39 },
				{ name: "Direct", users: 8 },
				{ name: "Paid", users: 5 },
				{ name: "Referral", users: 4 }
			],
			categories: [
				{ name: "Men Clothing", users: 9 },
				{ name: "Women Clothing", users: 9 },
				{ name: "Gadgets", users: 10 },
				{ name: "Books", users: 5 },
				{ name: "Others", users: 23 }
			],
			states: [
				{ name: "Others", users: 17 },
				{ name: "Pennsylvania", users: 4 },
				{ name: "Florida", users: 5 },
				{ name: "Texas", users: 7 },
				{ name: "New York", users: 11 },
				{ name: "California", users: 12 },
			]
		},
		{
			activeUsers: 57,
			pageViewsPerSecondLowerLimit: 0,
			pageViewsPerSecondUpperLimit: 3,

			device: [
				{ name: "Desktop", users: 41 },
				{ name: "Tablet", users: 5 },
				{ name: "Mobile", users: 11 }
			],
			trafficMedium: [
				{ name: "Organic", users: 39 },
				{ name: "Direct", users: 9 },
				{ name: "Paid", users: 5 },
				{ name: "Referral", users: 4 }
			],
			categories: [
				{ name: "Men Clothing", users: 8 },
				{ name: "Women Clothing", users: 9 },
				{ name: "Gadgets", users: 11 },
				{ name: "Books", users: 5 },
				{ name: "Others", users: 24 }
			],
			states: [
				{ name: "Others", users: 17 },
				{ name: "Pennsylvania", users: 4 },
				{ name: "Florida", users: 5 },
				{ name: "Texas", users: 7 },
				{ name: "New York", users: 12 },
				{ name: "California", users: 12 },
			]
		},
		{
			activeUsers: 58,
			pageViewsPerSecondLowerLimit: 0,
			pageViewsPerSecondUpperLimit: 3,

			device: [
				{ name: "Desktop", users: 42 },
				{ name: "Tablet", users: 5 },
				{ name: "Mobile", users: 11 }
			],
			trafficMedium: [
				{ name: "Organic", users: 40 },
				{ name: "Direct", users: 8 },
				{ name: "Paid", users: 6 },
				{ name: "Referral", users: 4 }
			],
			categories: [
				{ name: "Men Clothing", users: 9 },
				{ name: "Women Clothing", users: 10 },
				{ name: "Gadgets", users: 11 },
				{ name: "Books", users: 4 },
				{ name: "Others", users: 24 }
			],
			states: [
				{ name: "Others", users: 17 },
				{ name: "Pennsylvania", users: 4 },
				{ name: "Florida", users: 6 },
				{ name: "Texas", users: 7 },
				{ name: "New York", users: 12 },
				{ name: "California", users: 12 },
			]
		},
		{
			activeUsers: 59,
			pageViewsPerSecondLowerLimit: 0,
			pageViewsPerSecondUpperLimit: 4,

			device: [
				{ name: "Desktop", users: 43 },
				{ name: "Tablet", users: 4 },
				{ name: "Mobile", users: 12 }
			],
			trafficMedium: [
				{ name: "Organic", users: 41 },
				{ name: "Direct", users: 8 },
				{ name: "Paid", users: 6 },
				{ name: "Referral", users: 4 }
			],
			categories: [
				{ name: "Men Clothing", users: 9 },
				{ name: "Women Clothing", users: 10 },
				{ name: "Gadgets", users: 11 },
				{ name: "Books", users: 5 },
				{ name: "Others", users: 24 }
			],
			states: [
				{ name: "Others", users: 17 },
				{ name: "Pennsylvania", users: 4 },
				{ name: "Florida", users: 6 },
				{ name: "Texas", users: 7 },
				{ name: "New York", users: 12 },
				{ name: "California", users: 13 },
			]
		},
		{
			activeUsers: 60,
			pageViewsPerSecondLowerLimit: 0,
			pageViewsPerSecondUpperLimit: 4,

			device: [
				{ name: "Desktop", users: 43 },
				{ name: "Tablet", users: 5 },
				{ name: "Mobile", users: 12 }
			],
			trafficMedium: [
				{ name: "Organic", users: 40 },
				{ name: "Direct", users: 9 },
				{ name: "Paid", users: 6 },
				{ name: "Referral", users: 5 }
			],
			categories: [
				{ name: "Men Clothing", users: 9 },
				{ name: "Women Clothing", users: 10 },
				{ name: "Gadgets", users: 12 },
				{ name: "Books", users: 5 },
				{ name: "Others", users: 24 }
			],
			states: [
				{ name: "Others", users: 18 },
				{ name: "Pennsylvania", users: 3 },
				{ name: "Florida", users: 6 },
				{ name: "Texas", users: 8 },
				{ name: "New York", users: 12 },
				{ name: "California", users: 13 },
			]
		},
		{
			activeUsers: 61,
			pageViewsPerSecondLowerLimit: 0,
			pageViewsPerSecondUpperLimit: 5,

			device: [
				{ name: "Desktop", users: 44 },
				{ name: "Tablet", users: 5 },
				{ name: "Mobile", users: 12 }
			],
			trafficMedium: [
				{ name: "Organic", users: 42 },
				{ name: "Direct", users: 9 },
				{ name: "Paid", users: 5 },
				{ name: "Referral", users: 5 }
			],
			categories: [
				{ name: "Men Clothing", users: 9 },
				{ name: "Women Clothing", users: 10 },
				{ name: "Gadgets", users: 12 },
				{ name: "Books", users: 5 },
				{ name: "Others", users: 25 }
			],
			states: [
				{ name: "Others", users: 18 },
				{ name: "Pennsylvania", users: 3 },
				{ name: "Florida", users: 6 },
				{ name: "Texas", users: 8 },
				{ name: "New York", users: 13 },
				{ name: "California", users: 13 },
			]
		}
	];
	
	CanvasJS.addColorSet("customColorSet", [ 
		"#393f63",
		"#e5d8B0", 
		"#ffb367", 
		"#f98461", 
		"#d9695f",
		"#e05850",
	]);
	
	// CanvasJS doughnut chart to show device type of active users
	var usersDeviceDoughnutChart = new CanvasJS.Chart("users-device-doughnut-chart", {
		animationDuration: 800,
		animationEnabled: true,
		backgroundColor: "transparent",
		colorSet: "customColorSet",
		theme: "theme2",
		legend: {
			fontFamily: "calibri",
			fontSize: 14,
			horizontalAlign: "left",
			verticalAlign: "center",
			itemTextFormatter: function (e) {
				return e.dataPoint.name + ": " + Math.round(e.dataPoint.y / activeUsers * 100) + "%";  
			} 
		},
		title: {
			dockInsidePlotArea: true,
			fontSize: 55,
			fontWeight: "normal",
			horizontalAlign: "center",
			verticalAlign: "center",
			text: "55"
		},
		toolTip: {
			cornerRadius: 0,
			fontStyle: "normal",
			contentFormatter: function (e) {
				return e.entries[0].dataPoint.name + ": " + Math.round(e.entries[0].dataPoint.y / activeUsers * 100) + "% (" + e.entries[0].dataPoint.y  + ")";
			} 
		},
		data: [
			{
				innerRadius: "80%",
				radius: "90%",
				legendMarkerType: "square",
				showInLegend: true,
				startAngle: 90,
				type: "doughnut",
				dataPoints: [
					{  y: 39, name: "Desktop" },
					{  y: 5, name: "Tablet" },
					{  y: 11, name: "Mobile" }
				]
			}
		]
	});
	
	//----------------------------------------------------------------------------------//
	var allCharts = [
		usersDeviceDoughnutChart
	];
	
	// generate random number between given range
	function generateRandomNumber (minimum, maximum) {
		return Math.floor(Math.random() * (maximum - minimum + 1)) + minimum;
	}
	
	function updateUsersDeviceChart(dataIndex) {
		usersDeviceDoughnutChart.options.title.text = activeUsers.toString();
		
		for (var i = 0; i < data[dataIndex].device.length; i++)
			usersDeviceDoughnutChart.options.data[0].dataPoints[i].y = data[dataIndex].device[i].users;
		
		usersDeviceDoughnutChart.render();
	}
	
	
	// update all charts with revelant demo data, except "Page Views Per Second" and "Page Views Per Minute" charts
	function updateCharts(dataIndex) {
		activeUsers = data[dataIndex].activeUsers;
		pageViewsPerSecondLowerLimit = data[dataIndex].pageViewsPerSecondLowerLimit;
		pageViewsPerSecondUpperLimit = data[dataIndex].pageViewsPerSecondUpperLimit;

		updateUsersDeviceChart(dataIndex);

	}
	
	function updateChartsAtRandomIntervals() {
		var dataIndex = generateRandomNumber(0, data.length - 1);
		updateChartsInterval = generateRandomNumber(updateChartsIntervalLowerLimit, updateChartsIntervalUpperLimit);
				
		updateCharts(dataIndex);
		
		if (timeoutIdUpdateCharts)
			clearTimeout(timeoutIdUpdateCharts);
		
		timeoutIdUpdateCharts = setTimeout(function () {
			updateChartsAtRandomIntervals();
		}, updateChartsInterval);
	}
	
	// populate "Page Views Per Second" and "Page Views Per Minute" charts with initial data
	
	
	// chart properties cutomized further based on screen width	
	function chartPropertiesCustomization(chart) {
		if ($(window).outerWidth() >= 1920) {			
			
			chart.options.legend.fontSize = 14;
			chart.options.legend.horizontalAlign = "left";
			chart.options.legend.verticalAlign = "center";
			chart.options.legend.maxWidth = null;
			
		}else if ($(window).outerWidth() < 1920 && $(window).outerWidth() >= 1200) {
			
			chart.options.legend.fontSize = 14;
			chart.options.legend.horizontalAlign = "left";
			chart.options.legend.verticalAlign = "center";
			chart.options.legend.maxWidth = 140;
			
		} else if ($(window).outerWidth() < 1200 && $(window).outerWidth() >= 992) {
			
			chart.options.legend.fontSize = 12;
			chart.options.legend.horizontalAlign = "center";
			chart.options.legend.verticalAlign = "top";
			chart.options.legend.maxWidth = null;
			
		} else if ($(window).outerWidth() < 992) {
			
			chart.options.legend.fontSize = 14;
			chart.options.legend.horizontalAlign = "center";
			chart.options.legend.verticalAlign = "bottom";
			chart.options.legend.maxWidth = null;
			
		}
		
		chart.render();
	}
	
	function customizeCharts() {
		chartPropertiesCustomization(usersDeviceDoughnutChart);
	}
	
	function renderAllCharts() {
		for (var i = 0; i < allCharts.length; i++)
			allCharts[i].render();
	}
	
	function sidebarToggleOnClick() {
		$('#sidebar-toggle-button').on('click', function () {
			$('#sidebar').toggleClass('sidebar-toggle');
			$('#page-content-wrapper').toggleClass('page-content-toggle');
			renderAllCharts();
		});	
	}

	function screenScape()
	{
	/*    debugger;
	    $.ajax({
	        url: "https://www.altcointrader.co.za/",
	        dataType: 'text',
	        success: function (data) {
	            debugger;

	            var elements = $("<div>").html(data)[0].getElementsByTagName("ul")[0].getElementsByTagName("li");
	            for (var i = 0; i < elements.length; i++) {
	                var theText = elements[i].firstChild.nodeValue;
	                // Do something here
	            }
	        }
	    });
        */



	}

	function getKrakenTicker() {

	    var dataObj;

	    $.ajax({
	        url: "https://api.kraken.com/0/public/Ticker?pair=XBTUSD",
	        async : false,
	        dataType: 'GET',
	        success: function (data) {
	            debugger;

	            dataObj = data;
	        }
	    });


	    debugger;
	    var code = "XBTUSD";
	    //var altCoinJSON = UrlFetchApp.fetch("https://api.kraken.com/0/public/Ticker?pair=XBTUSD");
	    var data = JSON.parse(altCoinJSON.getContentText());
	    var innerVowel = "";
	    if ("XBTUSD" == code) {
	        innerVowel = "XXBTZUSD";
	    }
	    debugger;

	    var res = JSON.parse(data["result"][0]);
	    var res1 = JSON.parse(res[innerVowel]);
	    var res2 = JSON.parse(res1["a"]);
	    return res2[0];
	}
	
	(function init() {
		//customizeCharts();
		//$(window).resize(customizeCharts);
		//setTimeout(updateChartsAtRandomIntervals, 4000);
	    //sidebarToggleOnClick();
	    getKrakenTicker();

	    screenScape();
	})();
	
});