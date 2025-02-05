const BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL;

/* 
usage: apiFetch("/tasks")
*/
export async function apiFetch(endpoint) {
  const response = await fetch(`${BASE_URL}${endpoint}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",      
    },
  });

  if (!response.ok) {
    throw new Error("API request failed");
  }

  return response.json();
}

/* 
usage: 
let data = {...data comes here to be sent}
apiPut("/tasks",data)
*/
export async function apiPut(endpoint, data) {
    const response = await fetch(`${BASE_URL}${endpoint}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data)
    });
  
    if (!response.ok) {
      throw new Error("API request failed");
    }
  
    return response.json();
}
