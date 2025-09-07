document.addEventListener("DOMContentLoaded", () => {
    const inputFilial = document.getElementById("Filial");
    const inputFilialId = document.getElementById("FilialId");
    const filialsList = document.getElementById("filialsList")
    const searchFilialsButton = document.getElementById("searchFilialsButton");
    const searchFilialInput = document.getElementById("searchFilialInput");
    const modalEl = document.getElementById("searchFilialModal");
    const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);

    if (!inputFilial || !inputFilialId || !filialsList) return;

    searchFilialsButton.addEventListener("click", () => {
        fetch("/Collect/GetFilials")
        .then((response) => {
            return response.json();
        })
        .then((filials) => {
            console.log(filials);
            filialsList.textContent = "";
            filials.forEach((filial) => {
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
                newButton.className = "buttonSelectFilial p-0 border-0 bg-transparent";
                newButton.dataset.id = filial.id;
                newButton.dataset.name = filial.name;

                newTableData1.textContent = filial.id;
                newTableData2.textContent = filial.name;
                newTableData3.appendChild(newButton);
                newTableData3.classList.add("text-end");

                newTableRow.appendChild(newTableData1);
                newTableRow.appendChild(newTableData2);
                newTableRow.appendChild(newTableData3);

                filialsList.appendChild(newTableRow);
            })
        })
    });

    searchFilialInput.addEventListener("input", () => {
        if (searchFilialInput.value.trim().length == 0) return;

        fetch("/Collect/FilterFilialsList", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ input: searchFilialInput.value })
        })
        .then((response) => {
            return response.json();
        })
        .then((filials) => {
            console.log(filials);
            filialsList.textContent = "";
            filials.forEach((filial) => {
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
                newButton.className = "buttonSelectFilial p-0 border-0 bg-transparent";
                newButton.dataset.id = filial.id;
                newButton.dataset.name = filial.name;

                newTableData1.textContent = filial.id;
                newTableData2.textContent = filial.name;
                newTableData3.appendChild(newButton);
                newTableData3.classList.add("text-end");

                newTableRow.appendChild(newTableData1);
                newTableRow.appendChild(newTableData2);
                newTableRow.appendChild(newTableData3);

                filialsList.appendChild(newTableRow);
            })
        })
    });

    filialsList.addEventListener("click", (e) => {
        const button = e.target.closest(".buttonSelectFilial");
        if (button){
            inputFilial.value = button.dataset.name;
            inputFilialId.value = button.dataset.id;
            modal.hide();
        }
    });
})