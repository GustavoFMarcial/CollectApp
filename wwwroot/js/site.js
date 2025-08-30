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
    const items = document.querySelectorAll("#listProducts .itemProduct");

    if (!inputProduct || !listProducts || itemProduct) return;

    const showList = () => listProducts.classList.remove("d-none");
    const hideList = () => listProducts.classList.add("d-none");

    const hasValue = () => inputProduct.value.trim().length > 0;

    inputProduct.addEventListener("input", () => {
        hasValue() ? showList() : hideList();
    });

    inputProduct.addEventListener("blur", () => {
        setTimeout(hideList, 150);
    });

    inputProduct.addEventListener("click", () => {
        if (hasValue()) showList();
    });

    items.forEach(item => {
        item.addEventListener("click", function() {
            inputProduct.value = this.textContent;
            inputProductId.value = this.dataset.id;
        });
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

    const hasValue = () => inputSupplier.value.trim().length > 0;

    inputSupplier.addEventListener("input", () => {
        hasValue() ? showList() : hideList();
    });

    inputSupplier.addEventListener("blur", () => {
        setTimeout(hideList, 150);
    });

    inputSupplier.addEventListener("click", () => {
        if (hasValue()) showList();
    });

    items.forEach(item => {
        item.addEventListener("click", function() {
            inputSupplier.value = this.textContent;
            inputSupplierId.value = this.dataset.id;
        });
    });
})