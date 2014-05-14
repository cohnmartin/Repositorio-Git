window.onload = loadIndex;

function loadIndex() { // load indexfile
    // most current browsers support document.implementation
    if (document.implementation && document.implementation.createDocument) {
        xmlDoc = document.implementation.createDocument("", "", null);
        xmlDoc.load("PruebaBusqueda.xml");
    }
    // MSIE uses ActiveX
    else if (window.ActiveXObject) {
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.async = "false";
        xmlDoc.load("PruebaBusqueda.xml");
    }
}

function showAlert() {
    var tooltip = $find("RadToolTip3");
    var codigos = event.srcElement.getAttribute("Codigos").toString();
    var descripciones = event.srcElement.getAttribute("Descripciones").toString();
    var id = event.srcElement.getAttribute("IdProducto").toString();
    var ProductName = event.srcElement.innerText;
    var ctrName = "";
    tooltip.set_targetControlID(event.srcElement.id);


//    img = document.getElementById(ctrName + "_tblEncabezdo");
      var lblProducto = document.getElementById(ctrName + "lblProducto");
//    label = document.getElementById(event.srcElement.id);


    var ArrayCodigos = codigos.split('|');
    var ArrayDescripciones = descripciones.split('|');

    var i = 0;
    for (i = 0; i < ArrayCodigos.length - 1; i++) {
        eval('document.getElementById(\'' + ctrName + 'lblCol' + i + '\').innerText = ' + 'ArrayDescripciones[i];');
        eval('document.getElementById(\'' + ctrName + 'IdPres' + i + '\').value = ' + 'ArrayCodigos[i];');
        eval('document.getElementById(\'' + ctrName + 'Row' + i + '\').style.display = ' + '\'block\';');
    }



//    newImage = "url(" + label.getAttribute("Imagen") + ")";
//    ProductName = label.getAttribute("Descripcion");
//    img.style.backgroundImage = newImage;

    if (lblProducto != null)
        lblProducto.innerText = ProductName;


    HiddenId = document.getElementById(ctrName + "HiddenId");
    HiddenId.value = id;


    tooltip.show();
}

function setStyle() {
    this.className ='MouseOverStyle';
}
function resetStyle() {
    this.className ='MouseOutStyle';
}
function LimpiarTarget(sender, eventArgs) {
    sender.set_targetControlID();
}

function searchIndex() { // search the index (duh!)
    if (!xmlDoc) {
        loadIndex();
    }
    // get the search term from a form field with id 'searchme'

    var searchterm = document.getElementById("searchme").value;
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
            var name = allitems[i].getAttributeNode("padreDesc");
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
        var header = document.createElement("h5");
        var list = document.createElement("ol");
        var searchedfor = document.createTextNode("You've searched for " + searchterm);
        resultshere.appendChild(header);
        header.appendChild(searchedfor);
        resultshere.appendChild(list);
        for (var i = 0; i < results.length; i++) {
            var listitem = document.createElement("li");
            
            var item = document.createElement("span");
            item.id = "span1_" + results[i].text;
            item.onclick = showAlert;
            item.onmouseover = setStyle;
            item.onmouseout = resetStyle;
            item.innerHTML = results[i].text;


            item.setAttribute("Codigos", results[i].getAttributeNode("Codigos").value);
            item.setAttribute("Descripciones", results[i].getAttributeNode("Descripciones").value);
            item.setAttribute("IdProducto", results[i].getAttributeNode("IdProducto").value);
            
      

            list.appendChild(listitem);
            listitem.appendChild(item);
        }
    } else {
        // else tell the user no matches were found
        var resultshere = document.getElementById("resultshere");
        var para = document.createElement("p");
        var notfound = document.createTextNode("Sorry, I couldn't find anything like " + searchterm + "!");
        resultshere.appendChild(para);
        para.appendChild(notfound);
    }
}