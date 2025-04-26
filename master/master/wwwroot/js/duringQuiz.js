const steps = document.querySelectorAll('.steps .circle');
const questionText = document.getElementById('question');
const optionsContainer = document.getElementById('options');
let currentStep = 0;
const answers = {};

const questions = [
    {
        question: "How do you prefer to work or learn?",
        options: [
            "Independently, I like to focus on my own tasks and manage my time.",
            "In a Team, I enjoy collaborating with others and sharing ideas.",
            "With Hands-On Experience, I prefer practical, real-world applications of what I learn.",
            "With Detailed Instructions, I like clear guidance and structure."
        ]
    },
    {
        question: "What motivates you the most in your work or studies?",
        options: [
            "Achieving personal goals.",
            "Collaborating with a great team.",
            "Learning practical skills.",
            "Receiving clear and constructive feedback."
        ]
    },
    {
        question: "How do you handle challenges or obstacles?",
        options: [
            "I work independently to find solutions.",
            "I brainstorm ideas with others.",
            "I try hands-on methods to resolve the issue.",
            "I follow clear guidance and steps."
        ]
    },
    {
        question: "What kind of environment helps you be the most productive?",
        options: [
            "A quiet and focused environment.",
            "A collaborative and energetic environment.",
            "An environment with tools and resources for hands-on work.",
            "An environment with clear rules and structure."
        ]
    },
    {
        question: "What is your preferred method of receiving feedback?",
        options: [
            "Self-assessment and reflection.",
            "Group discussions and input from peers.",
            "Hands-on examples to improve my work.",
            "Detailed and structured feedback from a mentor."
        ]
    }
];

function updateQuestion() {
    const currentQuestion = questions[currentStep];
    questionText.textContent = currentQuestion.question;

    optionsContainer.innerHTML = "";
    currentQuestion.options.forEach((optionText, index) => {
        const optionDiv = document.createElement('div');
        optionDiv.className = 'option';

        const input = document.createElement('input');
        input.type = 'radio';
        input.id = `option${index + 1}`;
        input.name = 'preference';
        input.checked = answers[currentStep] === optionText;

        const label = document.createElement('label');
        label.htmlFor = `option${index + 1}`;
        label.textContent = optionText;

        optionDiv.appendChild(input);
        optionDiv.appendChild(label);
        optionsContainer.appendChild(optionDiv);

        input.addEventListener('change', () => {
            answers[currentStep] = optionText;
            steps[currentStep].classList.add('answered');
        });
    });
}

steps.forEach((circle, index) => {
    circle.addEventListener('click', () => {
        steps[currentStep].classList.remove('active');
        currentStep = index;
        steps[currentStep].classList.add('active');
        updateQuestion();
    });
});

document.getElementById('back').addEventListener('click', () => {
    if (currentStep > 0) {
        steps[currentStep].classList.remove('active');
        currentStep--;
        steps[currentStep].classList.add('active');
        updateQuestion();
    }
});

document.getElementById('next').addEventListener('click', () => {
    const isAnswered = Array.from(document.getElementsByName('preference')).some(input => input.checked);

    if (isAnswered) {
        steps[currentStep].classList.add('answered');
    }

    if (currentStep < steps.length - 1) {
        steps[currentStep].classList.remove('active');
        currentStep++;
        steps[currentStep].classList.add('active');
        updateQuestion();
    }
});

updateQuestion();