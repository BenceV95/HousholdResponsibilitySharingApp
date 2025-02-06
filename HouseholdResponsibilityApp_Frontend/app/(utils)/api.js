const BACKEND_URL = process.env.NEXT_PUBLIC_BACKEND_URL;

/* 
usage: apiFetch("/tasks")
*/
export async function apiFetch(endpoint) {
  console.log(BACKEND_URL);
  
  const response = await fetch(`${BACKEND_URL}${endpoint}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",      
    },
  });

  if (!response.ok) {
    throw new Error("API request failed");
  }

  console.log(`data arrived from: ${BACKEND_URL}${endpoint}`);
  
  return response.json();
}

/* 
usage: 
let data = {...data comes here to be sent}
apiPut("/tasks",data)
*/
export async function apiPut(endpoint, data) {
    const response = await fetch(`${BACKEND_URL}${endpoint}`, {
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

export async function apiPost(endpoint, data) {
  console.log(`${BACKEND_URL}${endpoint}`);
  
  const response = await fetch(`${BACKEND_URL}${endpoint}`, {
    method: "POST",
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

export async function apiDelete(endpoint) {
  console.log(`${BACKEND_URL}${endpoint}`);
  
  const response = await fetch(`${BACKEND_URL}${endpoint}`, {
    method: "DELETE",
  });

  if (!response.ok) {
    throw new Error("API request failed");
  }

}