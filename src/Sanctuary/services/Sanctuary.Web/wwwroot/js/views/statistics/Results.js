var Page = {

    Ready: function () {
        this.initCharts();    
    },

    initCharts: function () {
        $(".result-chart").each(function (index, element) {
            var chartId = $(this).data('chart-id');
            console.log("URL:", "../GetChartById/" + chartId);
            $.ajax({
                type: "GET",  // or "POST", "PUT", "DELETE", etc.
                dataType: 'json',
                url: "../GetChartById/" + chartId,
                success: function (data) {
                    console.log("Data:", data);
                    var domChart = $(this)[0];
                    var context = element.getContext("2d");
                    var myLineChart = new Chart(context, {
                        type: data.chartType,
                        data: {
                            labels: data.labels,
                            datasets: [
                                {
                                    label: data.datasetLabel,
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
                                    data: data.data,
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
                error: function (xhr, status, error) {
                    // Code to execute on error
                    console.error("Error:", error);
                }
            });
        });
    },
}