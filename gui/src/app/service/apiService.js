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
            try {
                const response = await fetch(baseUrl);
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                return response.json();
            } catch (error) {
                console.error("Error fetching data:", error);
                throw error;
            }
        },
        

        delete: async (id) => {
            const response = await fetch(`${baseUrl}/${id}`, {
                method: "DELETE",
            });
            return response;
        },

        put: async (id, body) => {
            const response = await fetch(`${baseUrl}/${id}`, {
                method: "PUT",
                headers: jsonContentHeader,
                body: JSON.stringify(body),
            });
            return response.json();
        },

        patch: async (id, body) => {
            const response = await fetch(`${baseUrl}/${id}`, {
                method: "PATCH",
                headers: jsonContentHeader,
                body: JSON.stringify(body),
            });
            return response;
        },
    };
};

export default apiService;
