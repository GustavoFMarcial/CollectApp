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
        });
    });
})