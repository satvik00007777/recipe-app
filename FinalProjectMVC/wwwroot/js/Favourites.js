function addToFavorites(title, source, imageUrl) {
    const uniqueId = btoa(title + source)

    fetch('https://localhost:7255/api/favorites/add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            uniqueId: uniqueId,
            title: title,
            sourceUrl: source,
            imageUrl: imageUrl
        })
    }).then(response => {
        if (response.ok) {
            alert("Recipe added to favorites!");
        } else {
            alert("Failed to add recipe to favorites.");
        }
    });
}