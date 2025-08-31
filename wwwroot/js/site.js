// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#CNPJ').mask('00.000.000/0000-00');
    $('#ZipCode').mask('00000-000');
});

document.addEventListener("DOMContentLoaded", () => {
    const inputProduct = document.getElementById("Product");
    const listProducts = document.getElementById("listProducts");
    const itemProduct = document.getElementById("itemProduct");
    const inputProductId = document.getElementById("ProductId");
    
    if (!inputProduct || !listProducts || itemProduct) return;

    const showList = () => listProducts.classList.remove("d-none");
    const hideList = () => listProducts.classList.add("d-none");

    inputProduct.addEventListener("input", () => {
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

    inputProduct.addEventListener("blur", () => {
        setTimeout(hideList, 150);
    });

    inputProduct.addEventListener("click", () => {
        showList();
    });

    listProducts.addEventListener("click", (e) => {
        if (e.target.classList.contains("itemProduct")) {
            inputProduct.value = e.target.textContent;
            inputProductId.value = e.target.dataset.id;
        }
    });
})

document.addEventListener("DOMContentLoaded", () => {
    const inputSupplier = document.getElementById("Supplier");
    const listSuppliers = document.getElementById("listSuppliers");
    const itemSupplier = document.getElementById("itemSupplier");
    const inputSupplierId = document.getElementById("SupplierId");
    const items = document.querySelectorAll("#listSuppliers .itemSupplier");

    if (!inputSupplier || !listSuppliers || itemSupplier) return;

    const showList = () => listSuppliers.classList.remove("d-none");
    const hideList = () => listSuppliers.classList.add("d-none");

    // inputSupplier.addEventListener("input", () => {

    // });

    inputSupplier.addEventListener("blur", () => {
        setTimeout(hideList, 150);
    });

    inputSupplier.addEventListener("click", () => {
        showList();
    });

    items.forEach(item => {
        item.addEventListener("click", function() {
            inputSupplier.value = this.textContent;
            inputSupplierId.value = this.dataset.id;
        });
    });
})