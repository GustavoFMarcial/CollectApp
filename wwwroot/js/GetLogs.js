document.addEventListener("DOMContentLoaded", () => {
    const logList = document.getElementById("log-list");
    const logPaginationButtonsUl = document.getElementById("logButtonsPagination");

    document.body.addEventListener("click", async (e) => {
        const logButton = e.target.closest(".log-list-button");

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

        function createPaginationButtons(totalPages) {
            logPaginationButtonsUl.textContent = "";
            for (let i = 1; i <= totalPages.totalPages; i++) {
                const li = document.createElement("li");
                const button = document.createElement("button");

                if (i == totalPages.pageNum){
                    button.classList.add("custom-active");
                }

                button.type = "button";
                button.value = i;
                button.innerHTML = i;
                button.classList.add("page-link", "page-log-button", "pagination-log-button");
                li.classList.add("page-item");

                li.appendChild(button);
                logPaginationButtonsUl.appendChild(li);
            }
        }

        if (logButton) {
            const entity = logButton.dataset.entity;
            const id = logButton.dataset.id;

            const response = await fetchLogs(`/AuditLog/GetLogs?entityName=${entity}&entityId=${id}`);
            createLogRow(response);
            createPaginationButtons(response);

            const modalEl = document.getElementById("logsModal");
            modalEl.dataset.entity = entity;
            modalEl.dataset.id = id;

            const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
            modal.show();
        }

        const logPaginationButton = e.target.closest(".pagination-log-button");
        if (logPaginationButton) {
            e.preventDefault(); // impede reload

            const modalEl = document.getElementById("logsModal");
            const entity = modalEl.dataset.entity;
            const id = modalEl.dataset.id;

            const response = await fetchLogs(`/AuditLog/GetLogs?entityName=${entity}&entityId=${id}&pageNum=${logPaginationButton.value}`);
            createLogRow(response);
            createPaginationButtons(response);
        }
    });
});