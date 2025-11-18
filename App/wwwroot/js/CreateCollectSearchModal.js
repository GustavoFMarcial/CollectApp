document.addEventListener("DOMContentLoaded", () => {
    const modalEl = document.getElementById("CreateCollectSearchEntitiesModal");

    if (!modalEl) return;

    const modalLabel = document.getElementById("CreateCollectSearchModal");
    const entityDescription = document.getElementById("entity-th-description");

    const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);

    const searchEntitiesButton = document.querySelectorAll(".search-entity-button");

    const entitiesList = document.getElementById("entitiesSearchedList");
    const searchEntitiesInput = document.getElementById("searchEntitiesInput");
    const searchEntities = document.getElementById("searchEntitiesButton");

    const inputProduct = document.getElementById("Product");
    const inputProductId = document.getElementById("ProductId");

    const inputFilial = document.getElementById("Filial");
    const inputFilialId = document.getElementById("FilialId");

    const inputSupplier = document.getElementById("Supplier");
    const inputSupplierId = document.getElementById("SupplierId");

    const ulButtonsPagination = document.getElementById("entitiesSearchedButtonsPagination");

    let currentEntity = "";

    if (!inputProduct || !inputProductId || !inputFilial || !inputFilialId || !inputSupplier || !inputSupplierId || !entitiesList) return;

    function createProductRow(product) {
        const row = document.createElement("tr");

        const tdId = document.createElement("td");
        tdId.textContent = product.id;

        const tdName = document.createElement("td");
        tdName.textContent = product.name;

        const tdAction = document.createElement("td");
        tdAction.classList.add("text-end");

        const button = document.createElement("button");
        button.className = "buttonSelectEntity p-0 border-0 bg-transparent";
        button.dataset.id = product.id;
        button.dataset.name = product.name;

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

    function renderEntities(entities) {
        entitiesList.textContent = "";
        (entities.items ?? entities).forEach(p => {
            entitiesList.appendChild(createProductRow(p));
        });
    }

    function renderPaginationButtons(totalPages) {
        ulButtonsPagination.textContent = "";
        for (var i = 1; i <= totalPages.totalPages; i++) {
            ulButtonsPagination.appendChild(createPaginationButton(i, totalPages.pageNum));
        }
    }

    async function fetchEntities(url, options = {}) {
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value
            || document.querySelector('meta[name="csrf-token"]')?.getAttribute('content');

        const headers = {
            ...options.headers,
            'RequestVerificationToken': token
        };

        const response = await fetch(url, {
            ...options,
            headers
        });

        const result = await response.json();
        console.log(result);

        if (result.success === false) {
            console.error("Erro ao buscar entidades:", result.message);
            alert(result.message);
            return { items: [] };
        }

        return result;
    }

    searchEntitiesButton.forEach(button => {
        button.addEventListener("click", async (e) => {
            searchEntitiesInput.value = "";
            const btn = e.target.closest("button");
            currentEntity = btn.value;

            if (currentEntity == "Product") {
                modalLabel.innerText = "Selecione um produto";
                entityDescription.innerText = "Produto";
            }
            else if (currentEntity == "Supplier") {
                modalLabel.innerText = "Selecione um fornecedor";
                entityDescription.innerText = "Fornecedor";
            }
            else {
                modalLabel.innerText = "Selecione uma filial";
                entityDescription.innerText = "Filial";
            }

            const input = searchEntitiesInput.value.trim();

            const response = await fetchEntities(`/${currentEntity}/List${currentEntity}sJson`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ input })
            });
            renderEntities(response.items);
            renderPaginationButtons(response);
        });
    });

    searchEntities.addEventListener("click", async () => {
        const input = searchEntitiesInput.value.trim();

        const response = await fetchEntities(`/${currentEntity}/List${currentEntity}sJson`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ input })
        });

        renderEntities(response.items);
        renderPaginationButtons(response);
    });

    ulButtonsPagination.addEventListener("click", async (e) => {
        const input = searchEntitiesInput.value.trim();
        const pageNum = e.target.value;

        if (e.target.classList == "page-link page-button") {
            const response = await fetchEntities(`/${currentEntity}/List${currentEntity}sJson`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ input, pageNum })
            });
            renderEntities(response.items);
            renderPaginationButtons(response)
        }
    });

    entitiesList.addEventListener("click", (e) => {
        const button = e.target.closest(".buttonSelectEntity");
        if (button) {
            if (currentEntity == "Product") {
                inputProduct.value = button.dataset.name;
                inputProductId.value = button.dataset.id;
            }
            else if (currentEntity == "Supplier") {
                inputSupplier.value = button.dataset.name;
                inputSupplierId.value = button.dataset.id;
            }
            else {
                inputFilial.value = button.dataset.name;
                inputFilialId.value = button.dataset.id;
            }
            modal.hide();
        }
    });
});
