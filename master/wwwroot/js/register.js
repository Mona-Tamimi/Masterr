// First Name Validation
function ValidateFN() {
    const fname = document.getElementById("firstName").value;
    const fnamePattern = /^[a-zA-Z]{3,15}$/;
    document.getElementById("Rfname").innerText =
        fnamePattern.test(fname) ? "" : "Invalid First Name. Must be 3-15 letters.";
}

// Last Name Validation
function ValidateLN() {
    const lname = document.getElementById("lastName").value;
    const lnamePattern = /^[a-zA-Z]{3,15}$/;
    document.getElementById("Rlname").innerText =
        lnamePattern.test(lname) ? "" : "Invalid Last Name. Must be 3-15 letters.";
}


// Email Validation
function ValidateE() {
    const email = document.getElementById("email").value;
    const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    document.getElementById("Remail").innerText =
        emailPattern.test(email) ? "" : "Invalid Email Address.";
}

// Password Validation
function ValidateP() {
    const password = document.getElementById("password").value;
    const passwordPattern = /^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#$%^&*.]).{8,}$/;
    document.getElementById("Rpassword").innerText =
        passwordPattern.test(password)
            ? ""
            : "Invalid Password. Must be 8+ characters, include 1 number, and 1 special char.";
}

// Confirm Password Validation
function ValidateCP() {
    const password = document.getElementById("password").value;
    const cpassword = document.getElementById("repeatPassword").value;
    document.getElementById("Rcpassword").innerText =
        password === cpassword ? "" : "Passwords do not match.";
}

document.getElementById('registerForm').addEventListener('submit', function (event) {
    event.preventDefault(); // Prevent form from submitting normally

    // Get form values
    const firstName = document.getElementById('firstName').value;
    const lastName = document.getElementById('lastName').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const repeatPassword = document.getElementById('repeatPassword').value;

    // Simple validation
    if (password !== repeatPassword) {
        alert('Passwords do not match!');
        return;
    }

    // Create user object
    const user = {
        firstName: firstName,
        lastName: lastName,
        email: email,
        password: password
    };

    // Save user to local storage
    localStorage.setItem('user', JSON.stringify(user));

    // Inform the user
    alert('Registration successful!');

    // Clear the form
    document.getElementById('registerForm').reset();

    window.location.href = "login.html";

});