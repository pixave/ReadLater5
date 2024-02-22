// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    RefreshCategories();

    $(".btnSaveCategory").bind("click", function (e) {

        let catswitch = $(this).closest(".modal-content").find(".catswitch").val();
        switch (catswitch) {
            case "C":
                jQuery.ajax({
                    url: "/api/Category",
                    type: "POST",
                    data: JSON.stringify({ UserID: _UserID, Name: $("#CategoryName").val() }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function () {
                        RefreshCategories();
                    }
                });
                break;
            case "U":
                jQuery.ajax({
                    url: "/api/Category?ID=" + $("#hdnCatID").val(),
                    type: "PUT",
                    data: JSON.stringify({ ID: parseInt($("#hdnCatID").val()), UserID: _UserID, Name: $("#CategoryName").val() }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function () {
                        RefreshCategories();
                    }
                });
                break;
            case "D":
                jQuery.ajax({
                    url: "/api/Category?id=" + $("#hdnDeleteCatID").val(),
                    type: "DELETE",
                    data: JSON.stringify({ id: parseInt($("#hdnDeleteCatID").val()), UserID: _UserID }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function () {
                        RefreshCategories();
                    }
                });
                break;
        }
       
        e.preventDefault();
    });



    $('#categoryModal').on('shown.bs.modal', function (e) {
        $("#CategoryName").focus();
    })

    function RefreshCategories() {
        let _uid = "";
        if (_UserID != undefined) {
            _uid = _UserID;
        } else {

        }
        if (_uid != "" && _uid != undefined) {
            $.get("/api/Category?UserID=" + _uid, function (data, status) {
                console.log("Data: " + data + "\nStatus: " + status);
                $("#tblCategories").html("");//clear the table
                // draw the table
                let html = "<tr><th>Name</th></tr>";
                data.forEach(function (item, index, arr) {
                    html += "<tr>";
                    html += '<td class="catname">' + item.name + '</td>';
                    html += "<td>";
                    html += '<a href="#" class="EditCat" data-id="' + item.id + '">Edit</a>&nbsp;|&nbsp;';
                    html += '<a href="#" class="DetailsCat" data-id="' + item.id + '">Details</a>&nbsp;|&nbsp;';
                    html += '<a href="#" class="DeleteCat" data-id="' + item.id + '">Delete</a>';
                    html += "</td>";
                    html += "</tr>";
                });
                $("#tblCategories").html(html);//redraw the table
                if ($("#categoryModal").is(":visible")) {
                    $("#categoryModal").find("button[data-dismiss='modal'").first().click();
                }
                if ($("#categoryDeleteModal").is(":visible")) {
                    $("#categoryDeleteModal").find("button[data-dismiss='modal'").first().click();
                }

                $(".EditCat").bind("click", function (e) {

                    let id = $(this).attr("data-id");
                    let itsname = $(this).closest("tr").find(".catname").html();
                    $("#hdnCatModalSwitch").val("U");   //Update Mode
                    $("#CategoryName").val(itsname);
                    $("#hdnCatID").val(id);
                    $("#btnSaveCategory").show();
                    $('#categoryModal').modal('show');

                    e.preventDefault();
                });

                $(".DetailsCat").bind("click", function (e) {

                    let id = $(this).attr("data-id");
                    let itsname = $(this).closest("tr").find(".catname").html();
                    $("#hdnCatModalSwitch").val("R");   //Read Mode
                    $("#CategoryName").val(itsname);
                    $("#hdnCatID").val(id);
                    $("#btnSaveCategory").hide();
                    $('#categoryModal').modal('show');

                    e.preventDefault();
                });

                $(".DeleteCat").bind("click", function (e) {

                    let id = $(this).attr("data-id");
                    let itsname = $(this).closest("tr").find(".catname").html();
                    $("#hdnDeleteCatModalSwitch").val("D");   //Read Mode
                    $("#DeleteCategoryName").val(itsname);
                    $("#hdnDeleteCatID").val(id);
                    $("#btnDeleteCategory").show();
                    $('#categoryDeleteModal').modal('show');

                    e.preventDefault();
                });
            });

            $("#btnCreateNew").bind("click", function (e) {
                $("#hdnCatModalSwitch").val("C");   //Create Mode
                $("#CategoryName").val("");
                $("#hdnCatID").val("");
                $("#btnSaveCategory").show();
                $('#categoryModal').modal('show');


                e.preventDefault();
            });
        } else {
            $('#notLoggedInModal').modal('show');
        }
      

    }

    



});