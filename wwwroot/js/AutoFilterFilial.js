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
    const buttonsPagination = document.querySelectorAll(".page-button");

    if (!inputFilial || !inputFilialId || !filialsList) return;

    // 🔹 Função para criar linha da tabela
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
        button.className = "page-link page-button";
        button.value = i;
        button.textContent = i;
        // var activeClass = i == pageNum ? "custom-active" : "";
        // button.classList.add(activeClass);

        const li = document.createElement("li");
        li.classList.add("page-item")
        li.appendChild(button);

        return li;
    }

    // 🔹 Função para renderizar lista
    function renderFilials(filials) {
        filialsList.textContent = "";
        filials.items.forEach(f => filialsList.appendChild(createFilialRow(f)));
    }

    function renderPaginationButtons(totalPages) {
        ulButtonsPagination.textContent = "";
        for (var i = 1; i <= totalPages.totalPages; i++){
            ulButtonsPagination.appendChild(createPaginationButton(i, totalPages.pageNum));
        }
    }

    // 🔹 Função de fetch reutilizável
    async function fetchFilials(url, options = {}) {
        const response = await fetch(url, options);
        if (!response.ok) {
            console.error("Erro ao buscar filiais:", response.statusText);
            return { items: [] };
        }
        return response.json();
    }

    // 🔹 Buscar todos
    searchFilialsButton.addEventListener("click", async () => {
        const filials = await fetchFilials("/Filial/ListFilialsJson");
        renderFilials(filials);
        renderPaginationButtons(filials);
    });

    // 🔹 Buscar filtrados
    searchFilial.addEventListener("click", async () => {
        const input = searchFilialInput.value.trim();
        // if (!input) return;

        const filials = await fetchFilials("/Filial/FilterFilialsListJson", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ input })
        });
        renderFilials(filials);
        renderPaginationButtons(filials);
    });

    // buttonsPagination.forEach(b => b.addEventListener("click", async (e) => {
    //     console.log(e.target.value);
    //     console.log(e.target.textContent);
    // }))

    ulButtonsPagination.addEventListener("click", async (e) => {
        const input = searchFilialInput.value.trim();
        const pageNum = e.target.value;

        if (e.target.classList == "page-link page-button"){
            console.log(e.target.value);
            console.log(e.target.textContent);

            const filials = await fetchFilials("/Filial/FilterFilialsListJson", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ input, pageNum })
            });
            renderFilials(filials);
            renderPaginationButtons(filials)
        }
    })

    // 🔹 Selecionar filial
    filialsList.addEventListener("click", (e) => {
        const button = e.target.closest(".buttonSelectFilial");
        if (button) {
            inputFilial.value = button.dataset.name;
            inputFilialId.value = button.dataset.id;
            modal.hide();
        }
    });
});
