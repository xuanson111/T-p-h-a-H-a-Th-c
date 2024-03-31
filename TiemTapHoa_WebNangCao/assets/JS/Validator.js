
function Validator(options) {
    // property =====================================================
    let formElement = document.querySelector(options.form);
    // biến lưu trữ các rules\
    let selectorRules = {};

    // function ========================================================
    function getParent(element, selector) {
        if (element) {
            while (element.parentElement) {
                if (element.parentElement.matches(selector)) {
                    return element.parentElement;
                }
                element = element.parentElement;
            }
        }
    }

    function validate(inputElement, rule) {
        let errorElement = getParent(inputElement, options.formGroupSelector).querySelector(options.errorSelector);
        let errorMessage;

        // lặp qua các rule của inputElement, nếu có lỗi thì bỏ qua
        let rules = selectorRules[rule.selector];
        for (let i = 0; i < rules.length; i++) {
            switch (inputElement.type) {
                case 'checkbox':
                case 'radio':
                    errorMessage = rules[i](
                        formElement.querySelector(rule.selector + ':checked')
                    );
                    break;
                default:
                    errorMessage = rules[i](inputElement.value);
            }
            if (errorMessage) break;
        }

        if (errorMessage) {
            errorElement.innerText = errorMessage;
            getParent(inputElement, options.formGroupSelector).classList.add('invalid');
        }
        return !!errorMessage;
    }

    function clearError(inputElement) {
        let parentElement = getParent(inputElement, options.formGroupSelector);
        let errorElement = parentElement.querySelector(options.errorSelector);

        parentElement.classList.remove('invalid');
        errorElement.innerText = '';
    }

    // event listener =============================================================
    if (formElement) {

        formElement.onsubmit = (e) => {
            e.preventDefault();
            let isFormValid = true;
            // lặp qua tưng rule và xử lý, nếu có lỗi thì gán isFormValid = false
            options.rules.forEach((rule) => {
                let inputElement = formElement.querySelector(rule.selector);
                let isValid = validate(inputElement, rule);
                if (isValid) {
                    isFormValid = false;
                }
            });


            if (isFormValid) {
                // trường hợp submit với javascript
                if (typeof options.onSubmit === 'function') {
                    let inputs = formElement.querySelectorAll('[name]');
                    let enableInput = Array.from(inputs).filter(function (input) {
                        return !input.disabled;
                    });

                    let formValues = Array.from(enableInput).reduce((values, input) => {
                        switch (input.type) {
                            case 'radio':
                                if (input.matches(':checked')) {
                                    values[input.name] = input.value;
                                }
                                break;
                            case 'checkbox':
                                if (!Array.isArray(values[input.name])) {
                                    values[input.name] = [];
                                }
                                if (!input.matches(':checked')) return values;
                                values[input.name].push(input.value);
                                break;
                            case 'file':
                                values[input.name] = input.files;
                                break;
                            default:
                                values[input.name] = input.value;
                        }
                        return values;
                    }, {});
                    options.onSubmit(formValues);
                } else {
                    // trường hợp submit với hành vi mặc định
                    formElement.submit();
                }
            }
        }

        options.rules.forEach((rule) => {
            // lưu lại các rule cho mỗi input, mỗi input có thể nhiều rule khác nhau
            if (Array.isArray(selectorRules[rule.selector])) {
                selectorRules[rule.selector].push(rule.test);
            } else {
                selectorRules[rule.selector] = [rule.test];
            }

            let inputElements = formElement.querySelectorAll(rule.selector);

            if (!inputElements) return;

            Array.from(inputElements).forEach(function (inputElement) {
                let errorElement = getParent(inputElement, options.formGroupSelector).querySelector(options.errorSelector);
                if (inputElement && !inputElement.disabled) {
                    // xử lý trường hợp người dùng blur khỏi ô input
                    inputElement.onblur = () => {
                        validate(inputElement, rule);
                    }

                    // xử lý khi người dùng nhập vào ô input
                    inputElement.oninput = () => {
                        errorElement.innerText = '';
                        getParent(inputElement, options.formGroupSelector).classList.remove('invalid');
                    }

                    /*if (inputElement.type === 'file') {
                        inputElement.onchange = () => {
                            validate(inputElement, rule);
                        }
                        let file = inputElement.files[0];
                        let imgElement = getParent(inputElement, options.formGroupSelector).querySelector('img');
                        if (file && imgElement) {
                            let reader = new FileReader();
                            reader.onload = function (e) {
                                imgElement.src = e.target.result;
                            };
                            reader.readAsDataURL(file);
                        }
                    }*/
                }
            })
        });


    // Xử lý sự kiện các button trong form (thêm, sửa, xóa) ==========================================================
    // sự kiện nút làm mới
    let btnReset = formElement.querySelector(options.btnResetSelector);
    if (btnReset) {
        btnReset.onclick = (e) => {
            e.preventDefault();
            let idMessage = formElement.querySelector(options.idMessageSelector);
            if (idMessage) {
                idMessage.value = '';
            }
            // truy vấn đến tất cả ô input trong form
            let inputs = formElement.querySelectorAll('[name]');
            let enableInput = Array.from(inputs).filter(function (input) {
                return !input.disabled;
            });
            // duyệt qua tất cả ô input
            Array.from(enableInput).forEach((input) => {
                clearError(input);
                switch (input.type) {
                    case 'radio':
                    case 'checkbox':
                        if (input.matches(':checked')) {
                            input.checked = false;
                        }
                        break;
                    case 'file':
                        let imgElement = getParent(input, options.formGroupSelector).querySelector('img');
                        if (!imgElement) {
                            break;
                        }
                        imgElement.src = "https://st.quantrimang.com/photos/image/2017/04/08/anh-dai-dien-FB-200.jpg";
                        input.value = '';
                        break;
                    default:
                        input.value = '';
                }
            })
        }
    }


    // let btnEdit = formElement.querySelector(options.btnEditSelector);
    // if (btnEdit) {
    //   btnEdit.onclick = (e) => {
    //     e.preventDefault();
    //     let idMessage = formElement.querySelector(options.idMessageSelector);
    //     console.log(idMessage.textContent)
    //   }
    // }

}

}



