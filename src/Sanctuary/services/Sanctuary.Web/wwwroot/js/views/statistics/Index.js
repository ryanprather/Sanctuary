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