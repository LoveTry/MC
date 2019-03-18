
/*
 *保存时框架调用方法
 */
function save(data) {
    $.hideLoading();
    if (data.url) {
        window.location.href = data.url;
    }
    else {
        $.toptip(data.msg, data.code == 1 ? 'success' : (data.code == 2 ? 'error' : 'warning'));
    }

}

function hideLoading() {
    $.hideLoading();
}
