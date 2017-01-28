/* global Backbone */

var app = app || {};

app.Book = Backbone.Model.extend({
    defaults: {
        coverImage: 'images/placeholder.png',
        title: 'No title',
        author: 'Unknown',
        releaseDate: 'Unknown',
        keywords: 'None'
    },

    idAttribute: '_id'
});