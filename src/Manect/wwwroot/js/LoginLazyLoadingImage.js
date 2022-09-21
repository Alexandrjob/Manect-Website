// progressive-image.js
if (window.addEventListener && window.requestAnimationFrame && document.getElementsByClassName) window.addEventListener('load', function () {
    // start
    var pItem = document.getElementsByClassName('progressive replace'), timer;
    inView();

    // image in view?
    function inView() {
        var p = 0;
        while (p < pItem.length) {
            loadFullImage(pItem[p]);
            pItem[p].classList.remove('replace');
        }
    }

    // replace with full image
    function loadFullImage(item) {
        if (!item || !item.href) return;
        // load image
        var img = new Image();
        if (item.dataset) {
            img.srcset = item.dataset.srcset || '';
            img.sizes = item.dataset.sizes || '';
        }
        img.src = item.href;
        img.className = 'reveal';
        if (img.complete) addImg();
        else img.onload = addImg;

        // replace image
        function addImg() {
            // disable click
            item.addEventListener('click', function (e) {
                e.preventDefault();
            }, false);

            // add full image
            item.appendChild(img).addEventListener('animationend', function (e) {

                // remove preview image
                var pImg = item.querySelector && item.querySelector('img.preview');
                if (pImg) {
                    e.target.alt = pImg.alt || '';
                    item.removeChild(pImg);
                    e.target.classList.remove('reveal');
                }
            });
        }
    }
}, false);
