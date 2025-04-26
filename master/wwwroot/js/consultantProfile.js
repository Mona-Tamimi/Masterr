document.addEventListener("DOMContentLoaded", function () {
    const modal = document.getElementById("booking-modal");
    const selectedTime = document.getElementById("selected-time");
    const closeModal = document.querySelector(".close");
    const confirmBtn = document.querySelector(".confirm-btn");

    document.querySelectorAll(".time-slot").forEach((slot) => {
        slot.addEventListener("click", function () {
            const time = this.textContent.trim(); // Get the time slot text
            selectedTime.textContent = `You selected ${time}`;
            modal.style.display = "flex"; // Ensure modal is displayed as a flex container
        });
    });

    closeModal.addEventListener("click", function () {
        modal.style.display = "none";
    });

    confirmBtn.addEventListener("click", function () {
        alert("Your appointment is confirmed!");
        modal.style.display = "none";
    });

    window.addEventListener("click", function (event) {
        if (event.target === modal) {
            modal.style.display = "none";
        }
    });
});
