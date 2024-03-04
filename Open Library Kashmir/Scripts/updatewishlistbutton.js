$(document).ready(function () {
    // Check TempData["BookInWishlist"] value
    var bookInWishlist = '@TempData["BookInWishlist"]';

    // Update properties of "Add to Wishlist" button based on TempData value
    if (bookInWishlist === 'True') {
        // Book is already in wishlist
        var bookInWishlist = '@TempData["BookInWishlist"]';

        $('#add-to-wishlist-btn').prop('disabled', true);
        $('#add-to-wishlist-btn').text('Added to Wishlist');
        $('#add-to-wishlist-btn').removeClass('btn-primary').addClass('btn-success');
    } else {
        var bookInWishlist = '@TempData["BookInWishlist"]';

        // Book is not in wishlist
        $('#add-to-wishlist-btn').prop('disabled', false);
        $('#add-to-wishlist-btn').text('Add to Wishlist');
        $('#add-to-wishlist-btn').removeClass('btn-success').addClass('btn-primary');
    }
});