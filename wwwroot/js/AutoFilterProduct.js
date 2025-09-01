document.addEventListener("DOMContentLoaded", () => {
    const inputProduct = document.getElementById("Product");
    const inputProductId = document.getElementById("ProductId");
    const listProducts = document.getElementById("listProducts");
    const itemProduct = document.getElementById("itemProduct");
    
    if (!inputProduct || !listProducts || itemProduct) return;

    const showList = () => listProducts.classList.remove("d-none");
    const hideList = () => listProducts.classList.add("d-none");

    inputProduct.addEventListener("input", () => {
        if (inputProduct.value.trim().length == 0) return;

        fetch("/Collect/FilterProductsList", {
            method: "POST", 
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ input: inputProduct.value })
        })
        .then((response) =>{
            if (!response.ok){
                console.log("Erro na requisição");
            }

            return response.json();
        })
        .then((products) => {
            listProducts.innerHTML = "";
            products.forEach(item => {
                var newListItem = document.createElement("li");
                newListItem.textContent = item.description;
                newListItem.dataset.id = item.id;
                newListItem.classList.add("itemProduct");
                listProducts.appendChild(newListItem);
            })
        })
        .catch((error) => {
            console.log(error);
        })
    });

    inputProduct.addEventListener("focus", () => {
        showList();
    });

    listProducts.addEventListener("click", (e) => {
        if (e.target.classList.contains("itemProduct")) {
            inputProduct.value = e.target.textContent;
            inputProductId.value = e.target.dataset.id;
            hideList();
        }
    });

    document.addEventListener("click", (e) => {
        if (!listProducts.contains(e.target) && e.target !== inputProduct) {
            hideList();
        }
    });
})

document.addEventListener("DOMContentLoaded", () => {
    const inputProduct = document.getElementById("Product");
    const inputProductId = document.getElementById("ProductId");
    const productsList = document.getElementById("productsList")
    const searchProductsButton = document.getElementById("searchProductsButton");
    const searchProductInput = document.getElementById("searchProductInput");
    const modalEl = document.getElementById("searchProductModal");
    const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);

    if (!inputProduct || !inputProductId || !productsList) return;

    searchProductsButton.addEventListener("click", () => {
        fetch("/Collect/GetProducts")
        .then((response) => {
            return response.json();
        })
        .then((products) => {
            console.log(products);
            productsList.textContent = "";
            products.forEach((product) => {
                var newTableRow = document.createElement("tr");

                var newTableData1 = document.createElement("td");
                var newTableData2 = document.createElement("td");
                var newTableData3 = document.createElement("td");

                var newButton = document.createElement("button");

                var newCheckImg = document.createElement("img");

                newCheckImg.src = "/assets/img/check-icon.svg";
                newCheckImg.alt = "Check icon";
                newCheckImg.width = 24;
                newCheckImg.height = 24;

                newButton.appendChild(newCheckImg);
                newButton.className = "buttonSelectProduct p-0 border-0 bg-transparent";
                newButton.dataset.id = product.id;
                newButton.dataset.description = product.description;

                newTableData1.textContent = product.id;
                newTableData2.textContent = product.description;
                newTableData3.appendChild(newButton);
                newTableData3.classList.add("text-end");

                newTableRow.appendChild(newTableData1);
                newTableRow.appendChild(newTableData2);
                newTableRow.appendChild(newTableData3);

                productsList.appendChild(newTableRow);
            })
        })
    });

    searchProductInput.addEventListener("input", () => {
        if (searchProductInput.value.trim().length == 0) return;

        fetch("/Collect/FilterProductsList", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ input: searchProductInput.value })
        })
        .then((response) => {
            return response.json();
        })
        .then((products) => {
            console.log(products);
            productsList.textContent = "";
            products.forEach((product) => {
                var newTableRow = document.createElement("tr");

                var newTableData1 = document.createElement("td");
                var newTableData2 = document.createElement("td");
                var newTableData3 = document.createElement("td");

                var newButton = document.createElement("button");

                var newCheckImg = document.createElement("img");

                newCheckImg.src = "/assets/img/check-icon.svg";
                newCheckImg.alt = "Check icon";
                newCheckImg.width = 24;
                newCheckImg.height = 24;

                newButton.appendChild(newCheckImg);
                newButton.className = "buttonSelectProduct p-0 border-0 bg-transparent";
                newButton.dataset.id = product.id;
                newButton.dataset.description = product.description;

                newTableData1.textContent = product.id;
                newTableData1.dataset.id = product.id;
                newTableData2.textContent = product.description;
                newTableData3.appendChild(newButton);
                newTableData3.classList.add("text-end");

                newTableRow.appendChild(newTableData1);
                newTableRow.appendChild(newTableData2);
                newTableRow.appendChild(newTableData3);

                productsList.appendChild(newTableRow);
            })
        })
    });

    productsList.addEventListener("click", (e) => {
        const button = e.target.closest(".buttonSelectProduct");
        if (button){
            inputProduct.value = button.dataset.description;
            inputProductId.value = button.dataset.id;
            modal.hide();
        }
    });
})