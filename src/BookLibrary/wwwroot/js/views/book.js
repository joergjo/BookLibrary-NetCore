/* global Backbone */
/* global _ */

var app = app || {};

app.BookView = Backbone.View.extend({
    tagName: 'div',
    className: 'bookContainer',
    template: $('#bookTemplate').html(),
    events: {
        'click .delete': 'deleteBook'
    },

    render: function () {
        var tmpl = _.template(this.template);
        this.$el.html(tmpl(this.model.toJSON()));
        return this;
    },

    deleteBook: function () {
        this.model.destroy();
        this.remove();
    }
});