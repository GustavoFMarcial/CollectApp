document.addEventListener("DOMContentLoaded", () => {
    document.body.addEventListener("click", async (e) => {
        const button = e.target.closest(".log-list-button");
        const logList = document.getElementById("log-list");

        async function fetchLogs(url) {
            const response = await fetch(url);

            return response.json();
        }

        function createLogRow(logs) {
            logList.textContent = "";
            for (let i = 0; i < logs.length; i++) {
                const tr = document.createElement("tr");

                tr.appendChild(Object.assign(document.createElement("td"), { innerHTML: logs[i].field }));
                tr.appendChild(Object.assign(document.createElement("td"), { innerHTML: logs[i].userName }));
                tr.appendChild(Object.assign(document.createElement("td"), { innerHTML: logs[i].changedAt }));
                tr.appendChild(Object.assign(document.createElement("td"), { innerHTML: logs[i].oldValue }));
                tr.appendChild(Object.assign(document.createElement("td"), { innerHTML: logs[i].newValue }));

                logList.appendChild(tr);
            }
        }

        if (button) {
            const entity = button.dataset.entity;
            const id = button.dataset.id;

            const logs = await fetchLogs(`/AuditLog/GetLogs?entityName=${entity}&entityId=${id}`);
            console.log(logs);
            createLogRow(logs)

            const modalEl = document.getElementById("logsModal");
            if (modalEl) {
                const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
                modal.show();
            }
        }
    });
});