export function handleMusic(id) {
    let player = document.getElementById(id);

    if (player.paused) {
        player.play();
        return true;
    } else {
        player.currentTime = 0;
        player.pause();
        return false;
    }

}