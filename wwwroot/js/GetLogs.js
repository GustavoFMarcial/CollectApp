document.addEventListener("DOMContentLoaded", () => {
    document.body.addEventListener("click", async (e) => {
        const button = e.target.closest(".log-list");
        
        if (button) {
            const entity = button.dataset.entity;
            const id = button.dataset.id;

            const logs = await fetch(`/AuditLog/GetLogs?entityName=${entity}&entityId=${id}`);

            const modalEl = document.getElementById("logsModal");
            if (modalEl) {
                const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
                modal.show();
            }
        }
    });
});