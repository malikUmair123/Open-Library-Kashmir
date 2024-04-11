$(document).ready(function () {
    // Retrieve TempData values
    var bookInWishlist = '@TempData["BookInWishlist"]' === 'True';

    // Update "Add to Wishlist" button
    var addToWishlistButton = $('#add-to-wishlist-btn');
    var messageArea = $('#message-area');

    if (bookInWishlist) {
        addToWishlistButton.prop('disabled', true);
        addToWishlistButton.removeClass('btn-primary').addClass('btn-success');
        messageArea.text("Book added to Wishlist");
    } else {
        messageArea.hide();
    }

});