document.addEventListener("DOMContentLoaded", function () {
    // Retrieve quiz score from local storage
    let score = localStorage.getItem("quizScore");

    // Define major based on score
    let specialty = "";
    let description = "";
    let imageSrc = "";

    if (score >= 80) {
        specialty = "Engineering";
        description = "You have strong analytical and problem-solving skills. Engineering is a great fit for you!";
        imageSrc = "/img/engineering.jpg";
    } else if (score >= 60) {
        specialty = "Computer Science";
        description = "You enjoy technology and problem-solving. Computer Science is an ideal choice!";
        imageSrc = "/img/computer-science.jpg";
    } else if (score >= 40) {
        specialty = "Business Administration";
        description = "You have leadership and strategic thinking skills. Business Administration is a great path!";
        imageSrc = "/img/business.jpg";
    } else {
        specialty = "Creative Arts";
        description = "You have a creative mindset and an artistic approach. The Arts and Design are perfect for you!";
        imageSrc = "/img/result.jpeg";
    }

    // Display result on the page
    document.getElementById("result-specialty").textContent = specialty;
    document.getElementById("result-description").textContent = description;
    document.getElementById("result-image").src = imageSrc;
});


function submitQuiz() {
    let userScore = calculateScore(); // Function to calculate score

    // Store the result in local storage
    localStorage.setItem("quizScore", userScore);

    // Redirect to the result page
    window.location.href = "result.html";
}
