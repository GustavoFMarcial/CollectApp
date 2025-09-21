document.addEventListener("DOMContentLoaded", () => {
    const inputProduct = document.getElementById("Product");
    const inputProductId = document.getElementById("ProductId");
    const productsList = document.getElementById("productsList");
    const searchProductsButton = document.getElementById("searchProductsButton");
    const searchProductInput = document.getElementById("searchProductInput");
    const searchProduct = document.getElementById("searchProduct");
    const modalEl = document.getElementById("searchProductModal");
    const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
    const ulButtonsPagination = document.getElementById("productButtonsPagination");

    if (!inputProduct || !inputProductId || !productsList) return;

    function createProductRow(product) {
        const row = document.createElement("tr");

        const tdId = document.createElement("td");
        tdId.textContent = product.id;

        const tdDescription = document.createElement("td");
        tdDescription.textContent = product.description;

        const tdAction = document.createElement("td");
        tdAction.classList.add("text-end");

        const button = document.createElement("button");
        button.className = "buttonSelectProduct p-0 border-0 bg-transparent";
        button.dataset.id = product.id;
        button.dataset.description = product.description;

        const img = document.createElement("img");
        img.src = "/assets/img/check-icon.svg";
        img.alt = "Check icon";
        img.width = 24;
        img.height = 24;

        button.appendChild(img);
        tdAction.appendChild(button);

        row.appendChild(tdId);
        row.appendChild(tdDescription);
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

    function renderProducts(products) {
        productsList.textContent = "";
        (products.items ?? products).forEach(p => {
            productsList.appendChild(createProductRow(p));
        });
    }

    function renderPaginationButtons(totalPages) {
        ulButtonsPagination.textContent = "";
        for (var i = 1; i <= totalPages.totalPages; i++){
            ulButtonsPagination.appendChild(createPaginationButton(i, totalPages.pageNum));
        }
    }

    async function fetchProducts(url, options = {}) {
        const response = await fetch(url, options);
        if (!response.ok) {
            console.error("Erro ao buscar produtos:", response.statusText);
            return { items: [] };
        }
        return response.json();
    }

    searchProductsButton.addEventListener("click", async () => {
        const input = searchProductInput.value.trim();

        const products = await fetchProducts("/Product/ListProductsJson", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ input })
        });
        renderProducts(products);
        renderPaginationButtons(products);
    });

    searchProduct.addEventListener("click", async () => {
        const input = searchProductInput.value.trim();

        const products = await fetchProducts("/Product/ListProductsJson", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ input })
        });
        renderProducts(products);
        renderPaginationButtons(products);
    });

    ulButtonsPagination.addEventListener("click", async (e) => {
        const input = searchProductInput.value.trim();
        const pageNum = e.target.value;

        if (e.target.classList == "page-link page-button"){
            const products = await fetchProducts("/Product/ListProductsJson", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ input, pageNum })
            });
            renderProducts(products);
            renderPaginationButtons(products)
        }
    });

    productsList.addEventListener("click", (e) => {
        const button = e.target.closest(".buttonSelectProduct");
        if (button) {
            inputProduct.value = button.dataset.description;
            inputProductId.value = button.dataset.id;
            modal.hide();
        }
    });
});
