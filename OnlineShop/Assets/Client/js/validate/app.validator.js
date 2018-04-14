$(document).ready(function () {

    var feedbackIcons = {
        valid: 'glyphicon glyphicon-ok',
        invalid: 'glyphicon glyphicon-remove',
        validating: 'glyphicon glyphicon-refresh'
    };


    var nameContact = {
        validators: {
            notEmpty: {
                message: 'Họ tên không được để trống'
            },
            stringLength: { max: 50, min: 5, message: 'Tên quá ngắn' },
            regexp: {
                regexp: /^[a-zA-Z]+$/,
                message: 'Tên của bạn không số hoặc kí tự đặc biệt'
            }
        }
    }

    var emailContact = {
        validators: {
            notEmpty: {
                message: 'Bạn cần phải nhập email'
            },
            emailAddress: {
                message: 'Email của bạn không đúng'
            }
        }
    }

    var mobileContact = {
        validators: {
            notEmpty: {
                message: 'Bạn cần phải nhập số điện thoại'
            },
            stringLength: {
                min: 10,
                max: 11,
                message: 'Số điện thoại quá ngắn hoặc quá dài'
            },
            regexp: {
                regexp: /^[0-9\s\-()+\.]+$/,
                message: 'Số điện thoại không chứa kí tự đặc biệt, chữ cái.'
            }
        }
    }

    var addressContact = {
        validators: {
            notEmpty: {
                message: 'Bạn phải nhập đia chỉ'
            },
            stringLength: {
                min: 12,
                max: 80,
                message: 'Địa chỉ quá ngắn' 
            }
        }
    }

    var contentContact = {
        validators: {
            notEmpty: {
                message: 'Bạn phải nhập nội dung'
            },
            stringLength: {
                max: 800,
                min: 15,
                message: 'Nội dung quá ngắn'
            }
        }        
    }

    var formContact = {
        feedbackIcons: feedbackIcons,
        fields: {
            txtName: nameContact,
            txtEmail: emailContact,
            txtMobile: mobileContact,
            txtAddress: addressContact,
            txtContent: contentContact
        }
    };

    $('#form_contact').bootstrapValidator(formContact);

});