$(document).ready(function () {
    // Retrieve TempData values
    var bookInWishlist = '@TempData["BookInWishlist"]' === 'True';
    var booksInWishlistDB = '@TempData["BooksInWishlistDB"]' === 'True';

    // Update "Add to Wishlist" button
    var addToWishlistButton = $('#add-to-wishlist-btn');

    if (bookInWishlist || booksInWishlistDB) {
        addToWishlistButton.prop('disabled', true);
        addToWishlistButton.text('Added to Wishlist');
        addToWishlistButton.removeClass('btn-primary').addClass('btn-success');
    } else {
        addToWishlistButton.prop('disabled', false);
        addToWishlistButton.text('Add to Wishlist');
        addToWishlistButton.removeClass('btn-success').addClass('btn-primary');
    }

    if (booksInWishlistDB) {
        $('#message-area').text("You have requested some books, thus new ones can't be added.");
    } else {
        $('#message-area').hide();
    }

});