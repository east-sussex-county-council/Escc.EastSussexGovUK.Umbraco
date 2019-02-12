if (typeof (jQuery) != 'undefined') {
    $(function () {

        // Define the file types we're looking for
        var types = {
            pdf: "Adobe PDF",
            rtf: "RTF",
            doc: "Word",
            docx: "Word",
            dot: "Word",
            dotx: "Word",
            xls: "Excel",
            xlsx: "Excel",
            xlt: "Excel",
            xltx: "Excel",
            csv: "CSV",
            ppt: "PowerPoint",
            pptx: "PowerPoint",
            pps: "PowerPoint",
            ppsx: "PowerPoint",
            pot: "PowerPoint",
            potx: "PowerPoint",
            mp3: "MP3",
            mov: "QuickTime",
            wmv: "Windows Media",
            wma: "Windows Media",
            exe: "Computer program",
            zip: "zipped files",
            gif: "Image",
            jpg: "Image",
            jpeg: "Image",
            png: "Image",
        }

        // Get all the document links on the page, unless specifically excluded
        var mediaLinks = $("a:not([class~='no-meta'])").filter(function() {
            return new RegExp("(" + Object.keys(types).join("|\\.") + ")$", "i").test($(this).attr('href'));
        });        

        // If there are any documents, get the node id from the metadata and request info on the documents for this Umbraco node
        if (mediaLinks.length) {
            var nodeId = $("meta[name='eGMS.systemID']").attr("content");
            if (!nodeId) return;

            var url = "/umbraco/api/media/GetMediaOnPage?pageid=" + nodeId.replace(/[^0-9]/gi,'');
            $.getJSON(url, function(data) {

                $.each(data, function () {
                    // For each media item returned by the API, get the matching media link from the original set by matching the URL
                    var result = this;
                    mediaLinks.filter(function() {
                        return this.href.toUpperCase().indexOf(result.url.toUpperCase(), this.href.length - result.length) !== -1;
                    })

                    // Append format and size to link. 
                    // Sanitise text to defend against XSS because append() accepts HTML.
                    .each(function () {
                        var textToAppend = " (" + types[result.type] + ", " + result.size + ")".replace(/[^A-Za-z0-9, ()]/gi, '');
                        textToAppend = '<small class=\"downloadDetail\">' + textToAppend + "</small>";

                        $(this).append(textToAppend);
                    });
                });
            });
        }
    });
}