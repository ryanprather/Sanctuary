var Page = {

    Ready: function () {
        
        this.initSubjectTable();

        $("#btn-patient-add").click(function () {
            Page.buttonAddPopulate();
        });
        $("#btn-patient-edit").click(function () {
            Page.buttonEditPopulate();
        });
        $("#btn-patient-remove").click(function () {
            Page.buttonRemovePopulate();
        });
        $("#btn-patient-import").click(function () {
            Page.buttonImportPopulate();
        });
        $("#btn-patient-export").click(function () {
            Page.buttonExportPopulate();
        });
    },

    initSubjectTable: function () {
        var data = [
            {
                'id': '94189b8a-7854-440a-80d6-87b782f35bb7',
                'patientId': '5452254',
                'site': 'France',
                'isActive': 'True',
                'createdDate': '2024-06-24',
                'dataRangeStart': '2024/07/15', 
                'dataRangeEnd': '2024/08/15'
            },
            {
                'id': '94189b8a-7854-440a-80d6-87b782f35bb7',
                'patientId': '5459999',
                'site': 'London',
                'isActive': 'True',
                'createdDate': '2024-08-24',
                'dataRangeStart': '2024/05/03',
                'dataRangeEnd': '2024/05/31'
            },
            {
                'id': '94189b8a-7854-440a-80d6-87b782f35bb7',
                'patientId': '5452254',
                'site': 'France',
                'isActive': 'True',
                'createdDate': '2024-06-24',
                'dataRangeStart': '2024/07/15',
                'dataRangeEnd': '2024/08/15'
            },
            {
                'id': '94189b8a-7854-440a-80d6-87b782f35bb7',
                'patientId': '5453599',
                'site': 'London',
                'isActive': 'True',
                'createdDate': '2024-08-24',
                'dataRangeStart': '2024/05/03',
                'dataRangeEnd': '2024/05/31'
            },
        ]
        var table = $('#subjects-table');
        table.bootstrapTable({ data: data });
    },

    buttonAddPopulate: function () {
        $.ajax({
            type: "GET",  // or "POST", "PUT", "DELETE", etc.
            dataType: 'html',
            url: "Patient/Add",
            success: function (data) {
                console.log("Data:", data);
                $('.modal-body').html(data);
                $('#patient-modal').modal('show');
            },
            error: function (xhr, status, error) {
                // Code to execute on error
                console.error("Error:", error);
            }
        });
    },

    detailsFormater: function (value, row) {
        return '<a href="Patient/Details/'+row.id+'" class="btn btn-info" role="button">Details</a>';
    },

    buttonEditPopulate: function () {
        alert("edit button click");
    },

    buttonRemovePopulate: function () {
        alert("remove button click");
    },

    buttonImportPopulate: function () {
        alert("import button click");
    },

    buttonExportPopulate: function () {
        alert("export button click");
    },

}