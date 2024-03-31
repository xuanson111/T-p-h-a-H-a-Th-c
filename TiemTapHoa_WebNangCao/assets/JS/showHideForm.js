function handleShowHideForm(options) {
    // property
    let form = document.querySelector(options.form);
    let btnShowForms = document.querySelectorAll(options.btnShowSelector);

    // event listener
    if (form) {
        let btnCloseForms = form.querySelectorAll(options.btnCloseSelector);
        let deck = document.querySelector(options.deckSelector);

        btnShowForms.forEach((item) => {
            item.onclick = () => {
                form.style.display = 'block';
                deck.style.display = 'block'
            }
        })

        btnCloseForms.forEach(item => {
            item.onclick = () => {
                form.style.display = 'none';
                deck.style.display = 'none'
            }
        })

        deck.onclick = () => {
            form.style.display = 'none';
            deck.style.display = 'none'
        }

        if (form.submit) {
            form.querySelectorAll("input").forEach(input => {
                input.value = "";
            })
        }
    }
}