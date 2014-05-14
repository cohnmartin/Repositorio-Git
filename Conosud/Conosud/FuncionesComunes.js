/// <reference name="MicrosoftAjax.js"/>


Sys.Application.add_init(AppInit);

function AppInit(sender) {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(SetTam);
}

//the following code use radconfirm to mimic the blocking of the execution thread.
//The approach has the following limitations:
//1. It works inly for elements that have *click* method, e.g. links and buttons
//2. It cannot be used in if(!confirm) checks
window.blockConfirm = function(text, mozEvent, oWidth, oHeight, callerObj, oTitle) {
    var ev = mozEvent ? mozEvent : window.event; //Moz support requires passing the event argument manually 
    //Cancel the event 
    ev.cancelBubble = true;
    ev.returnValue = false;
    if (ev.stopPropagation) ev.stopPropagation();
    if (ev.preventDefault) ev.preventDefault();

    //Determine who is the caller 
    var callerObj = ev.srcElement ? ev.srcElement : ev.target;

    //Call the original radconfirm and pass it all necessary parameters 
    if (callerObj) {
        //Show the confirm, then when it is closing, if returned value was true, automatically call the caller's click method again. 
        var callBackFn = function(arg) {
            if (arg) {
                callerObj["onclick"] = "";
                if (callerObj.click) callerObj.click(); //Works fine every time in IE, but does not work for links in Moz 
                else if (callerObj.tagName == "A") //We assume it is a link button! 
                {
                    try {
                        eval(callerObj.href)
                    }
                    catch (e) { }
                }
            }
        }

        radconfirm(text, callBackFn, oWidth, oHeight, callerObj, oTitle);
    }
    return false;
}


function SetTam(sender, args) {

    var mensaje = args.get_postBackElement().getAttribute("Mensaje");

    var div = document.getElementById('divBloq1');

    if (div != null) {
        if (document.documentElement.clientHeight > document.documentElement.scrollHeight)
            div.style.height = document.documentElement.clientHeight + "px";
        else
            div.style.height = document.documentElement.scrollHeight + "px";


        div.style.width = document.documentElement.scrollWidth + "px";
        div.style.top = div.style.left = 0;
        div.style.position = "absolute";
        div.style.backgroundColor = "black";
        div.style.zIndex = 999000000;


        try { div.style.opacity = "0.5"; }
        catch (e) { }

        if (typeof (div.style.filter) != "undefined") {
            div.style.filter = "alpha(opacity=50);";
        }
        else if (typeof (div.style.MozOpacity) != "undefined") {
            div.style.MozOpacity = 1 / 2;
        }

        if (mensaje != null) {
            var lbl = document.getElementById('divTituloCarga');
            lbl.innerText = mensaje;
        }
        else {
            var lbl = document.getElementById('divTituloCarga');
            lbl.innerText = "";
        }
    }
}

window.blockConfirmCallBackFn = function (text, mozEvent, oWidth, oHeight, callerObj, oTitle, CallBackFn) {
    var ev = mozEvent ? mozEvent : window.event; //Moz support requires passing the event argument manually
    //Cancel th e event 

    ev.cancelBubble = true;
    ev.returnValue = false;
    if (ev.stopPropagation) ev.stopPropagation();
    if (ev.preventDefault) ev.preventDefault();

    //Determine who is the caller 
    var callerObj = ev.srcElement ? ev.srcElement : ev.target;

    //Call the original radconfirm and pass it all necessary parameters 
    if (callerObj) {
        //Show the confirm, then when it is closing, if returned value was true, automatically call the caller's click method again. 
        radconfirm(text, CallBackFn, oWidth, oHeight, callerObj, oTitle);
    }
    return false;
}