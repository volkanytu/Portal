(function ($) {
    $.widget("metro.panel", {

        version: "1.0.0",

        options: {
            onCollapse: function () { parent.autoResize('ifrmContent'); },
            onExpand: function () {parent.autoResize('ifrmContent');}
        },

        _create: function () {
            //debugger;
            var ssss = this.element.attr("id");
            var element = this.element, o = this.options,
                header = element.children('.panel-header'),
                content = element.children('.panel-content');

            header.on('click', function () {
                //debugger;
                content.slideToggle(
                    'fast',
                    function () {
                        element.toggleClass('collapsed');
                        if (element.hasClass('collapsed')) {
                            o.onCollapse();
                        } else {
                            //debugger;
                            o.onExpand();
                        }
                    }
                );
            });
        },

        _destroy: function () {

        },

        _setOption: function (key, value) {
            //debugger;
            this._super('_setOption', key, value);
        }
    })
})(jQuery);


