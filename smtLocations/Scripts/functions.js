function getVirtDir() {
    var Path = location.host;
    var VirtualDirectory;
    if (Path.indexOf("localhost") >= 0 && Path.indexOf(":") >= 0) {
        VirtualDirectory = "";

    }
    else {
        var pathname = window.location.pathname;
        var VirtualDir = pathname.split('/');
        VirtualDirectory = VirtualDir[1];
        VirtualDirectory = '/' + VirtualDirectory;
    }
    return VirtualDirectory;
}
function blockV2(text) {
    $("<table id='overlay' style='margin-top:-40px;'><tbody><tr><td>"
        + "<div class='cssload-wrap'>" +
        "<div class= 'translate' >" +
        "<div class='scale'></div>" +
        "    </div >" +
        "</div >" +
        "<div class='cssload-wrap'>" +
        "    <div class='translate'>" +
        "        <div class='scale'></div>" +
        "    </div>" +
        "</div>" +
        "<div class='cssload-wrap'>" +
        "    <div class='translate'>" +
        "        <div class='scale'></div>" +
        "    </div>" +
        "</div>" +
        "<div class='cssload-wrap'>" +
        "    <div class='translate'>" +
        "        <div class='scale'></div>" +
        "    </div>" +
        "</div>" +
        "<div class='cssload-wrap'>" +
        "    <div class='translate'>" +
        "        <div class='scale'></div>" +
        "    </div>" +
        "</div>" +
        "<div class='cssload-wrap'>" +
        "    <div class='translate'>" +
        "        <div class='scale'></div>" +
        "    </div>" +
        "</div>" +
        "<div class='cssload-wrap'>" +
        "    <div class='translate'>" +
        "        <div class='scale'></div>" +
        "    </div>" +
        "</div>" +
        "<div class='cssload-wrap'>" +
        "    <div class='translate'>" +
        "        <div class='scale'></div>" +
        "    </div>" +
        "</div>" +
        "<div class='cssload-wrap'>" +
        "    <div class='translate'>" +
        "        <div class='scale'></div>" +
        "    </div>" +
        "</div>" +
        "<br/><br/><br/><div id='txtOver'>" + text + "</div>" +
        "</td></tr></tbody></table>").css({
            "position": "fixed",
            "top": "0px",
            "left": "0px",
            "width": "100%",
            "height": "100%",
            "background-color": "rgba(0,0,0,.5)",
            "z-index": "10000",
            "vertical-align": "middle",
            "text-align": "center",
            "color": "#fff",
            "font-size": "40px",
            "font-weight": "bold",
            "cursor": "wait"
        }).appendTo("body");
}
function removeOverlay() {
    $("#overlay").remove();
}
function format(value) {
    return '<div>' + value + '</div>';
}
function getLocations() {
    blockV2("Obteniendo Locaciones...");
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/getLocations.ashx",
        success: function (data) {
            removeOverlay();
            var groupColumn = 0;
            var collapsedGroups = {};
            var collapsedGroupsM = {};
            r = jQuery.parseJSON(data);
            if (r.result === "true") {
                $("#minLocations").html(r.htmlMin);
                $("#locations").html(r.html);
                var tableS = $('#tableLoc').DataTable({
                    "paging": false,
                    "searching": true,
                    "lengthChange": false,
                    "pageLength": 5,
                    "info": false,
                    "autoWidth": true,
                    "columnDefs": [
                        {
                            "targets": [0],
                            "visible": false
                        }
                    ],
                    rowGroup: {
                        dataSrc: 0,
                        startRender: function (rows, group) {
                            var collapsed = !collapsedGroups[group];
                            //alert(collapsed);
                            rows.nodes().each(function (r) {
                                r.style.display = collapsed ? '' : 'none';
                            });

                            var Required = rows
                                .data()
                                .pluck(3)
                                .sum();
                            Required = $.fn.dataTable.render.number('.', ',', 0).display(Required);

                            var Current = rows
                                .data()
                                .pluck(4)
                                .sum();
                            Current = $.fn.dataTable.render.number('.', ',', 0).display(Current);
                            return $('<tr/>')
                                .append('<td colspan="8"><a href=\"#\">+ ' + group + ' [' + rows.count() + ']  ******   Required: ' + Required + '    -   Current: ' + Current + ' </a></td>')
                                .attr('data-name', group)
                                .toggleClass('collapsed', collapsed);
                        }
                    },
                    "ordering": false
                    //'language': { 'url': getVirtDir() + '/Scripts/Spanish.json' }
                });
                var tableM = $('#tableLocMin').DataTable({
                    "paging": false,
                    "searching": true,
                    "lengthChange": false,
                    "pageLength": 5,
                    "info": false,
                    "autoWidth": true,
                    "columnDefs": [
                        {
                            "targets": [0],
                            "visible": false
                        }
                    ],
                    rowGroup: {
                        dataSrc: 0,
                        startRender: function (rows, group) {
                            var collapsedM = !collapsedGroupsM[group];
                            //alert(collapsed);
                            rows.nodes().each(function (r) {
                                r.style.display = collapsedM ? '' : 'none';
                            });

                            var Required = rows
                                .data()
                                .pluck(3)
                                .sum();
                            Required = $.fn.dataTable.render.number('.', ',', 0).display(Required);

                            var Current = rows
                                .data()
                                .pluck(4)
                                .sum();
                            Current = $.fn.dataTable.render.number('.', ',', 0).display(Current);
                            return $('<tr/>')
                                .append('<td colspan="8"><a href=\"#\">+ ' + group + ' [' + rows.count() + ']  ******   Required: ' + Required + '    -   Current: ' + Current + ' </a></td>')
                                .attr('data-name', group)
                                .toggleClass('collapsed', collapsedM);
                        }
                    },
                    "ordering": false
                    //'language': { 'url': getVirtDir() + '/Scripts/Spanish.json' }
                });
                $('#tableLoc tbody').on('click', 'tr.group-start', function () {
                    var name = $(this).data('name');
                    collapsedGroups[name] = !collapsedGroups[name];
                    tableS.draw(false);
                }); 
                $('#tableLoc tbody').on('click', 'td.details-control', function () {
                    var tr = $(this).closest('tr');
                    var row = tableS.row(tr);

                    if (row.child.isShown()) {
                        // This row is already open - close it
                        row.child.hide();
                        tr.removeClass('shown');
                    } else {
                        // Open this row
                        row.child(format(tr.data('child-value'))).show();
                        tr.addClass('shown');
                    }
                });

                $('#tableLocMin tbody').on('click', 'tr.group-start', function () {
                    var nameM = $(this).data('name');
                    collapsedGroupsM[nameM] = !collapsedGroupsM[nameM];
                    tableM.draw(false);
                });
                $('#tableLocMin tbody').on('click', 'td.details-control', function () {
                    var trM = $(this).closest('tr');
                    var rowM = tableM.row(trM);

                    if (rowM.child.isShown()) {
                        // This row is already open - close it
                        rowM.child.hide();
                        trM.removeClass('shown');
                    } else {
                        // Open this row
                        rowM.child(format(trM.data('child-value'))).show();
                        trM.addClass('shown');
                    }
                });
            }


            return false;
        },
        error: function () { }
    });
}

