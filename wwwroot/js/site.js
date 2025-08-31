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
    const listSuppliers = document.getElementById("listSuppliers");
    const itemSupplier = document.getElementById("itemSupplier");

    if (!inputSupplier || !listSuppliers || itemSupplier) return;

    const showList = () => listSuppliers.classList.remove("d-none");
    const hideList = () => listSuppliers.classList.add("d-none");

    inputSupplier.addEventListener("input", () => {
        if (inputSupplier.value.trim().length == 0) return;

        fetch("/Collect/FilterSuppliersList", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ input: inputSupplier.value })
        })
        .then((response) => {
            return response.json();
        })
        .then((suppliers) => {
            console.log(suppliers);
            listSuppliers.textContent = "";
            suppliers.forEach((supplier) => {
                var newListItem = document.createElement("li");
                newListItem.textContent = supplier.name;
                newListItem.dataset.id = supplier.id;
                newListItem.classList.add("itemSupplier");
                listSuppliers.appendChild(newListItem);
            })
        })
    });

    inputSupplier.addEventListener("focus", () => {
        showList();
    });

    listSuppliers.addEventListener("click", (e) => {
        if (e.target.classList.contains("itemSupplier")) {
            inputSupplier.value = e.target.textContent;
            inputSupplierId.value = e.target.dataset.id;
            hideList();
        }
    });

    document.addEventListener("click", (e) => {
        if (!listSuppliers.contains(e.target) && e.target !== inputSupplier){
            hideList();
        }
    })
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