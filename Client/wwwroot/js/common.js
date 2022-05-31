export function setMasterColor(hueSaturation) {
    const COLOR_BASE_KEY = '--k-color-primary-base';
    let root = document.documentElement;
    root.style.setProperty(COLOR_BASE_KEY, hueSaturation);
}

export function clickElement(id) {
    document.getElementById(id).click();
}