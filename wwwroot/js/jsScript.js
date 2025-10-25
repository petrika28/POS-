
    $(document).ready(function () {
                        const productFormHtml = `
    <div class="product-form border p-3 mb-3">
        <div class="form-group">
            <label>Name of product</label>
            <input type="text" name="names" class="form-control product-name" id="searchBox" required />
            <div class="search-results"></div>  <!-- This div will hold the search results -->
        </div>
        <div class="form-group">
            <label>Quantity of product</label>
            <input type="number" name="quantities" class="form-control" required min="1" />
        </div>
        <button type="button" id="deleteProductFormBtn" class="btn btn-secondary mb-3">Delete Product</button>
    </div>
    `;

    // Add product form on button click
    $('#addProductFormBtn').click(function () {
        $('#productsContainer').append(productFormHtml);
                        });

    // Handle dynamically added product forms' delete button
    $('#productsContainer').on('click', '#deleteProductFormBtn', function () {
        $(this).closest('.product-form').remove();
                        });

    // Handle the keyup event on the dynamically added product name fields
    $('#productsContainer').on('keyup', '#searchBox', function () {
                            var searchTerm = $(this).val().trim();
    var searchResultsDiv = $(this).next('.search-results');  // Get the associated search results div

    // Clear previous search results
    searchResultsDiv.empty();

                            if (searchTerm.length >= 3) {
        console.log('Searching for: ' + searchTerm);  // To check search term

    // Make AJAX request to search products
    $.ajax({
        url: searchProductsUrl,  // Correct route to the Products controller
    type: 'GET',
    data: {term: searchTerm },
    success: function (data) {
        console.log(data);  // Inspect the returned data

    // Empty the search results container before adding new ones
    searchResultsDiv.empty();

                                        if (data.length > 0) {
        // Loop through the products and display only the names
        data.forEach(function (productName) {
            let resultHtml = `
                                                    <div class="search-result">
                                                        <h5>${productName}</h5>
                                                    </div>
                                                `;
            searchResultsDiv.append(resultHtml);  // Append to the associated results div
        });
                                        } else {
        searchResultsDiv.append('<p>No products found.</p>');
                                        }
                                    },
    error: function () {
        console.log('AJAX request failed');
    searchResultsDiv.append('<div>Error while searching.</div>');
                                    }
                                });
                            }
                        });
                    });




