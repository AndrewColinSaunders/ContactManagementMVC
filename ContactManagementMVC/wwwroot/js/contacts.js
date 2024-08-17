$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();

    $('#addContactButton').click(function () {
        $.get('/Contacts/Add', function (data) {
            $('#addContactModalContent').html(data);
            $('#addContactModal').modal('show');
        });
    });

    $(document).on('submit', '#addContactForm', function (event) {
        event.preventDefault();
        var form = $(this);
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            headers: {
                'RequestVerificationToken': token
            },
            success: function (response) {
                if (response.success) {
                    $('#addContactModal').modal('hide');
                    location.reload();
                } else {
                    $('#addContactModalContent').html(response);
                }
            }
        });
    });

    window.showEditModal = function (contactId, name, email, phoneNumber, address) {
        $.get('/Contacts/Edit', { id: contactId }, function (data) {
            $('#editContactModalContent').html(data);
            $('#editContactModal').modal('show');
            $('#editContactModal').find('#Name').val(name);
            $('#editContactModal').find('#Email').val(email);
            $('#editContactModal').find('#PhoneNumber').val(phoneNumber);
            $('#editContactModal').find('#Address').val(address);
        });
    }

    $(document).on('submit', '#editContactForm', function (event) {
        event.preventDefault();
        var form = $(this);
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            headers: {
                'RequestVerificationToken': token
            },
            success: function (response) {
                if (response.success) {
                    $('#editContactModal').modal('hide');
                    location.reload();
                } else {
                    $('#editContactModalContent').html(response);
                }
            }
        });
    });

    window.showDeleteModal = function (contactId) {
        $('#deleteConfirmationModal').modal('show');
        $('#confirmDeleteButton').off('click').on('click', function () {
            $.ajax({
                url: '/Contacts/Delete/' + contactId,
                type: 'POST',
                headers: {
                    'RequestVerificationToken': token
                },
                success: function () {
                    $('#deleteConfirmationModal').modal('hide');
                    location.reload();
                }
            });
        });
    }

    $('#searchBar').on('keyup', function () {
        var searchTerm = $(this).val().toLowerCase();
        var filterCriteria = $('#filterCriteria').val();
        var visibleCards = 0;
        $('.contact-card').filter(function () {
            var textToSearch;
            switch (filterCriteria) {
                case 'name':
                    textToSearch = $(this).find('.card-title').text().toLowerCase();
                    break;
                case 'email':
                    textToSearch = $(this).find('.card-text strong:eq(0)').parent().text().toLowerCase();
                    break;
                case 'phone':
                    textToSearch = $(this).find('.card-text strong:eq(1)').parent().text().toLowerCase();
                    break;
                case 'address':
                    textToSearch = $(this).find('.card-text strong:eq(2)').parent().text().toLowerCase();
                    break;
                default:
                    textToSearch = $(this).text().toLowerCase();
            }

            var isVisible = textToSearch.indexOf(searchTerm) > -1;
            $(this).toggle(isVisible);
            if (isVisible) {
                visibleCards++;
            }
        });

        if (visibleCards === 0) {
            $('#noResultsMessage').show();
        } else {
            $('#noResultsMessage').hide();
        }
    });
});
