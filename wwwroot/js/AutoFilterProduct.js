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