// định nghĩa các rule
Validator.isRequired = function (selector, message) {
    return {
        selector,
        test(value) {
            return value ? undefined : message || 'vui lòng nhập trường này';
        }
    }
}

// keyword: javascript email regex
Validator.isEmail = function (selector) {
    return {
        selector,
        test(value) {
            return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(value) ? undefined : 'Trường này phải là email';
        }
    }
}

Validator.minLength = function (selector, min) {
    return {
        selector,
        test(value) {
            return value.length >= min ? undefined : 'Nhập tối thiểu ' + min + ' kí tự';
        }
    }
}

Validator.isConfirmed = (selector, getConfirmValue, message) => {
    return {
        selector,
        test(value) {
            return value === getConfirmValue() ? undefined : message || 'Gía trị nhập vào không chính xác';
        }
    }
}

Validator.isImage = (selector) => {
    return {
        selector,
        test(value) {
            if (value === "") {
                return undefined;
            }
            return (/\.(gif|jpe?g|tiff?|png|webp|bmp)$/i).test(value) ? undefined : 'Vui lòng chọn file hình ảnh';
        }
    }
}

Validator.isNumberAndMinValue = function (selector, minValue, message) {
    return {
        selector,
        test(value) {
            const isValidNumber = !isNaN(value) && isFinite(value);
            return (isValidNumber && value >= minValue) ? undefined : message || `Vui lòng nhập một số lớn hơn hoặc bằng ${minValue}`;
        }
    };
};

/** Cách dùng
 * 
    Validator({
        form: '#form-1',
        formGroupSelector: '.form-group',
        errorSelector: '.form-message',
        rules: [
            Validator.isRequired('#fullname'),

            Validator.isRequired('#email'),
            Validator.isEmail('#email'),

            Validator.minLength('#password', 6),

            Validator.isRequired('#password_confirmation'),
            Validator.isConfirmed('#password_confirmation', function() {
              return document.querySelector('#form-1 #password').value;
            }, 'Mật khẩu nhập lại không chính xác'),

            Validator.isRequired('input[name="gender"]'),

            Validator.isRequired('#province'),

            Validator.isRequired('#avatar')
        ],
        onSubmit(data) {
          console.log(data);
        }
    });
 * 
 */