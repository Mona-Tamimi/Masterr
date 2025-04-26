document.addEventListener("DOMContentLoaded", function () {
    const textElement = document.getElementById("conversation-text");
    const circles = document.querySelectorAll(".circle");

    // Array of texts to cycle through
    const textOptions = [
        "Explore AI-powered career matches based on your skills.",
        "Receive personalized career growth strategies tailored to you.",
        "Discover job opportunities aligned with your passions."
    ];

    let currentIndex = 0;

    function updateText() {
        // Change text
        textElement.textContent = textOptions[currentIndex];

        // Highlight the current circle
        circles.forEach(circle => circle.style.opacity = "0.5");
        circles[currentIndex].style.opacity = "1";

        // Move to next text, loop back to 0 if at the end
        currentIndex = (currentIndex + 1) % textOptions.length;
    }

    // Start automatic text changing every 3 seconds
    setInterval(updateText, 3000);

    // Initialize the first text and circle on page load
    updateText();
});


document.addEventListener("DOMContentLoaded", function () {
    const chatContainer = document.getElementById("chat-container");
    const chatBox = document.getElementById("chat-box");
    const userInput = document.getElementById("user-input");
    const sendBtn = document.getElementById("send-btn");
    const closeChatBtn = document.getElementById("close-chat");
    const getStartedBtn = document.querySelector(".get-started");

    // Show Chat on "Get Started" Click
    getStartedBtn.addEventListener("click", () => {
        chatContainer.style.display = "block";
    });

    // Close Chat
    closeChatBtn.addEventListener("click", () => {
        chatContainer.style.display = "none";
    });

    // Handle User Input
    sendBtn.addEventListener("click", async () => {
        const message = userInput.value.trim();
        if (!message) return;

        // Display User Message
        appendMessage("You", message);
        userInput.value = "";

        // Fetch GPT-3 Response
        const response = await fetchChatGPTResponse(message);
        appendMessage("AI", response);
    });

    // Function to Append Messages to Chat
    function appendMessage(sender, text) {
        const messageElement = document.createElement("div");
        messageElement.innerHTML = `<strong>${sender}:</strong> ${text}`;
        chatBox.appendChild(messageElement);
        chatBox.scrollTop = chatBox.scrollHeight;
    }

    // Fetch OpenAI ChatGPT API Response
    async function fetchChatGPTResponse(userMessage) {
        const apiKey = "YOUR_OPENAI_API_KEY";  // Replace with your actual OpenAI API Key
        const apiUrl = "https://api.openai.com/v1/chat/completions";

        const response = await fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${apiKey}`
            },
            body: JSON.stringify({
                model: "gpt-3.5-turbo",
                messages: [{ role: "user", content: userMessage }],
                max_tokens: 100
            })
        });

        const responseData = await response.json();
        return responseData.choices[0].message.content;
    }
});
