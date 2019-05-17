// Copied unaltered from ScrollToFormScript.cshtml in Umbraco Forms 7.1.1 so that it can be loaded from a .js file rather than rendered inline, 
// and therefore be cacheable and compatible with content security policy.
document.onreadystatechange = function () {

    if (document.readyState == "complete") {
        var validationsDivs = document.querySelectorAll(".field-validation-error, .umbraco-forms-submitmessage");
        var scrollY = 0;
        for (var i = 0; i < validationsDivs.length; i++) {

            var node = getNode(validationsDivs[i]);
            var offset = node.getBoundingClientRect().top;

            if (0 < offset && (offset < scrollY || scrollY === 0)) {
                scrollY = offset;
            }
        }

        if (scrollY > 0) {
            window.scrollTo(0, scrollY);
        }
    }
}

function getNode(node) {
    var runner = node;
    while (runner.tagName !== "BODY") {
        if (runner.classList.contains("umbraco-forms-field")) {
            return runner;
        }

        runner = runner.parentNode;
    }

    return node;
}