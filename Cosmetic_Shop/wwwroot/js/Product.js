document.addEventListener("DOMContentLoaded", function () {
    const urlParams = new URLSearchParams(window.location.search);
    const productId = parseInt(urlParams.get("productId"));
    const userId = parseInt(document.getElementById("user-id")?.value || urlParams.get("userId"));

    const image = urlParams.get("image");
    const name = urlParams.get("name");
    const description = urlParams.get("description");
    const price = urlParams.get("price");

    if (!productId || isNaN(productId)) {
        alert("❌ Invalid or missing product ID.");
        return;
    }

    document.getElementById("product-image").src = image;
    document.getElementById("product-name").textContent = name;
    document.getElementById("product-description").textContent = description;
    document.getElementById("product-price").textContent = `$${price}`;
    document.getElementById("product-id").value = productId;
    document.getElementById("user-id").value = userId;

    const token = document.querySelector("input[name='__RequestVerificationToken']")?.value || "";

    // REVIEW SECTION
    const reviewInput = document.getElementById("review-input");
    const reviewsContainer = document.getElementById("reviews-container");
    const submitReviewButton = document.getElementById("submit-review");

    submitReviewButton.addEventListener("click", async function () {
        const reviewContent = reviewInput.value.trim();
        if (!reviewContent || !userId) {
            alert("❌ Review content or user missing.");
            return;
        }

        const response = await fetch("/Reviews/Create", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": token
            },
            body: JSON.stringify({
                Content: reviewContent,
                ProductId: productId,
                UserId: userId
            })
        });

        const result = await response.json();
        if (result.success) {
            reviewInput.value = "";
            loadReviews();
        } else {
            alert("❌ Failed to submit review.");
        }
    });

    async function loadReviews() {
        try {
            const response = await fetch(`/Reviews/GetReviews?productId=${productId}`);
            const reviews = await response.json();
            reviewsContainer.innerHTML = "";

            if (reviews.length === 0) {
                reviewsContainer.innerHTML = "<p>No reviews yet. Be the first to review!</p>";
                return;
            }

            reviews.forEach(review => {

                const currentUserId = userId;
                const reviewElement = document.createElement("div");
                reviewElement.classList.add("review");

                const profilePic = review.profilePicture || "/images/default-profile.png";
                const userName = review.name || "Unknown user";

                let actionButtons = '';
                if (review.userId === currentUserId) {
                    actionButtons = `
                        <div class="review-actions">
                            <button class="edit-btn" data-id="${review.reviewId}">
                                <i class="fas fa-pen"></i>
                            </button>
                            <button class="delete-btn" data-id="${review.reviewId}">
                                <i class="fas fa-trash"></i>
                            </button>
                        </div>`;
                }

                reviewElement.innerHTML = `
                    <div class="review-header">
                        <img class="review-profile-pic" src="${profilePic}" alt="${userName}'s profile">
                        <strong>${userName}</strong>
                    </div>
                    <div class="review-text">
                        <p>${review.content}</p>
                    </div>
                    ${actionButtons}
                `;

                reviewsContainer.appendChild(reviewElement);
            });

            document.querySelectorAll(".edit-btn").forEach(btn =>
                btn.addEventListener("click", editReview));
            document.querySelectorAll(".delete-btn").forEach(btn =>
                btn.addEventListener("click", deleteReview));

        } catch (err) {
            console.error("❌ Error loading reviews:", err);
        }
    }

    async function editReview(e) {
        const reviewId = e.target.closest("button").getAttribute("data-id");
        const newContent = prompt("Edit your review:");
        if (!newContent || newContent.trim() === "") return;

        const response = await fetch(`/Reviews/Edit/${reviewId}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": token
            },
            body: JSON.stringify({ ReviewId: reviewId, Content: newContent.trim() })
        });

        const result = await response.json();
        if (result.success) loadReviews();
    }

    async function deleteReview(e) {
        const reviewId = e.target.closest("button").getAttribute("data-id");
        if (!confirm("Delete this review?")) return;

        const response = await fetch(`/Reviews/Delete/${reviewId}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": token
            }
        });

        const result = await response.json();
        if (result.success) loadReviews();
    }

    loadReviews(); 
});

document.addEventListener("DOMContentLoaded", function () {
    console.log("✅ AddToCart.js loaded");

    document.body.addEventListener("click", async function (e) {
        const addBtn = e.target.closest(".add-to-cart");
        if (!addBtn) return;

        const productIdRaw = addBtn.getAttribute("data-product-id");
        const userIdRaw = addBtn.getAttribute("data-user-id");

        console.log("🔍 Raw IDs:", { productIdRaw, userIdRaw });

        const productId = parseInt(productIdRaw);
        const userId = parseInt(userIdRaw);

        console.log("🛒 Add to cart clicked | productId:", productId, "| userId:", userId);

        if (!Number.isInteger(productId) || productId <= 0 || !Number.isInteger(userId) || userId <= 0) {
            alert("❌ Missing or invalid product/user ID");
            return;
        }

        const token = document.querySelector("input[name='__RequestVerificationToken']")?.value || "";
        addBtn.disabled = true;

        try {
            const response = await fetch("/CartItems/AddToCart", {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                    "RequestVerificationToken": token
                },
                body: new URLSearchParams({
                    productId,
                    userId,
                    quantity: 1
                })
            });

            const result = await response.json();
            alert(result.success ? "✅ Added to cart!" : "❌ Failed to add to cart.");
        } catch (error) {
            console.error("❌ Error adding to cart:", error);
        } finally {
            addBtn.disabled = false;
        }
    });
});
document.addEventListener("DOMContentLoaded", async function () {
    const userId = parseInt(document.getElementById("current-user-id")?.value || document.getElementById("user-id")?.value || "0");
    const productId = parseInt(document.getElementById("product-id")?.value || "0");
    const token = document.querySelector("input[name='__RequestVerificationToken']")?.value || "";

    // Verificăm dacă produsul e deja în favorite
    if (userId && productId) {
        try {
            const res = await fetch(`/FavoriteItems/GetFavorites?userId=${userId}`);
            const data = await res.json();
            const fav = data.find(p => p.productId === productId && p.pressed);
            const icon = document.querySelector(".like-btn i");
            if (icon) {
                icon.classList.toggle("fa-solid", !!fav);
                icon.classList.toggle("fa-regular", !fav);
            }
        } catch (err) {
            console.error("⚠️ Failed to load favorites:", err);
        }
    }

    // Toggle favorite pe click
    document.body.addEventListener("click", async function (e) {
        const btn = e.target.closest(".like-btn");
        if (!btn) return;

        const pid = parseInt(btn.getAttribute("data-product-id"));
        const icon = btn.querySelector("i");

        if (!userId || !pid || !icon) return;

        const response = await fetch("/FavoriteItems/ToggleFavorite", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": token
            },
            body: JSON.stringify({ userId, productId: pid })
        });

        const result = await response.json();
        if (result.success) {
            icon.classList.toggle("fa-solid", result.pressed);
            icon.classList.toggle("fa-regular", !result.pressed);
        } else {
            alert("❌ Failed to toggle favorite");
        }
    });
});

