/*
* jQuery tableGroup plugin
* Version: 0.1.0
*
* Copyright (c) 2007 Roman Weich
* http://p.sohei.org
*
* Dual licensed under the MIT and GPL licenses 
* (This means that you can choose the license that best suits your project, and use it accordingly):
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*
* Changelog: 
* v 0.1.0 - 2007-05-20
*/

(function($) {
    var defaults = {
        groupColumn: 1,
        useNumChars: 0,
        groupClass: ''
    };

    /**
    * Returns the cell element which has the passed column index value.
    * @param {element} table	The table element.
    * @param {array} cells		The cells to loop through.
    * @param {integer} col	The column index to look for.
    */
    var getCell = function(table, cells, col) {
        for (var i = 0; i < cells.length; i++) {
            if (cells[i].realIndex === undefined) //the test is here, because rows/cells could get added after the first run
            {
                fixCellIndexes(table);
            }
            if (cells[i].realIndex == col) {
                return cells[i];
            }
        }
        return null;
    };

    /**
    * Calculates the actual cellIndex value of all cells in the table and stores it in the realCell property of each cell.
    * Thats done because the cellIndex value isn't correct when colspans or rowspans are used.
    * Originally created by Matt Kruse for his table library - Big Thanks! (see http://www.javascripttoolbox.com/)
    * @param {element} table	The table element.
    */
    var fixCellIndexes = function(table) {
        var rows = table.rows;
        var len = rows.length;
        var matrix = [];
        var cols = 0;
        for (var i = 0; i < len; i++) {
            var cells = rows[i].cells;
            var clen = cells.length;
            cols = Math.max(clen, cols);
            for (var j = 0; j < clen; j++) {
                var c = cells[j];
                var rowSpan = c.rowSpan || 1;
                var colSpan = c.colSpan || 1;
                var firstAvailCol = -1;
                if (!matrix[i]) {
                    matrix[i] = [];
                }
                var m = matrix[i];
                // Find first available column in the first row
                while (m[++firstAvailCol]) { }
                c.realIndex = firstAvailCol;
                for (var k = i; k < i + rowSpan; k++) {
                    if (!matrix[k]) {
                        matrix[k] = [];
                    }
                    var matrixrow = matrix[k];
                    for (var l = firstAvailCol; l < firstAvailCol + colSpan; l++) {
                        matrixrow[l] = 1;
                    }
                }
            }
        }
        table.numCols = cols;
    };

    /**
    * Simple grouping of rows in a table.
    *
    * @param {map} options			An object for optional settings (options described below).
    *
    * @option {integer} groupColumn		The column to group after.
    *							Index starting at 1!
    *							Default value: 1
    * @option {string} groupClass		A CSS class that is set on the inserted grouping row.
    *							Default value: ''
    * @option {integer} useNumChars		Defines the number of characters that are used to group the rows together. Set it to 0 to use all characters.
    *							Default value: 0
    *
    * @example $('#table').tableGroup();
    * @desc Group the rows using the default settings.
    *
    * @example $('#table').tableGroup({groupColumn: 3, groupClass: 'mygroups', useNumChars: 1});
    * @desc Group the rows after the first character in the third column. Set the CSS class "mygroups" to all inserted rows.
    *
    * @type jQuery
    *
    * @name tableGroup
    * @cat Plugins/tableGroup
    * @author Roman Weich (http://p.sohei.org)
    */

    $.fn.tableTotalizar = function(options) {

        return this.each(function() {
            var tboI, rowI, gC, body, row, $row, lastIH, c, cStr, match, tc, NameBody;
            var footer, tFoot;
            var rowHeader, tHead;
            if (!this.tBodies || !this.tBodies.length) {
                return;
            }

            var cellTotalizadores = [];
            var contCells = 0;
            var tHead = this.tHead;
            var rowHeader = tHead.rows[0];
            for (var i = 0; i < rowHeader.cells.length - 1; i++) {
                if (rowHeader.cells[i].getAttribute("Totalizar") == "True") {
                    cellTotalizadores[contCells] = i;
                    contCells++;
                }
            }


            if (this.tBodies[0].rows.length > 0) {
                var rowModelo = this.tBodies[0].rows[0];
                var trFooter = $("<tr>");
                for (var j = 0; j < rowModelo.cells.length; j++) {

                    var cellTotalizador = jQuery.inArray(j, cellTotalizadores);
                    if (cellTotalizador >= 0) {
                        var total = 0;
                        for (var rowGroupIndex = 0; rowGroupIndex < this.tBodies[0].rows.length; rowGroupIndex++) {
                            total += parseFloat(this.tBodies[0].rows[rowGroupIndex].cells[cellTotalizadores[cellTotalizador]].innerText);
                        }

                        $(rowModelo.cells[j].outerHTML.replace(rowModelo.cells[j].innerHTML, ''))
                                    .append(total.toFixed(2))
                                    .addClass("tdFooter")
                                    .appendTo(trFooter);
                    }
                    else {
                        $(rowModelo.cells[j].outerHTML.replace(rowModelo.cells[j].innerHTML, ''))
                                    .append('&nbsp;')
                                    .addClass("tdFooter")
                                    .appendTo(trFooter);
                    }
                }

                this.tBodies[0].appendChild(trFooter[0]);

            }
        });
    }

    $.fn.tableGroup = function(options) {
        var settings = $.extend({}, defaults, options);

        return this.each(function() {
            var tboI, rowI, gC, body, row, $row, lastIH, c, cStr, match, tc, NameBody;
            var footer, tFoot;
            var rowHeader, tHead;
            var NameGroupColumn;
            gC = settings.groupColumn - 1;
            lastIH = '';
            if (!this.tBodies || !this.tBodies.length) {
                return;
            }
            cStr = 'tGroup ' + settings.groupClass;
            NameBody = 0;

            //Genero los rows de cada uno de los grupos existentes
            for (tboI = 0; tboI < this.tBodies.length; tboI++) {
                body = this.tBodies[tboI];
                //                tFoot = this.tFoot;
                //                footer = tFoot.innerHTML;

                var cellTotalizadores = [];
                var contCells = 0;
                tHead = this.tHead;
                rowHeader = tHead.rows[tHead.rows.length - 1];
                for (var i = 0; i <= rowHeader.cells.length - 1; i++) {
                    if (rowHeader.cells[i].getAttribute("Totalizar") == "True") {
                        cellTotalizadores[contCells] = i;
                        contCells++;
                    }

                    if (settings.groupColumn - 1 == i) {
                        rowHeader.cells[i].style.display = "none";
                        NameGroupColumn = rowHeader.cells[i].innerText;
                    }
                }



                var rowindice = 0;
                var rowsGroup = [];
                var rowGroup = null;
                var nombreTable = this.id;

                for (rowI = 0; rowI < body.rows.length; rowI++) {
                    row = body.rows[rowI];
                    c = getCell(this, row.cells, gC);
                    if (c) {
                        if (settings.useNumChars == 0) {
                            match = c.innerHTML;
                            row.setAttribute("idGrupo", "grupo" + NameBody);

                            for (var j = 0; j < row.cells.length; j++) {
                                $(row.cells[j]).removeClass("tdSimple")
                                $(row.cells[j]).removeClass("tdSimpleAlt")
                                $(row.cells[j]).addClass("tdSimple")
                            }
                            
                        }
                        else {
                            tc = c.textContent || c.innerText;
                            match = tc.substr(0, settings.useNumChars);
                        }
                        if (match !== lastIH) {

                            var tbodyName = "Tbody_" + NameBody;
                            var tablegroup = '<table cellpadding="0" cellspacing="0" width="100%" border="0"><tr><td onclick="return toggleGroup(' + nombreTable + ',grupo' + (NameBody + 1) + ',this);" class="rgGroupItemImageAbierto">&nbsp;</td><td class="rgGroupItem"><span>' + NameGroupColumn + ': ' + match + '</span></td></tr></table>';
                            //insert grouping row
                            $row = $('<tr class="group" id="grupo' + (NameBody + 1) + '"><td  align="left" colspan="' + this.numCols + '">' + tablegroup + '</td></tr>');
                            $row.find('td')[0].realIndex = 0;
                            $(row).before($row);
                            lastIH = match;
                            NameBody++;

                        }

                        c.style.display = "none";
                    }
                }
            }



            $('#' + nombreTable + ' .group').each(function() {


                var idgrupo = this.id;
                var rowsGroup = $('#' + nombreTable + " tbody tr[idGrupo='" + idgrupo + "']").toArray();
                var rowModelo = rowsGroup[rowsGroup.length - 1];

                if (cellTotalizadores.length > 0) {
                    rowTbl = CreateRowTotalizador(rowsGroup, rowModelo);
                    $(rowModelo).after(rowTbl);
                }

                

            });



            function CreateRowTotalizador(rowsGroup, rowModelo) {

                var trFooter = $("<tr>");
                for (var j = 0; j < rowModelo.cells.length; j++) {

                    var cellTotalizador = jQuery.inArray(j, cellTotalizadores);
                    if (cellTotalizador >= 0) {
                        var total = 0;
                        for (var rowGroupIndex = 0; rowGroupIndex < rowsGroup.length; rowGroupIndex++) {
                            total += parseFloat(rowsGroup[rowGroupIndex].cells[cellTotalizadores[cellTotalizador]].innerText);
                        }

                        $(rowModelo.cells[j].outerHTML.replace(rowModelo.cells[j].innerHTML, ''))
                                    .append(total.toFixed(2))
                                    .removeClass("tdSimple")
                                    .removeClass("tdSimpleAlt")
                                    .addClass("tdTotalizador")
                                    .appendTo(trFooter);
                    }
                    else {
                        $(rowModelo.cells[j].outerHTML.replace(rowModelo.cells[j].innerHTML, ''))
                                    .append('&nbsp;')
                                    .removeClass("tdSimple")
                                    .removeClass("tdSimpleAlt")
                                    .addClass("tdTotalizadorVacio")
                                    .appendTo(trFooter);
                    }
                }

                return trFooter[0];
                //rowTbl[0].all[2].appendChild(trFooter[0]);
            }
        });





    };

    /**
    * Removes the grouping rows from the table.
    *
    * @type jQuery
    *
    * @name tableUnGroup
    * @cat Plugins/tableGroup
    * @author Roman Weich (http://p.sohei.org)
    */
    $.fn.tableUnGroup = function() {


        this.find('.group').each(function() {

            $(this).remove();

        });


        this.find('.tdTotalizador').each(function() {

            $(this).remove();

        });


        this.find('.tdTotalizadorVacio').each(function() {

            $(this).remove();

        });
    };
})(jQuery);


function toggleGroup(Table, Group, img) {

    var nameTable = Table.id;
    var nameGroup = Group.id;
    var rowsGroup = $('#' + nameTable + " tbody tr[idGrupo='" + nameGroup + "']").toArray();
    for (var i = 0; i < rowsGroup.length; i++) {
        if ($(rowsGroup[i]).css("display") == "" || $(rowsGroup[i]).css("display") == "block")
            $(rowsGroup[i]).css("display", "none");
        else
            $(rowsGroup[i]).css("display", "block");
    }

    if ($(img).attr("class") == "rgGroupItemImageAbierto") {
        $(img).removeClass("rgGroupItemImageAbierto");
        $(img).addClass("rgGroupItemImageCerrado");
    }
    else {
        $(img).removeClass("rgGroupItemImageCerrado");
        $(img).addClass("rgGroupItemImageAbierto");
    }


}