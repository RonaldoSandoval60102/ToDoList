const apiService = (url, endpoint) => {
    const baseUrl = `${url}/${endpoint}`;
    const jsonContentHeader = {
        "Content-type": "application/json; charset=UTF-8",
    };

    return {
        post: async (body) => {
            const response = await fetch(baseUrl, {
                method: "POST",
                headers: jsonContentHeader,
                body: JSON.stringify(body),
            });
            return response.json();
        },

        getAll: async () => {
            const response = await fetch(baseUrl);
            return response.json();
        },

        delete: async (id) => {
            const response = await fetch(`${baseUrl}/${id}`, {
                method: "DELETE",
            });
            return response.json();
        },
    };
};

export default apiService;