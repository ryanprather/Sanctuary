var Page = {

    Ready: function () {
        
        this.initSubjectTable();
        $("#btn-stats-add").click(function () {
            Page.buttonAddPopulate();
        });
    },

    initSubjectTable: function () {
        var table = $('#stats-jobs-table');
        table.bootstrapTable();
        table.bootstrapTable('showLoading'); 
        $.ajax({
            type: "GET",  // or "POST", "PUT", "DELETE", etc.
            dataType: 'json',
            url: "Statistics/GetPreviousJobs",
            success: function (data) {
                console.log("Data:", data);
                //table.bootstrapTable({ data: data });
                table.bootstrapTable('load', data)
                table.bootstrapTable('hideLoading');
            },
            error: function (xhr, status, error) {
                // Code to execute on error
                console.error("Error:", error);
            }
        });
    },

    detailsFormater: function (value, row) {
        return '<a href="Statistics/Results/' + row.id + '" class="btn btn-info" role="button">Results</a>';
    },

    patientCount: function (value, row) {
        return '<span class="badge bg-secondary">' + row.options.patients.length + '</span>';
    },

    fileCount: function (value, row) {
        return '<span class="badge bg-secondary">' + row.options.dataFiles.length + '</span>';
    },

    endpointCount: function (value, row) {
        return '<span class="badge bg-secondary">' + row.options.endpoints.length +'</span>';
    },

    buttonAddPopulate: function () {
        $.ajax({
            type: "POST",  // or "POST", "PUT", "DELETE", etc.
            dataType: 'json',
            url: "Statistics/CreateStatisticsJob",
            success: function (data) {
                console.log("Data:", data);
            },
            error: function (xhr, status, error) {
                // Code to execute on error
                console.error("Error:", error);
            }
        });
    },
}