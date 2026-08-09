function setActiveButton(buttons, activeButton) {
    buttons.forEach((button) => button.classList.toggle("is-active", button === activeButton));
}

function renderScanResult(mode) {
    const result = document.getElementById("scan-result");
    if (!result) {
        return;
    }

    if (mode === "twod") {
        result.innerHTML = [
            '<span class="result-label">Product identity</span>',
            "<strong>Bold Dark Roast Coffee</strong>",
            "<p>GTIN: 00012345678905<br>Lot: 2027-C<br>Best before: 2027-12-15<br>Status: Active</p>",
            "<p>Resources: product information, ingredients, allergens, preparation instructions, recycling, manufacturer.</p>"
        ].join("");
        return;
    }

    result.innerHTML = [
        '<span class="result-label">GTIN</span>',
        "<strong>012345678905</strong>",
        "<p>Database lookup: Bold Dark Roast Coffee, $12.99.</p>"
    ].join("");
}

function wireScanDemo() {
    const buttons = Array.from(document.querySelectorAll(".scan-trigger"));
    buttons.forEach((button) => {
        button.addEventListener("click", () => {
            setActiveButton(buttons, button);
            renderScanResult(button.dataset.scan);
        });
    });
}

function wireRecallDemo() {
    const button = document.getElementById("simulate-recall");
    const lot = document.getElementById("recall-lot");
    const result = document.getElementById("recall-result");

    if (!button || !lot || !result) {
        return;
    }

    button.addEventListener("click", () => {
        const recalled = !lot.classList.contains("is-recalled");
        lot.classList.toggle("is-recalled", recalled);
        lot.innerHTML = recalled ? "Lot 2027-C <strong>Recalled</strong>" : "Lot 2027-C <strong>Active</strong>";
        button.textContent = recalled ? "Reset Lot" : "Simulate Recall";
        result.classList.toggle("notice-card--critical", recalled);
        result.textContent = recalled
            ? "Product notice: this demonstration lot has been marked recalled. The printed QR did not change."
            : "Lot 2027-C is currently active in this demonstration registry.";
    });
}

function wireExpirationDemo() {
    const buttons = Array.from(document.querySelectorAll(".expiry-trigger"));
    const result = document.getElementById("expiry-result");

    if (!buttons.length || !result) {
        return;
    }

    const messages = {
        sale: { className: "notice-card--ok", text: "Expires 2028. Status: Sale." },
        markdown: { className: "notice-card--warn", text: "Near expiration. Status: Markdown." },
        expired: { className: "notice-card--critical", text: "Expired. Status: Do not sell." }
    };

    buttons.forEach((button) => {
        button.addEventListener("click", () => {
            setActiveButton(buttons, button);
            const state = messages[button.dataset.expiry] || messages.sale;
            result.classList.remove("notice-card--ok", "notice-card--warn", "notice-card--critical");
            result.classList.add(state.className);
            result.textContent = state.text;
        });
    });
}

function parseInspectorPayload(value) {
    const trimmed = value.trim();
    const output = {
        symbol: trimmed.startsWith("http") ? "QR Code" : "UPC-A or raw value",
        raw: trimmed || "None",
        scheme: "None",
        host: "None",
        gtin: "None",
        lot: "None",
        digitalLink: "No"
    };

    if (!trimmed) {
        return output;
    }

    const numeric = trimmed.match(/^\d{8,14}$/);
    if (numeric) {
        output.gtin = trimmed;
        return output;
    }

    try {
        const url = new URL(trimmed);
        output.scheme = url.protocol.replace(":", "").toUpperCase();
        output.host = url.host;
        const gtinMatch = url.pathname.match(/\/01\/(\d{8,14})/);
        const lotMatch = url.pathname.match(/\/10\/([^/?#]+)/);

        if (gtinMatch) {
            output.gtin = gtinMatch[1];
            output.digitalLink = "Yes";
        }

        if (lotMatch) {
            output.lot = decodeURIComponent(lotMatch[1]);
        }
    } catch {
        output.symbol = "Raw payload";
    }

    return output;
}

function escapeHtml(value) {
    return String(value)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function renderInspector() {
    const input = document.getElementById("inspector-input");
    const output = document.getElementById("inspector-output");

    if (!input || !output) {
        return;
    }

    const parsed = parseInspectorPayload(input.value);
    output.innerHTML = [
        "<dl>",
        `<dt>Symbol</dt><dd>${escapeHtml(parsed.symbol)}</dd>`,
        `<dt>Raw content</dt><dd><code>${escapeHtml(parsed.raw)}</code></dd>`,
        `<dt>Scheme</dt><dd>${escapeHtml(parsed.scheme)}</dd>`,
        `<dt>Host</dt><dd>${escapeHtml(parsed.host)}</dd>`,
        `<dt>GTIN</dt><dd>${escapeHtml(parsed.gtin)}</dd>`,
        `<dt>Lot</dt><dd>${escapeHtml(parsed.lot)}</dd>`,
        `<dt>Digital Link</dt><dd>${escapeHtml(parsed.digitalLink)}</dd>`,
        "</dl>"
    ].join("");
}

function wireInspector() {
    const input = document.getElementById("inspector-input");
    if (!input) {
        return;
    }

    input.addEventListener("input", renderInspector);
    renderInspector();
}

document.addEventListener("DOMContentLoaded", () => {
    wireScanDemo();
    wireRecallDemo();
    wireExpirationDemo();
    wireInspector();
});
