$(document).ready(function () {
    // Function to update wishlist count badge
    function updateWishlistCount() {
        $.ajax({
            url: '/Donation/GetWishlistCount',
            type: 'GET',
            success: function (data) {
                var count = data.Count;
                $('#wishlist-count-badge').text(count);
                // Show badge
                $('#wishlist-count-badge').show();
            },
            error: function () {
                console.log('Error fetching wishlist count.');
            }
        });
    }

    // Call updateWishlistCount initially
    updateWishlistCount();

    // Event handler for adding book to wishlist
    $('#add-to-wishlist-btn').click(function (e) {
        e.preventDefault(); // Prevent default form submission behavior
        var form = $(this).closest('form');
        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: form.serialize(),
            success: function () {
                updateWishlistCount(); // Update wishlist count after adding book
            },
            error: function () {
                console.log('Error adding book to wishlist.');
            }
        });
    });
});
