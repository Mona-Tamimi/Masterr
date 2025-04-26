function openEditModal() {
    document.getElementById("editProfileModal").style.display = "block";

    // Pre-fill fields with existing data
    document.getElementById("editName").value = document.getElementById("displayName").innerText;
    document.getElementById("editEmail").value = document.getElementById("displayEmail").innerText;
}

function closeEditModal() {
    document.getElementById("editProfileModal").style.display = "none";
}

document.getElementById("profileImageUpload").addEventListener("change", function (event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById("profilePic").src = e.target.result;
        };
        reader.readAsDataURL(file);
    }
});

function saveProfile() {
    let newName = document.getElementById("editName").value.trim();
    let newEmail = document.getElementById("editEmail").value.trim();

    if (newName.length < 3) {
        document.getElementById("nameError").innerText = "Name must be at least 3 characters";
        return;
    } else {
        document.getElementById("nameError").innerText = "";
    }

    if (!newEmail.includes("@") || !newEmail.includes(".")) {
        document.getElementById("emailError").innerText = "Enter a valid email";
        return;
    } else {
        document.getElementById("emailError").innerText = "";
    }

    // Update profile info
    document.getElementById("displayName").innerText = newName;
    document.getElementById("displayEmail").innerText = newEmail;

    closeEditModal();
    alert("Profile Updated Successfully!");
}
