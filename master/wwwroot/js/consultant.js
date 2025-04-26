document.addEventListener('DOMContentLoaded', function () {
    const filterOptions = document.querySelectorAll('.filter-option');
    const specialistCards = document.querySelectorAll('.col-md-4');

    // Function to show all specialists
    function showAllSpecialists() {
        specialistCards.forEach(card => {
            card.style.display = 'block';
        });
    }

    // Add event listener to each filter option
    filterOptions.forEach(option => {
        option.addEventListener('click', function () {
            const filterValue = option.getAttribute('data-value');

            // Show all specialists if 'All' is selected
            if (filterValue === 'all') {
                showAllSpecialists();
            } else {
                // Filter specialists by specialty
                specialistCards.forEach(card => {
                    const cardSpecialty = card.getAttribute('data-specialty');
                    if (cardSpecialty === filterValue) {
                        card.style.display = 'block';
                    } else {
                        card.style.display = 'none';
                    }
                });
            }

            // Highlight selected filter option
            filterOptions.forEach(option => {
                option.classList.remove('selected');
            });
            option.classList.add('selected');
        });
    });

    // Set default filter to 'All' on page load
    showAllSpecialists();
    filterOptions.forEach(option => {
        if (option.getAttribute('data-value') === 'all') {
            option.classList.add('selected');
        }
    });
});



document.addEventListener("DOMContentLoaded", function () {
    const cards = document.querySelectorAll("#specialist-list .col-md-4");
    const filterOptions = document.querySelectorAll(".filter-option");
    const prevBtn = document.getElementById("prev-btn");
    const nextBtn = document.getElementById("next-btn");
    const pageInfo = document.getElementById("page-info");

    const CARDS_PER_PAGE = 6;
    let currentPage = 1;
    let filteredCards = Array.from(cards);

    // Function to display cards based on the current page
    function displayCards() {
        // Hide all cards
        cards.forEach(card => card.style.display = "none");

        // Calculate start and end indices for the current page
        const start = (currentPage - 1) * CARDS_PER_PAGE;
        const end = start + CARDS_PER_PAGE;

        // Show cards for the current page
        filteredCards.slice(start, end).forEach(card => card.style.display = "block");

        // Update pagination info
        pageInfo.textContent = `Page ${currentPage} of ${Math.ceil(filteredCards.length / CARDS_PER_PAGE)}`;

        // Enable/disable buttons
        prevBtn.disabled = currentPage === 1;
        nextBtn.disabled = currentPage === Math.ceil(filteredCards.length / CARDS_PER_PAGE);
    }

    // Function to filter cards
    function filterCards(specialty) {
        if (specialty === "all") {
            filteredCards = Array.from(cards);
        } else {
            filteredCards = Array.from(cards).filter(card => card.dataset.specialty === specialty);
        }
        currentPage = 1; // Reset to the first page
        displayCards();
    }

    // Event listener for filter options
    filterOptions.forEach(option => {
        option.addEventListener("click", function () {
            filterOptions.forEach(opt => opt.classList.remove("active"));
            this.classList.add("active");
            filterCards(this.dataset.value);
        });
    });

    // Event listeners for pagination buttons
    prevBtn.addEventListener("click", function () {
        if (currentPage > 1) {
            currentPage--;
            displayCards();
        }
    });

    nextBtn.addEventListener("click", function () {
        if (currentPage < Math.ceil(filteredCards.length / CARDS_PER_PAGE)) {
            currentPage++;
            displayCards();
        }
    });

    // Initial display
    filterCards("all"); // Default filter
});
