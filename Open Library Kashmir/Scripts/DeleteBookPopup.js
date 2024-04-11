$(document).ready(function () {
    $('.delete-btn').click(function () {

        var bookId = $(this).attr('id');
        var confirmation = confirm("Are you sure you want to delete this book?");
        if (confirmation) {
            $.ajax({
                url: '@Url.Action("DeleteBook", "Donation")',
                type: 'POST',
                data: { id: bookId },
                success: function (result) {
                    // Optionally, you can handle success response
                    // For example, remove the row from the table
                    $('#row_' + bookId).remove();
                },
                error: function (xhr, status, error) {
                    // Optionally, you can handle error response
                    console.error(xhr.responseText);
                }
            });
        }
    });
});