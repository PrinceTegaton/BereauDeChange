$(function () {
    $('#logout').click(function () {
        $.post('/Home/Logout', function (r) {
            location.href = '/Home/Index';
        });
    });

    $('#switch-signup').click(function () {
        $('#signin-area').hide();
        $('#signup-area').show();
    })

    $('#switch-signin').click(function () {
        $('#signin-area').show();
        $('#signup-area').hide();
    })

    $('#form-signin').submit(function (e) {
        e.preventDefault();

        var $form = $(this);
        $.post('/Home/Login', $form.serialize(), function (r) {
            if (!r.isSuccess) {
                $('#form-signin .alert').text(r.message);
                $('#form-signin .alert').show();
            }
            else {
                location.href = '/Home/Dashboard'
            }
        })
    });

    $('#form-signup').submit(function (e) {
        e.preventDefault();

        var $form = $(this);
        $.post('/Home/CreateAccount', $form.serialize(), function (r) {
            if (!r.isSuccess) {
                $('#form-signup .alert').text(r.message);
                $('#form-signup .alert').show();
            }
            else {
                alert(r.message);
                location.href = '/Home/Dashboard'
            }
        })
    })

    $('#currencylist').change(function () {
        var currency = $(this).find('option:selected').text();
        var rate = $(this).val();

        $('#rate').val(rate);
        $('#currency').val(currency);
    })

    $('#btn-auth').click(function () {
        var currency = $('#currencylist option:selected').text();
        if (currency == '---') {
            alert('You have not selected destination currency');
            $('form #fail-msg').text('You have not selected destination currency');
            $('form #fail-msg').show();
            return;
        } else {
            $('form #fail-msg').hide();
        }

        // show local amount
        var amount = parseFloat($('#amount').val());
        var rate = parseFloat($('#rate').val());
        var amountlocal = amount * rate;

        $('#amountlocal').text('₦' + amountlocal);
        $('#charges').text('$0.95 (fixed)');
        $('#beneficiaryinfo').hide();
        $('#auth').show();
    })

    $('#btn-back').click(function () {
        $('#beneficiaryinfo').show();
        $('#auth').hide();
    })

    $('#form-transfer').submit(function (e) {
        e.preventDefault();

        var $form = $(this);
        $.post('/Home/Transfer', $form.serialize(), function (r) {
            if (!r.isSuccess) {
                $('form #fail-msg').text(r.message);
                $('form #fail-msg').show();
            }
            else {
                $('#auth').hide();
                $('form #fail-msg').hide();
                $('#success').show();

                $('#msg').text(r.data);
            }
        })
    })
});