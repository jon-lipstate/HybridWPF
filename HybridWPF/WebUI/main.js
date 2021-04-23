console.log("hello ze console")
function postmsg() {
    window.chrome.webview.postMessage(JSON.stringify({foo:"bar"}));     
}
function wll() {
    console.log("FAAASDASDASDA");
    
}
function initializeinterop() {
    console.log(`initializeinterop`)
    var gv = window.globalVar;

    gv = {};
    //gv.textbox = window.chrome.webview.hostObjects.sync.mm;
    //gv.textboxAsync = window.chrome.webview.hostObjects.mm;
}
function testReturn() {
    return JSON.stringify({ k: "mary had a little lamb"})
}

async function callHelloWorldDotnet(name) {
    // .NET object reference (async)
    var msg = await window.chrome.webview.hostObjects.mm.HelloWorldSync(name);
    alert(msg);
}