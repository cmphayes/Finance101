//----------------------------------
//   File          : js/pages/dashboard_analytics.js
//   Type          : JS file
//   Version       : 1.1.0
//   Last Updated  : July 08, 2017
//----------------------------------

// ---------------------------------
// Table of contents
// ---------------------------------
// 1. Overview
// 2. Traffic Type
// 3. Real Time
// 4. World map


// ---------------------------------
// 1. Overview
// ---------------------------------
$(function () {
    Highcharts.chart('leads', {
        chart: {
            type: 'areaspline',
            width: null,
            height: 250,
            spacingBottom: 20,
			spacingTop: 20,
			spacingLeft: 0,
			spacingRight: 0,
            backgroundColor: 'transparent',
        },
        plotOptions: {
			spline: {
                lineWidth: 2,
                states: {
                    hover: {
                        lineWidth: 3
                    }
                },
                marker: {
                    enabled: false
                },
                pointInterval: 1,
            }
        },
		title: {
			text: null
		},
		subtitle: {
			text: null
		},
		xAxis: {
            labels: {enabled:true},
			lineWidth: 1,
			tickLength: 1,
			gridLineWidth: 1,
			categories: ['Jun 01','Jun 02','Jun 03','Jun 04','Jun 05','Jun 06','Jun 07','Jun 08','Jun 09','Jun 10','Jun 11','Jun 12','Jun 13','Jun 14','Jun 15','Jun 16','Jun 17','Jun 18','Jun 19','Jun 20','Jun 21','Jun 22','Jun 23','Jun 24','Jun 25','Jun 26']
		},
		yAxis: {
			title: {
				text: 'Views'
			},
			labels: {enabled:true},
			gridLineWidth: 1
		},
		legend: {
			enabled: false
		},
		credits: {
			enabled: false
		},
		tooltip: {
			enabled:true
		},
        series: [{
			name: 'Views',
			showSymbol: true,
            color: chart_data_color_option1,
            data: [12,23,21,19,34,25,9,24,13,19,39,32,23,21,19,34,25,19,24,13,19,23,21,19,34,25]
        }],
    });
});


// ---------------------------------
// 2. Traffic Type
// ---------------------------------
$('.search').easyPieChart({
    easing: 'easeOutBounce',
    animate: 3500,
    size:100,
    lineWidth:12,
    lineCap:'round',
    trackColor: '#DCDCDC',
    barColor:chart_data_color_option5,
    onStep: function(from, to, percent) {
        $(this.el).find('.per_search').text(Math.round(percent));
    }
});
var chart = window.chart = $('.search').data('easyPieChart');

$('.direct').easyPieChart({
    easing: 'easeOutBounce',
    animate: 3500,
    size:100,
    lineWidth:12,
    lineCap:'round',
    trackColor: '#DCDCDC',
    barColor:chart_data_color_option6,
    onStep: function(from, to, percent) {
        $(this.el).find('.per_direct').text(Math.round(percent));
    }
});
var chart = window.chart = $('.direct').data('easyPieChart');

$('.reff').easyPieChart({
    easing: 'easeOutBounce',
    animate: 3500,
    size:100,
    lineWidth:12,
    lineCap:'round',
    trackColor: '#DCDCDC',
    barColor:chart_data_color_option7,
    onStep: function(from, to, percent) {
        $(this.el).find('.per_reff').text(Math.round(percent));
    }
});
var chart = window.chart = $('.reff').data('easyPieChart');

$('.other').easyPieChart({
    easing: 'easeOutBounce',
    animate: 3500,
    size:100,
    lineWidth:12,
    lineCap:'round',
    trackColor: '#DCDCDC',
    barColor:chart_data_color_option8,
    onStep: function(from, to, percent) {
        $(this.el).find('.per_other').text(Math.round(percent));
    }
});
var chart = window.chart = $('.other').data('easyPieChart');


// ---------------------------------
// 3. Real Time
// ---------------------------------
Morris.Donut({
  element: 'donut-example',
  data: [
    {label: "New Visitors", value: 21},
    {label: "Returning Visitors", value: 79}
  ],
  colors: colors,
  formatter: function (x) { return x + "%"}
});



// ---------------------------------
// 4. World map
// ---------------------------------
$(function() {
	$('.map-world-markers').vectorMap({
		map: 'world_mill_en',
		backgroundColor: 'transparent',
        selectedColor: '#B1C1C9',
		scaleColors: ['#B1C1C9', '#B1C1C9'],
		normalizeFunction: 'polynomial',
		regionStyle: {
			initial: {
				fill: '#B1C1C9'
			}
		},
		hoverOpacity: 0.8,
		hoverColor: false,
		markerStyle: {
			initial: {
				r: 8,
				'fill': '#FFFFFF',
				'fill-opacity': 1,
				'stroke': '#4F5467',
				'stroke-width' : 1.5,
				'stroke-opacity': 0.6
			},
			hover: {
				'stroke': '#4F5467',
                'fill': '#4F5467',
				'fill-opacity': 1,
				'stroke-width': 2
			}
		},
		focusOn: {
			x: 0.5,
			y: 0.4,
			scale: 1
		},
		markers: [
			{latLng:[40.4637,3.7492],name:"Madrid – Spain"},
            {latLng:[55.7558,37.6173],name:"Moscow – Russia"},
            {latLng:[37.9838,23.7275],name:"Athens – Greece"},
            {latLng:[45.4215,-75.6972],name:"Ottawa – Canada"},
            {latLng:[47.7511,-120.7401],name:"Washington – USA"},
            {latLng:[52.5200,13.4050],name:"Berlin – Germany"},
            {latLng:[35.6895,139.6917],name:"Tokyo – Japan"},
            {latLng:[48.8566,2.3522],name:"Paris – France"},
            {latLng:[28.7041,77.1025],name:"Delhi - India"},
            {latLng:[51.5074,0.1278],name:" London – England"}
		]
	});
});
