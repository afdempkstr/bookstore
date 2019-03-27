$(function () {
    // Reference the auto-generated proxy for the hub.
    var notifications = $.connection.notificationHub;

    // Create a function that the hub can call back to display messages.
    notifications.client.onNotify = function (message) {
        alert(message);
    };

    //create a function that the hub can call back to notify us that a new book was added
    notifications.client.onBookAdded = function(book) {
        //create a copy of the notification div that is initially hidden
        var notificationElement = $('#notification').clone();
        notificationElement.attr('id', '#notification-' + Math.random());
        // fill it with data
        notificationElement.find('.notify-book-title').html(book.Title);
        notificationElement.find('.notify-book-author').html(book.Author);
        // display it
        $('#notification-container').prepend(notificationElement);
        $('#notification-container').removeClass('hidden');
        notificationElement.toggleClass('fade');
        notificationElement.fadeIn();
    };

    // Start the connection.
    $.connection.hub.start().done(function () {
        console.log("Connected to notifications hub");
    });
});