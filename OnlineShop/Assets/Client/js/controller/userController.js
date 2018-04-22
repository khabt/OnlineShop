var user = {
    init: function () {
        user.loadProvince();
        user.registerEvent();
    },
    registerEvent: function () {
        $('#ddlProvince').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                user.loadDistrict(parseInt(id));
            }
            else {
                $('#ddlDistrict').html('');                
            }
            $('#ddlWard').html('');
            $('#Address').val('');
        });

        $('#ddlDistrict').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                user.loadWard(parseInt(id));
            }
            else {
                $('#ddlWard').html('');                
            }
            $('#Address').val('');
        });

        $('#ddlWard').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                user.getAddress();
            }
            else {
                $('#Address').val('');
            }
        });
    },
    loadProvince: function () {
        $.ajax({
            url: 'User/LoadProvince',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var html = '<option value="">---Chọn tỉnh/thành--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        //html += '<option value="' + item.ID + '">' + `${i + 1}.` + item.Name + '</option>';
                        i = i + 1;
                        var order = i.toString().length == 1 ? '0' + i : i;
                        html += `<option value='${item.ID}'>${i+1}. ${item.Name}</option>`
                    });
                    $('#ddlProvince').html(html);
                }
            }
        });
    },
    loadDistrict: function (id) {
        $.ajax({
            url: '/User/LoadDistrict',
            type: 'POST',
            data: { provinceID: id },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var html = '<option value="">--Chọn quận/huyện--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.ID + '">' + item.Name + '</option>';
                    });
                    $('#ddlDistrict').html(html);
                }
            }
        });
    },
    loadWard: function (id) {
        $.ajax({
            url: '/User/LoadWard',
            type: 'POST',
            dataType: 'json',
            data: { districtID: id },
            success: function (response) {
                var html = '<option value="">--Chọn xã/phường--</option>';
                var data = response.data;
                $.each(data, function (i, item) {
                    html += '<option value="' + item.ID + '">' + item.Name + '</option>';
                });
                $('#ddlWard').html(html);
            }
        });
    },
    getAddress: function () {
        var address = $('#ddlWard option:selected').text() + ', ' + $('#ddlDistrict option:selected').text() + ', ' + $('#ddlProvince option:selected').text();
        $('#Address').val(address);
    }
}
user.init();