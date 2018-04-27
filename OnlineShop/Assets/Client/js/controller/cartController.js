var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/";
        });

        $('#btnUpdate').off('click').on('click', function () {
            var listProduct = $('.txtQuatity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quatity: $(item).val(),
                    Product: {
                        ID: $(item).data('id')
                    }
                });
            });

            $.ajax({
                url: '/Cart/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: "JSON",
                type: 'POST',
                success: function (response) {
                    if (response.status == true)
                        window.location.href = '/gio-hang';
                    else
                    {

                    }
                }
            });
        });

        $('#btnDeleteAll').off('click').on('click', function () {
            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'JSON',
                type: 'POST',
                success: function (response) {
                    if (response.status == true) {
                        window.location.href = '/gio-hang';
                    }
                    else { }
                }
            });
        });

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Cart/Delete', 
                data: { id: $(this).data('id') },
                dataType: 'json',
                type: 'POST',
                success: function (response) {
                    if (response.status == true)
                        window.location.href = '/gio-hang';
                    else {

                    }
                }
            });
        });

        $('#btnPayment').off('click').on('click', function () {
            window.location.href = '/thanh-toan';
        });
    }
}
cart.init();