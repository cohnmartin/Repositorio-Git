window.onload = loadIndex;

function loadIndex() { // load indexfile
    // most current browsers support document.implementation
    if (document.implementation && document.implementation.createDocument) {
        xmlDoc = document.implementation.createDocument("", "", null);
        xmlDoc.load("Legajos.xml");
    }
    // MSIE uses ActiveX
    else if (window.ActiveXObject) {
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.async = "true";
        xmlDoc.load("Legajos.xml");
    }
}
function showAlert() {



    if (event.srcElement.parentElement.getAttribute("IdLegajoSeleccionado") != null) {
        ClickLegajo(
                event.srcElement.parentElement.getAttribute("IdLegajoSeleccionado"),
                event.srcElement.parentElement.getAttribute("UltimoPeriodo")
                );
    }
    else
    {
        ClickLegajo(
            event.srcElement.getAttribute("IdLegajoSeleccionado"),
            event.srcElement.getAttribute("UltimoPeriodo")
            );
        }
}
function ActualizarXML()
{
    loadIndex();
}
function setStyle() {
    this.className = 'MouseOverStyle';
}
function resetStyle() {
    this.className = 'MouseOutStyle';
}
function LimpiarTarget(sender, eventArgs) {
    sender.set_targetControlID();
}

function searchIndex(searchText) { // search the index (duh!)
    if (!xmlDoc) {
        loadIndex();
    }
    // get the search term from a form field with id 'searchme'

    var searchterm = searchText.value; 
    var allitems = xmlDoc.getElementsByTagName("item");
    results = new Array;
    if (searchterm.length < 2) {
        var resultshere = document.getElementById("resultshere");
        resultshere.innerText = "";
        return true;
    } else {
        var resultshere = document.getElementById("resultshere");
        resultshere.innerText = "";

        for (var i = 0; i < allitems.length; i++) {
            // see if the XML entry matches the search term,
            // and (if so) store it in an array
            //var name = allitems[i].lastChild.nodeValue;
            var name = allitems[i].getAttributeNode("NroDocumento");
            var exp = new RegExp("^" + searchterm, "i");
            if (name != null && name.value.match(exp) != null) {
                results.push(allitems[i]);
            }
        }
        // send the results to another function that displays them to the user
        showResults(results, searchterm);
    }
}


// The following is just an example of how you
// could handle the search results
function showResults(results, searchterm) {

    if (results.length > 0) {
        // if there are any results, put them in a list inside the "resultshere" div
        var resultshere = document.getElementById("resultshere");
        var list = document.createElement("ol");
        resultshere.appendChild(list);
        for (var i = 0; i < results.length; i++) {
            var listitem = document.createElement("li");
            

            var item = document.createElement("span");
            item.id = "span1_" + results[i].text;
            item.onclick = showAlert;
            item.onmouseover = setStyle;
            item.onmouseout = resetStyle;

            var Nombre = results[i].text;
            var Contrato = results[i].getAttributeNode("contrato").value;
            var nroCompleto = results[i].getAttributeNode("NroDocumento").value;
            var UltimoPeriodo = results[i].getAttributeNode("UltimoPeriodo").value;
            
            var busqueda = "<b>" + searchterm.fontsize(3) + "</b>" + nroCompleto.substring(searchterm.length, nroCompleto.length) + ": ";
            item.innerHTML = busqueda + " Nombre: " + Nombre.fontcolor('Blue') + " Contrato: " + Contrato.fontcolor('Blue') + " último período: " + UltimoPeriodo.fontcolor('Blue');
            item.setAttribute("IdLegajoSeleccionado", results[i].getAttributeNode("IdLegajo").value);
            item.setAttribute("UltimoPeriodo", UltimoPeriodo);
            

            list.appendChild(listitem);
            listitem.appendChild(item);
        }
    } else {
        // else tell the user no matches were found
        var resultshere = document.getElementById("resultshere");
        var list = document.createElement("ol");
        resultshere.appendChild(list);
        var listitem = document.createElement("li");
        var item = document.createElement("span");
        item.id = "span1_sinresultados";
        item.innerHTML = "No hay legajos con el número que esta ingresando: " + searchterm.fontcolor('Black');
        list.appendChild(listitem);
        listitem.appendChild(item);
        
    }
}