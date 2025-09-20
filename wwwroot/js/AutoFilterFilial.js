document.addEventListener("DOMContentLoaded", () => {
    const inputFilial = document.getElementById("Filial");
    const inputFilialId = document.getElementById("FilialId");
    const filialsList = document.getElementById("filialsList");
    const searchFilialsButton = document.getElementById("searchFilialsButton");
    const searchFilialInput = document.getElementById("searchFilialInput");
    const searchFilial = document.getElementById("searchFilial");
    const modalEl = document.getElementById("searchFilialModal");
    const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
    const ulButtonsPagination = document.getElementById("filialButtonsPagination");

    if (!inputFilial || !inputFilialId || !filialsList) return;

    function createFilialRow(filial) {
        const row = document.createElement("tr");

        const tdId = document.createElement("td");
        tdId.textContent = filial.id;

        const tdName = document.createElement("td");
        tdName.textContent = filial.name;

        const tdAction = document.createElement("td");
        tdAction.classList.add("text-end");

        const button = document.createElement("button");
        button.className = "buttonSelectFilial p-0 border-0 bg-transparent";
        button.dataset.id = filial.id;
        button.dataset.name = filial.name;

        const img = document.createElement("img");
        img.src = "/assets/img/check-icon.svg";
        img.alt = "Check icon";
        img.width = 24;
        img.height = 24;

        button.appendChild(img);
        tdAction.appendChild(button);

        row.appendChild(tdId);
        row.appendChild(tdName);
        row.appendChild(tdAction);

        return row;
    }

    function createPaginationButton(i, pageNum) {
        const button = document.createElement("button");
        button.classList.add("page-link", "page-button");

        if (i === pageNum) {
        button.classList.add("custom-active");
        }
        
        button.value = i;
        button.textContent = i;

        const li = document.createElement("li");
        li.classList.add("page-item")
        li.appendChild(button);

        return li;
    }

    function renderFilials(filials) {
        filialsList.textContent = "";
        (filials.items ?? filials).forEach(f => {
            filialsList.appendChild(createFilialRow(f));
        });
    }

    function renderPaginationButtons(totalPages) {
        ulButtonsPagination.textContent = "";
        for (var i = 1; i <= totalPages.totalPages; i++){
            ulButtonsPagination.appendChild(createPaginationButton(i, totalPages.pageNum));
        }
    }

    async function fetchFilials(url, options = {}) {
        const response = await fetch(url, options);
        if (!response.ok) {
            console.error("Erro ao buscar filiais:", response.statusText);
            return { items: [] };
        }
        return response.json();
    }

    searchFilialsButton.addEventListener("click", async () => {
        const input = searchFilialInput.value.trim();

        const filials = await fetchFilials("/Filial/ListFilialsJson", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ input })
        });
        renderFilials(filials);
        renderPaginationButtons(filials);
    });

    searchFilial.addEventListener("click", async () => {
        const input = searchFilialInput.value.trim();

        const filials = await fetchFilials("/Filial/ListFilialsJson", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ input })
        });
        renderFilials(filials);
        renderPaginationButtons(filials);
    });

    ulButtonsPagination.addEventListener("click", async (e) => {
        const input = searchFilialInput.value.trim();
        const pageNum = e.target.value;

        if (e.target.classList == "page-link page-button"){
            const filials = await fetchFilials("/Filial/ListFilialsJson", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ input, pageNum })
            });
            renderFilials(filials);
            renderPaginationButtons(filials)
        }
    });

    filialsList.addEventListener("click", (e) => {
        const button = e.target.closest(".buttonSelectFilial");
        if (button) {
            inputFilial.value = button.dataset.name;
            inputFilialId.value = button.dataset.id;
            modal.hide();
        }
    });
});
