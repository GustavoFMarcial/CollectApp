document.addEventListener("DOMContentLoaded", () => {
    const inputSupplier = document.getElementById("Supplier");
    const inputSupplierId = document.getElementById("SupplierId");
    const suppliersList = document.getElementById("suppliersList");
    const searchSuppliersButton = document.getElementById("searchSuppliersButton");
    const searchSupplierInput = document.getElementById("searchSupplierInput");
    const searchSupplier = document.getElementById("searchSupplier");
    const modalEl = document.getElementById("searchSupplierModal");

    if (!modalEl) return;

    const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
    const ulButtonsPagination = document.getElementById("supplierButtonsPagination");

    if (!inputSupplier || !inputSupplierId || !suppliersList) return;

    function createSupplierRow(supplier) {
        const row = document.createElement("tr");

        const tdId = document.createElement("td");
        tdId.textContent = supplier.id;

        const tdName = document.createElement("td");
        tdName.textContent = supplier.name;

        const tdAction = document.createElement("td");
        tdAction.classList.add("text-end");

        const button = document.createElement("button");
        button.className = "buttonSelectSupplier p-0 border-0 bg-transparent";
        button.dataset.id = supplier.id;
        button.dataset.name = supplier.name;

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

    function renderSuppliers(suppliers) {
        suppliersList.textContent = "";
        (suppliers.items ?? suppliers).forEach(s => {
            suppliersList.appendChild(createSupplierRow(s));
        });
    }

    function renderPaginationButtons(totalPages) {
        ulButtonsPagination.textContent = "";
        for (var i = 1; i <= totalPages.totalPages; i++) {
            ulButtonsPagination.appendChild(createPaginationButton(i, totalPages.pageNum));
        }
    }

    async function fetchSuppliers(url, options = {}) {
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value
            || document.querySelector('meta[name="csrf-token"]')?.getAttribute('content');

        const headers = {
            ...options.headers,
            'RequestVerificationToken': token
        };

        const response = await fetch(url, {...options, headers});

        if (!response.ok) {
            console.error("Erro ao buscar fornecedores:", response.statusText);
            return { items: [] };
        }

        return response.json();
    }

    searchSuppliersButton.addEventListener("click", async () => {
        const input = searchSupplierInput.value.trim();

        const suppliers = await fetchSuppliers("/Supplier/ListSuppliersJson", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ input })
        });
        renderSuppliers(suppliers);
        renderPaginationButtons(suppliers);
    });

    searchSupplier.addEventListener("click", async () => {
        const input = searchSupplierInput.value.trim();

        const suppliers = await fetchSuppliers("/Supplier/ListSuppliersJson", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ input })
        });
        renderSuppliers(suppliers);
        renderPaginationButtons(suppliers);
    });

    ulButtonsPagination.addEventListener("click", async (e) => {
        const input = searchSupplierInput.value.trim();
        const pageNum = e.target.value;

        if (e.target.classList == "page-link page-button") {
            const suppliers = await fetchSuppliers("/Supplier/ListSuppliersJson", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ input, pageNum })
            });
            renderSuppliers(suppliers);
            renderPaginationButtons(suppliers)
        }
    });

    suppliersList.addEventListener("click", (e) => {
        const button = e.target.closest(".buttonSelectSupplier");
        if (button) {
            inputSupplier.value = button.dataset.name;
            inputSupplierId.value = button.dataset.id;
            modal.hide();
        }
    });
});
