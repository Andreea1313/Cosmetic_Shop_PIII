document.addEventListener("DOMContentLoaded", function () {
    const categoryTitle = document.getElementById("category-title");
    const filterCategoryContainer = document.getElementById("filter-category");

    const productElements = document.querySelectorAll(".product");

    const uniqueCategories = new Set();
    productElements.forEach(product => {
        const cat = product.getAttribute("data-category");
        if (cat) uniqueCategories.add(cat);
    });

    uniqueCategories.forEach(cat => {
        const displayName = cat.replace(/-/g, ' ').replace(/\b\w/g, c => c.toUpperCase());
        const label = document.createElement("label");
        label.innerHTML = `<input type="checkbox" class="filter" value="${cat}"> ${displayName}`;
        filterCategoryContainer.appendChild(label);
    });

    filterCategoryContainer.addEventListener("change", () => {
        const selected = Array.from(document.querySelectorAll(".filter:checked")).map(cb => cb.value);
        document.querySelectorAll(".product").forEach(product => {
            const cat = product.getAttribute("data-category");
            product.style.display = (selected.length === 0 || selected.includes(cat)) ? "block" : "none";
        });
    });

});
