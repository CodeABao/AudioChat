(function () {

    var _ = {};

    _.GetResponse = function () {
        var reps = document.getElementsByClassName("markdown");
        var last = reps[reps.length - 1];
        const bridge = chrome.webview.hostObjects.bridge;
        bridge.BackResponse(last.innerText);
    }
    _.CommitComment = function () {
        debugger;
        document.getElementById("prompt-textarea").nextSibling.click();
    }

    _.InputComment = function (comment) {
        debugger;
        var commentArea = document.getElementById("prompt-textarea");
        commentArea.click();

        const bridge = chrome.webview.hostObjects.bridge;
        bridge.InsertTextAsync(comment);
    }

    window.DOUYINDATA = _;
})()