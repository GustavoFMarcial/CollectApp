document.addEventListener("DOMContentLoaded", () => {
    const logList = document.getElementById("log-list");
    const logPaginationButtos = document.getElementById("logButtonsPagination");

    document.body.addEventListener("click", async (e) => {
        const button = e.target.closest(".log-list-button");

        async function fetchLogs(url) {
            const response = await fetch(url);

            return response.json();
        }

        function createLogRow(logs) {
            logList.textContent = "";
            for (let i = 0; i < logs.items.length; i++) {
                const tr = document.createElement("tr");

                tr.appendChild(Object.assign(document.createElement("td"), { innerHTML: logs.items[i].field }));
                tr.appendChild(Object.assign(document.createElement("td"), { innerHTML: logs.items[i].userName }));
                tr.appendChild(Object.assign(document.createElement("td"), { innerHTML: logs.items[i].changedAt }));
                tr.appendChild(Object.assign(document.createElement("td"), { innerHTML: logs.items[i].oldValue }));
                tr.appendChild(Object.assign(document.createElement("td"), { innerHTML: logs.items[i].newValue }));

                logList.appendChild(tr);
            }
        }

        function createPaginationButtons(totalPages){
            for (let i = 1; i <= totalPages.totalPages; i++){
                const li = document.createElement("li");
                const button = document.createElement("button");

                button.value = i;
                button.innerHTML = i;
                button.classList.add("page-link", "page-button");
                li.classList.add("page-item");

                li.appendChild(button);
                logPaginationButtos.appendChild(li);
            }
        }

        if (button) {
            const entity = button.dataset.entity;
            const id = button.dataset.id;

            const response = await fetchLogs(`/AuditLog/GetLogs?entityName=${entity}&entityId=${id}`);
            console.log(response);
            createLogRow(response);
            createPaginationButtons(response);

            const modalEl = document.getElementById("logsModal");
            if (modalEl) {
                const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
                modal.show();
            }
        }
    });
});