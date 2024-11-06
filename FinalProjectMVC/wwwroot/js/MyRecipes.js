function showDetails(recipeId) {
    var detailsDiv = document.getElementById("details-" + recipeId);
    if (detailsDiv.style.display === "none") {
        detailsDiv.style.display = "block";
    } else {
        detailsDiv.style.display = "none";
    }
}