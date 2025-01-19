var Page = {

    Ready: function () {
        this.initChart();
        this.initRecentTable();
    },

    initChart: function () {
        var lineChart = document.getElementById("data-ingrest-chart").getContext("2d");
        var myLineChart = new Chart(lineChart, {
            type: "line",
            data: {
                labels: [
                    "Jan",
                    "Feb",
                    "Mar",
                    "Apr",
                    "May",
                    "Jun",
                    "Jul",
                    "Aug",
                    "Sep",
                    "Oct",
                    "Nov",
                    "Dec",
                ],
                datasets: [
                    {
                        label: "Mb Per Month",
                        borderColor: "#1d7af3",
                        pointBorderColor: "#FFF",
                        pointBackgroundColor: "#1d7af3",
                        pointBorderWidth: 2,
                        pointHoverRadius: 4,
                        pointHoverBorderWidth: 1,
                        pointRadius: 4,
                        backgroundColor: "transparent",
                        fill: true,
                        borderWidth: 2,
                        data: [
                            43, 25, 67, 32, 329, 453, 380, 434, 568, 610, 700, 900,
                        ],
                    },
                ],
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                legend: {
                    position: "bottom",
                    labels: {
                        padding: 10,
                        fontColor: "#1d7af3",
                    },
                },
                tooltips: {
                    bodySpacing: 4,
                    mode: "nearest",
                    intersect: 0,
                    position: "nearest",
                    xPadding: 10,
                    yPadding: 10,
                    caretPadding: 10,
                },
                layout: {
                    padding: { left: 15, right: 15, top: 15, bottom: 15 },
                },
            },
        });
    },

    initRecentTable: function () {
        var data = [
            {
                'fileName': 'ActivityData.csv',
                'uploadDate': '12/03/2021',
                'fileSize': '80 Gb',
                'uploadType': 'API Intergration'
            },
            {
                'fileName': '6minuteWalk.csv',
                'uploadDate': '12/04/2021',
                'fileSize': '340 Mb',
                'uploadType': 'Manual'
            },
            {
                'fileName': 'testdata.csv',
                'uploadDate': '12/18/2021',
                'fileSize': '57 Mb',
                'uploadType': 'Web hook'
            },
        ]
        var table = $('#recent-files-table');
        table.bootstrapTable({ data: data });
    },

}