// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#CNPJ').mask('00.000.000/0000-00');
    $('#ZipCode').mask('00000-000');
});

document.addEventListener("DOMContentLoaded", () => {
    const inputSupplier = document.getElementById("Supplier");
    const inputSupplierId = document.getElementById("SupplierId");
    const suppliersList = document.getElementById("suppliersList")
    const searchSuppliersButton = document.getElementById("searchSuppliersButton");
    const searchSupplierInput = document.getElementById("searchSupplierInput");
    const modalEl = document.getElementById("searchSupplierModal");
    const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);

    if (!inputSupplier || !inputSupplierId || !suppliersList) return;

    searchSuppliersButton.addEventListener("click", () => {
        fetch("/Collect/GetSuppliers")
        .then((response) => {
            return response.json();
        })
        .then((suppliers) => {
            console.log(suppliers);
            suppliersList.textContent = "";
            suppliers.forEach((supplier) => {
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
                newButton.className = "buttonSelectSupplier p-0 border-0 bg-transparent";
                newButton.dataset.id = supplier.id;
                newButton.dataset.name = supplier.name;

                newTableData1.textContent = supplier.id;
                newTableData1.dataset.id = supplier.id;
                newTableData2.textContent = supplier.name;
                newTableData3.appendChild(newButton);
                newTableData3.classList.add("text-end");

                newTableRow.appendChild(newTableData1);
                newTableRow.appendChild(newTableData2);
                newTableRow.appendChild(newTableData3);

                suppliersList.appendChild(newTableRow);
            })
        })
    });

    searchSupplierInput.addEventListener("input", () => {
        if (searchSupplierInput.value.trim().length == 0) return;

        fetch("/Collect/FilterSuppliersList", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ input: searchSupplierInput.value })
        })
        .then((response) => {
            return response.json();
        })
        .then((suppliers) => {
            console.log(suppliers);
            suppliersList.textContent = "";
            suppliers.forEach((supplier) => {
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
                newButton.className = "buttonSelectSupplier p-0 border-0 bg-transparent";
                newButton.dataset.id = supplier.id;
                newButton.dataset.name = supplier.name;

                newTableData1.textContent = supplier.id;
                newTableData1.dataset.id = supplier.id;
                newTableData2.textContent = supplier.name;
                newTableData3.appendChild(newButton);
                newTableData3.classList.add("text-end");

                newTableRow.appendChild(newTableData1);
                newTableRow.appendChild(newTableData2);
                newTableRow.appendChild(newTableData3);

                suppliersList.appendChild(newTableRow);
            })
        })
    });

    suppliersList.addEventListener("click", (e) => {
        const button = e.target.closest(".buttonSelectSupplier");
        if (button){
            inputSupplier.value = button.dataset.name;
            inputSupplierId.value = button.dataset.id;
            modal.hide();
        }
    });
})

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