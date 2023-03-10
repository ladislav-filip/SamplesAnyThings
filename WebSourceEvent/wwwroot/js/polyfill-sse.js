function PolyfillSseStart(containerId, endpoint, jwtToken) {
    const source = new EventSourcePolyfill(endpoint, {
        headers: {
            'authorization': 'Bearer ' + jwtToken
        },
        withCredentials: true
    });
    const dtView = document.getElementById(containerId);

    source.onmessage = function (event) {
        let divEl = document.createElement("div");
        var txEl = document.createElement("span");
        txEl.className = "me-3";
        txEl.textContent = event.lastEventId;
        divEl.appendChild(txEl);
        txEl = document.createElement("span");
        txEl.textContent = event.data;
        divEl.appendChild(txEl);
        dtView.appendChild(divEl);
    };

    source.addEventListener("info", (event) => {
        let divEl = document.createElement("div");
        divEl.className = "text-light bg-dark";
        var txEl = document.createElement("span");
        txEl.className = "me-3";
        txEl.textContent = event.lastEventId;
        divEl.appendChild(txEl);
        txEl = document.createElement("span");
        txEl.textContent = event.data;
        divEl.appendChild(txEl);
        dtView.appendChild(divEl);
    })
}