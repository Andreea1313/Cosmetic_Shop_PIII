document.addEventListener("DOMContentLoaded", async function () {
    const userId = parseInt(document.getElementById("current-user-id")?.value || "0");
    const token = document.querySelector("input[name='__RequestVerificationToken']")?.value || "";

    const cartItemsContainer = document.getElementById("cart-items");
    const subtotalPriceElement = document.getElementById("subtotal-price");
    const shippingPriceElement = document.getElementById("shipping-price");
    const totalPriceElement = document.getElementById("total-price");
    const favoritesListContainer = document.getElementById("favorites-list");

    async function loadCartFromDb() {
        if (!cartItemsContainer || !userId) return;
        const response = await fetch(`/CartItems/GetCartItems?userId=${userId}`);
        const cart = await response.json();

        cartItemsContainer.innerHTML = "";
        let subtotal = 0;

        cart.forEach(item => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td><img src="${item.image}" alt="${item.name}" width="50"></td>
                <td>${item.name}</td>
                <td>${item.description}</td>
                <td>$${item.price}</td>
                <td><input type="number" value="${item.quantity}" min="1" data-id="${item.cartItemId}" class="quantity-input"></td>
                <td>$${(item.price * item.quantity).toFixed(2)}</td>
                <td><button class="remove-item" data-id="${item.cartItemId}">Remove</button></td>`;
            cartItemsContainer.appendChild(row);
            subtotal += item.price * item.quantity;
        });

        const shipping = subtotal > 50 ? 0 : 5;
        subtotalPriceElement.textContent = subtotal.toFixed(2);
        shippingPriceElement.textContent = shipping.toFixed(2);
        totalPriceElement.textContent = (subtotal + shipping).toFixed(2);
    }

    async function loadFavoritesFromDb() {
        if (!favoritesListContainer || !userId) return;

        try {
            const response = await fetch(`/FavoriteItems/GetFavorites?userId=${userId}`);
            const favorites = await response.json();

            favoritesListContainer.innerHTML = "";
            if (favorites.length === 0) {
                favoritesListContainer.innerHTML = "<p>No favorites yet.</p>";
                return;
            }

            favorites.forEach(product => {
                const productElement = document.createElement("div");
                productElement.classList.add("product");
                productElement.setAttribute("data-product-id", product.productId);

                const heartIconClass = product.pressed ? "fa-solid" : "fa-regular";

                productElement.innerHTML = `
                <img src="${product.image}" alt="${product.name}">
                <h3>${product.name}</h3>
                <p>${product.description}</p>
                <div class="price">$${product.price}</div>
                <button class="add-to-cart" data-product-id="${product.productId}" data-user-id="${userId}">Add to Cart</button>
                <button class="like-btn" data-product-id="${product.productId}" data-user-id="${userId}">
                    <i class="${heartIconClass} fa-heart"></i>
                </button>`;

                favoritesListContainer.appendChild(productElement);
            });
        } catch (error) {
            console.error("❌ Error loading favorites:", error);
        }
    }


    // EVENT LISTENERS UNIFIED
    document.body.addEventListener("click", async function (event) {
        const addBtn = event.target.closest(".add-to-cart");
        const likeBtn = event.target.closest(".like-btn");
        const removeBtn = event.target.closest(".remove-from-favorites");

        // Add to Cart
        if (addBtn) {
            const productId = parseInt(addBtn.getAttribute("data-product-id"));
            if (!Number.isInteger(productId) || productId <= 0 || !Number.isInteger(userId) || userId <= 0) {
                alert("❌ Missing or invalid product/user ID");
                return;
            }


            const response = await fetch("/CartItems/AddToCart", {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                    "RequestVerificationToken": token
                },
                body: new URLSearchParams({ productId, userId, quantity: 1 })
            });

            const result = await response.json();
            alert(result.success ? "✅ Added to cart!" : "❌ Failed to add to cart.");
        }

        // Toggle Favorite
        if (likeBtn) {
            const productId = parseInt(likeBtn.getAttribute("data-product-id"));
            const icon = likeBtn.querySelector("i");
            if (!productId || !userId) return;

            const response = await fetch("/FavoriteItems/ToggleFavorite", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": token
                },
                body: JSON.stringify({ userId, productId })
            });

            const result = await response.json();
            if (result.success) {
                icon.classList.toggle("fa-solid", result.pressed);
                icon.classList.toggle("fa-regular", !result.pressed);
                if (!result.pressed && favoritesListContainer) {
                    likeBtn.closest(".product")?.remove();
                }
            } else {
                alert("❌ Failed to toggle favorite.");
            }
        }

        // Remove from Favorites
        if (removeBtn) {
            const productId = parseInt(removeBtn.getAttribute("data-product-id"));
            if (!confirm("Remove from favorites?")) return;

            const response = await fetch("/FavoriteItems/RemoveFromFavorites", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": token
                },
                body: JSON.stringify({ userId, productId })
            });

            const result = await response.json();
            if (result.success) {
                removeBtn.closest(".product")?.remove();
            } else {
                alert("❌ Failed to remove from favorites.");
            }
        }

        // Remove from Cart
        if (event.target.classList.contains("remove-item")) {
            const cartItemId = event.target.getAttribute("data-id");
            if (!confirm("Remove item from cart?")) return;

            const response = await fetch("/CartItems/Remove", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": token
                },
                body: JSON.stringify({ cartItemId: parseInt(cartItemId) })
            });

            const result = await response.json();
            if (result.success) {
                await loadCartFromDb();
            } else {
                alert("❌ Failed to remove item.");
            }
        }
    });

    // Quantity Update
    document.body.addEventListener("change", async function (e) {
        if (e.target.classList.contains("quantity-input")) {
            const cartItemId = e.target.getAttribute("data-id");
            const newQuantity = parseInt(e.target.value);

            if (newQuantity <= 0 || isNaN(newQuantity)) {
                alert("❌ Quantity must be a positive number.");
                return;
            }

            const response = await fetch("/CartItems/UpdateQuantity", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": token
                },
                body: JSON.stringify({ cartItemId: parseInt(cartItemId), quantity: newQuantity })
            });

            const result = await response.json();
            if (result.success) {
                await loadCartFromDb();
            } else {
                alert("❌ Failed to update quantity.");
            }
        }
    });

    await loadCartFromDb();
    await loadFavoritesFromDb();
});

