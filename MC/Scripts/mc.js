
/*
 *保存时框架调用方法
 */
function save(data) {
    $.hideLoading();
    switch (data.toString()) {
        case "success":
            $.toptip('保存成功', 'success');
            break;
        case "faild":
            $.toptip('保存失败,请重新登录！', 'error');
            break;
        case "valid":
            $.toptip('请输入完整信息！', 'warning');
            break;
        case "error":
            $.toptip('请重新登录！', 'error');
            break;
        default:
            $.toptip('网络有异常！', 'error');
            break;
    }
}

function hideLoading() {
    $.hideLoading();
}
