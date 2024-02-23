// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    if ($("#tblCategories").length > 0) {
        RefreshCategories();
    }

    if ($("#tblBookmarks").length > 0) {
        RefreshBookmarks();
    }

    if ($("#divDashboard").length > 0) {
        RefreshDashboard();
    }


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



    $(".btnSaveBookMark").bind("click", function (e) {

        let bmswitch = $(this).closest(".modal-content").find(".bmswitch").val();
        let isValid = true;
        if ($("#BookMarkURL").val() == "" && bmswitch != "D") {
            isValid = false;
            $("#BookMarkURL").closest(".form-group").find(".errorfeedback").slideDown();
        } else {
            $("#BookMarkURL").closest(".form-group").find(".errorfeedback").slideUp();
        }
        if ($("#ddlBMCategory").val() == "" && bmswitch != "D") {
            isValid = false;
            $("#ddlBMCategory").closest(".form-group").find(".errorfeedback").slideDown();
        } else {
            $("#ddlBMCategory").closest(".form-group").find(".errorfeedback").slideUp();
        }
        if (isValid) {
            
            let catid = $("#ddlBMCategory").val();

            if ((bmswitch == "C" || bmswitch == "U") && catid == "xxx") {
                // get a new category id
                jQuery.ajax({
                    url: "/api/Category",
                    type: "POST",
                    data: JSON.stringify({ UserID: _UserID, Name: $("#NewCategoryName").val() }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (d) {
                        catid = d.id;
                        switch (bmswitch) {
                            case "C":
                                CreateBookmark(catid);
                            case "U":
                                UpdateBookmark(catid);
                                break;
                        }
                        RefreshBookmarks();
                    }
                });
            } else {
                switch (bmswitch) {
                    case "C":
                        CreateBookmark(catid);
                        break;
                    case "U":
                        UpdateBookmark(catid);
                        break;
                    case "D":
                        jQuery.ajax({
                            url: "/api/Bookmark?id=" + $("#hdnBMID").val(),
                            type: "DELETE",
                            data: JSON.stringify({ id: parseInt($("#hdnBMID").val()), UserID: _UserID }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function () {
                                RefreshBookmarks();
                            }
                        });
                        break;
                }

            }

        }
       
        e.preventDefault();
    });

    function UpdateBookmark(catid) {
        jQuery.ajax({
            url: "/api/Bookmark?ID=" + $("#hdnBMID").val(),
            type: "PUT",
            data: JSON.stringify({ ID: parseInt($("#hdnBMID").val()), UserID: _UserID, URL: $("#BookMarkURL").val(), ShortDescription: $("#BookMarkShortDesc").val(), CategoryId: catid }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                RefreshBookmarks();
            }
        });

    }
    function CreateBookmark(catid) {
        jQuery.ajax({
            url: "/api/Bookmark",
            type: "POST",
            data: JSON.stringify({ UserID: _UserID, URL: $("#BookMarkURL").val(), ShortDescription: $("#BookMarkShortDesc").val(), CategoryId: catid }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                RefreshBookmarks();
            }
        });
    }

    $('#categoryModal').on('shown.bs.modal', function (e) {
        $("#CategoryName").focus();
    })





});