function updateLoc() {
    updateLocations();
    setInterval(function () {
        updateLocations();
    }, 300000);
}

function updateLocations() {
    blockV2("Actualizando Locaciones...");
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/updateContainers.ashx",
        success: function (data) {
            removeOverlay();
            r = jQuery.parseJSON(data);
            if (r.result === "true") {
                getLocations();
                getAllReport();
            }
            return false;
        },
        error: function () { }
    });
}
function getEvenReport() {
    blockV2("");
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/getEvenReport.ashx",
        success: function (data) {
            removeOverlay();
            r = jQuery.parseJSON(data);
            if (r.result === "true") {
                $("#btnPrint").html("<a class='btn btn-primary btn-sm' href='" + getVirtDir() + "/Reports/Even/" + r.html + "'>DOWNLOAD</a>");
            }
            return false;
        },
        error: function () { }
    });
}
function getAllReport() {
    blockV2("");
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/getAllReport.ashx",
        success: function (data) {
            removeOverlay();
            r = jQuery.parseJSON(data);
            if (r.result === "true") {
                $("#btnPrint").html("<a class='btn btn-primary btn-sm' href='" +getVirtDir() + "/Reports/All/" + r.html + "'>DOWNLOAD</a>");
            }
            return false;
        },
        error: function () { }
    });
}
function getOddReport() {
    blockV2("");
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/getOddReport.ashx",
        success: function (data) {
            removeOverlay();
            r = jQuery.parseJSON(data);
            if (r.result === "true") {
                $("#btnPrint").html("<a class='btn btn-primary btn-sm' href='" + getVirtDir() + "/Reports/Odd/" + r.html + "'>DOWNLOAD</a>");
            }
            return false;
        },
        error: function () { }
    });
}
function getMinReport() {
    blockV2("");
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/getMinimumReport.ashx",
        success: function (data) {
            removeOverlay();
            r = jQuery.parseJSON(data);
            if (r.result === "true") {
                $("#btnPrint").html("<a class='btn btn-primary btn-sm' href='" + getVirtDir() + "/Reports/" + r.html + "'>DOWNLOAD</a>");
            }
            return false;
        },
        error: function () { }
    });
}
function sendAutoMail() {
    sendMail();
    setInterval(function () {
        sendMail();
    }, 7200000);
}
function sendMail() {
    //blockV2("");
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/sendMail.ashx",
        success: function (data) {
            //removeOverlay();
            r = jQuery.parseJSON(data);
            if (r.result === "true") {
                alert(r.html);
            }
            if (r.result === "false") {
                alert(r.html);
            }
            return false;
        },
        error: function () { }
    });
}