if (typeof (jQuery) != 'undefined') {

    // http://www.paulirish.com/2009/throttled-smartresize-jquery-event-handler/
    (function ($, sr) {

        // debouncing function from John Hann
        // http://unscriptable.com/index.php/2009/03/20/debouncing-javascript-methods/
        var debounce = function (func, threshold, execAsap) {
            var timeout;

            return function debounced() {
                var obj = this, args = arguments;
                function delayed() {
                    if (!execAsap)
                        func.apply(obj, args);
                    timeout = null;
                };

                if (timeout)
                    clearTimeout(timeout);
                else if (execAsap)
                    func.apply(obj, args);

                timeout = setTimeout(delayed, threshold || 100);
            };
        }
        // smartresize 
        jQuery.fn[sr] = function (fn) { return fn ? this.bind('resize', debounce(fn)) : this.trigger(sr); };

    })(jQuery, 'smartresize');



    // By Chris Coyier & tweaked by Mathias Bynens
    // https://css-tricks.com/fluid-width-youtube-videos/

        $(function () {

            // Embed YouTube videos
            var youTube = new RegExp(/^https?:\/\/(youtu.be\/|www.youtube.com\/watch\?v=)([A-Za-z0-9_-]+)$/);
            var youTubeWidth = 450, youTubeHeight = 318;

            $("a.embed").filter(function (index) {
                return youTube.test(this.href);
            }).each(function () {

                // Swop YouTube link for embedded video
                var match = youTube.exec(this.href);
                $(this).replaceWith('<iframe width="' + youTubeWidth + '" height="' + youTubeHeight + '" src="https://www.youtube-nocookie.com/embed/' + match[2] + '" frameborder="0" allowfullscreen="allowfullscreen" class="video"></iframe>');
            });

            // Find embedded YouTube videos
            var $allVideos = $("iframe[src^='https://www.youtube-nocookie.com']"),

                // The element that is fluid width
                $fluidEl = $(".body .container");

            // Figure out and save aspect ratio for each video
            $allVideos.each(function () {

                $(this)
                    .data('aspectRatio', this.height / this.width)

                    // and remove the hard coded width/height
                    .removeAttr('height')
                    .removeAttr('width');
            });

            // When the window is resized
            $(window).smartresize(function () {

                var newWidth = $fluidEl.width();

                // Resize all videos according to their own aspect ratio
                $allVideos.each(function () {

                    var $el = $(this);
                    $el
                        .width(newWidth)
                        .height(newWidth * $el.data('aspectRatio'));

                });

                // Kick off one resize to fix all videos on page load
            }).resize();

        });


}