function RefreshCategories() {
    let _uid = "";
    if (_UserID != undefined) {
        _uid = _UserID;
    } else {

    }
    if (_uid != "" && _uid != undefined) {
        $.get("/api/Category?UserID=" + _uid, function (data, status) {
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

        $("#btnCreateNewCat").bind("click", function (e) {
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

function BuildCategoryDDL(_uid) {

    $.get("/api/Category?UserID=" + _uid, function (data, status) {
        $("#ddlBMCategory").html("");//clear the list
        let html = "";
        html += '<option data-id="" value="">Please select...</option>';
        data.forEach(function (item, index, arr) {
            html += '<option data-id="' + item.id + '" value="' + item.id + '"">' + item.name + '</option>';
        });
        html += '<option data-id="xxx" value="xxx">Add new...</option>';
        $("#ddlBMCategory").html(html);//redraw the control
        $("#ddlBMCategory").unbind("change");
        $("#ddlBMCategory").bind("change", function (e) {
            switch ($(this).val()) {
                case "xxx":
                    $("#fgNewCat").slideDown();
                    break;
                default:
                    $("#fgNewCat").slideUp();
                    break;
            }
        });
    });
}

function setupBMModal(title, blnReadonly, blnDelete) {
    if (!blnReadonly) {
        $("#btnSaveBookMark").show();
        if (!blnDelete) {
            $("#btnSaveBookMark").html("Save Changes");
        } else {
            $("#btnSaveBookMark").html("Delete");
        }
        
    } else {
        if (!blnDelete) {
            $("#btnSaveBookMark").hide();
        } else {
            $("#btnSaveBookMark").show();
        }
    }
    $("#bookMarkModal").find("input,select").attr("readonly", blnReadonly);
    $("#bookMarkModal").find("input,select").attr("disabled", blnReadonly);
    $("#bookMarkModal .errorfeedback ").hide();
    $("#fgNewCat").hide();
    $("#bookMarkModalLabel").html(title);
    $('#bookMarkModal').modal('show');
}

function RefreshBookmarks() {
    let _uid = "";
    if (_UserID != undefined) {
        _uid = _UserID;
    } else {

    }
    if (_uid != "" && _uid != undefined) {
        $.get("/api/Bookmark?UserID=" + _uid, function (data, status) {

            $("#tblBookmarks").html("");//clear the table
            // draw the table
            let html = "<tr><th>URL</th><th>Short Description</th><th>Category</th><th>Date Created</th></tr>";
            data.forEach(function (item, index, arr) {
                html += "<tr>";
                html += '<td class="bookmark_url"><a target="_blank" data-id="' + item.id + '" class="bookmarklink" href="' + item.url + '">' + item.url + '</a></td>';
                html += '<td class="bookmark_shortdescription">' + item.shortDescription + '</td>';
                html += '<td class="bookmark_category">' + item.category.name + '</td>';
                html += '<td class="bookmark_datecreated">' + item.createDate.toString() + '</td>';
                html += "<td>";
                html += '<a href="#" class="EditBookMark" data-id="' + item.id + '">Edit</a>&nbsp;|&nbsp;';
                html += '<a href="#" class="DetailsBookMark" data-id="' + item.id + '">Details</a>&nbsp;|&nbsp;';
                html += '<a href="#" class="DeleteBookMark" data-id="' + item.id + '">Delete</a>';
                html += "</td>";
                html += "</tr>";
            });
            $("#tblBookmarks").html(html);//redraw the table
            if ($("#bookMarkModal").is(":visible")) {
                $("#bookMarkModal").find("button[data-dismiss='modal'").first().click();
            }
            if ($("#bookMarkDeleteModal").is(":visible")) {
                $("#bookMarkDeleteModal").find("button[data-dismiss='modal'").first().click();
            }

            $(".bookmarklink").bind("click", function (e) {
                // log the click to the table
                let _BookmarkID = $(this).attr("data-id");
                jQuery.ajax({
                    url: "/api/BookmarkClicks",
                    type: "POST",
                    data: JSON.stringify({ BookmarkID: _BookmarkID }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function () {
                    }
                });

                
            });
            $(".EditBookMark").bind("click", function (e) {
                let id = $(this).attr("data-id");
                // get the latest record
                $.get("/api/Bookmark?UserID=" + _uid + "&id=" + id, function (data, status) {
                    if (data != undefined) {
                        if (data.length > 0) {
                            let itsname = data[0].url;
                            let shortdesc = data[0].shortDescription;
                            $("#hdnBMModalSwitch").val("U");   //Update Mode
                            $("#hdnBMID").val(id);
                            $("#ddlBMCategory").val(data[0].categoryId);
                            $("#BookMarkURL").val(itsname);
                            $("#BookMarkShortDesc").val(shortdesc);
                        }
                    }
                    //$("#bookMarkModalLabel").html("Update");
                    //$("#btnSaveBookMark").show();
                    //$("#fgNewCat").hide();
                    //$("#btnSaveBookMark").html("Save Changes");
                    //$("#bookMarkModal").find("input,select").attr("readonly", false);
                    //$("#bookMarkModal").find("input,select").attr("disabled", false);
                    //$('#bookMarkModal').modal('show');
                    setupBMModal("Update", false, false);
                    
                });

                e.preventDefault();
            });

            $(".DetailsBookMark").bind("click", function (e) {
                let id = $(this).attr("data-id");
                // get the latest record
                $.get("/api/Bookmark?UserID=" + _uid + "&id=" + id, function (data, status) {
                    if (data != undefined) {
                        if (data.length > 0) {
                            let itsname = data[0].url;
                            let shortdesc = data[0].shortDescription;
                            $("#hdnBMModalSwitch").val("U");   //Update Mode
                            $("#hdnBMID").val(id);
                            $("#ddlBMCategory").val(data[0].categoryId);
                            $("#BookMarkURL").val(itsname);
                            $("#BookMarkShortDesc").val(shortdesc);
                        }
                    }
                    //$("#bookMarkModalLabel").html("Retrieve");
                    //$("#btnSaveBookMark").hide();
                    //$("#fgNewCat").hide();
                    //$("#btnSaveBookMark").html("Save Changes");
                    //$("#bookMarkModal").find("input,select").attr("readonly", true);
                    //$("#bookMarkModal").find("input,select").attr("disabled", true);
                    //$('#bookMarkModal').modal('show');
                    setupBMModal("Retrieve", true, false);
                });
                //$('#bookMarkModal').modal('show');

                e.preventDefault();
            });

            $(".DeleteBookMark").bind("click", function (e) {
                
                let id = $(this).attr("data-id");
                let itsname = $(this).closest("tr").find(".bookmark_url").html();
                let shortdesc = $(this).closest("tr").find(".bookmark_shortdescription").html();
                
                $("#hdnBMID").val(id);
                $("#hdnBMModalSwitch").val("D");   //Update Mode
                $("#BookMarkURL").val(itsname);
                $("#BookMarkShortDesc").val(shortdesc);
                $("#btnSaveBookMark").show();
                $("#btnSaveBookMark").html("Delete");
                //$("#bookMarkModalLabel").html("Delete");
                //$("#fgNewCat").hide();
                //$("#bookMarkModal").find("input,select").attr("readonly", true);
                //$("#bookMarkModal").find("input,select").attr("disabled", true);
                //$('#bookMarkModal').modal('show');
                setupBMModal("Delete", true, true);

                e.preventDefault();
            });
        });
        BuildCategoryDDL(_uid);
        $("#btnCreateNewBM").bind("click", function (e) {
            
            $("#hdnBMModalSwitch").val("C");   //Create Mode
            $("#BookMarkURL").val("");
            $("#BookMarkShortDesc").val("");
            $("#hdnBMID").val("");

            $("#bookMarkModalLabel").html("Create");
            //$("#btnSaveBookMark").show();
            //$("#btnSaveBookMark").html("Save Changes");
            //$("#fgNewCat").hide();
            //$("#bookMarkModal").find("input,select").attr("readonly", false);
            //$("#bookMarkModal").find("input,select").attr("disabled", false);
            //$('#bookMarkModal').modal('show');
            setupBMModal("Create", false,false);
            

            e.preventDefault();
        });
    } else {
        $('#notLoggedInModalBM').modal('show');
    }


}

function RefreshDashboard() {
    let _uid = "";
    if (_UserID != undefined) {
        _uid = _UserID;
    } else {

    }
    if (_uid != "" && _uid != undefined) {
        $.get("/api/Dashboards?UserID=" + _uid, function (data, status) {

            $("#divDashboard").html("");//clear the table
            // draw the table
            let html = "";
            let i = 0;
            data.forEach(function (item, index, arr) {
                i += 1;
                html += '<div class="card">';
                html += '   <div class="card-header" id="heading' + i.toString() + '">';
                html += '       <h5 class="mb-0">';
                html += '           <button class="btn btn-link"  data-toggle="collapse" data-target="#collapse' + i.toString() + '" aria-expanded="true" aria-controls="collapse' + i.toString() + '">';
                html += '               <div class="dash_user_id">' + item.username + '&nbsp;Bookmarks:' + item.numbookmarks + '&nbsp;Clicks:' + item.numclicks + '</div>';
                html += '           </button>';
                html += '       </h5>';
                html += '   </div>';
                html += '   <div id="collapse' + i.toString() + '" class="collapse" aria-labelledby="heading' + i.toString() + '" data-parent="#divDashboard">';
                html += '       <div class="card-body">';
                
                item.bookmarks.forEach(function (bmitem, bmindex, bmarr) {
                    html +=  bmitem.url + '&nbsp;Clicks:' + bmitem.numclicks + '<br/>';
                });

                html += '       </div>';
                html += '   </div>';
                html += '</div>';
            });
           
            $("#divDashboard").html(html);
          


            
        });
    } else {
        $('#notLoggedInModal').modal('show');
    }


}


