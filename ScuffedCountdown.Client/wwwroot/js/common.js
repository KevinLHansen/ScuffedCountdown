export function setMasterColor(hueSaturation) {
    const COLOR_BASE_KEY = '--k-color-primary-base';
    let root = document.documentElement;
    root.style.setProperty(COLOR_BASE_KEY, hueSaturation);
}

export function clickElement(id) {
    document.getElementById(id).click();
}

export function alert1(message) {
    alert(message);
}

export function playErrorSound(id) {
    let player = document.getElementById(id);

    player.volume = 0.5;
    player.currentTime = 0;
    player.play();
}