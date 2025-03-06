const BACKEND_URL = "/api";

//we should make our fetches uniform, and make the backend send unform error messages as well!
//also, not just saying API request failed, cause we wont know what exactly went wrong

async function handleResponse(response) {
  if (!response.ok) {
    const errorData = await response.json();
    throw new Error(errorData.Message); 
  }
  if (response.status === 204) return null;
  return await response.json();
}


/* 
usage: apiFetch("/tasks")
*/
export async function apiFetch(endpoint) {
  console.log(`GET: ${BACKEND_URL}${endpoint}`);
  const response = await fetch(`${BACKEND_URL}${endpoint}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
  });
  return handleResponse(response);
}

/* 
usage: 
let data = {...data comes here to be sent}
apiPut("/tasks",data)
*/
export async function apiPut(endpoint, data) {
  console.log(`PUT: ${BACKEND_URL}${endpoint}`);
  const response = await fetch(`${BACKEND_URL}${endpoint}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
}


export async function apiPost(endpoint, data) {
  console.log(`POST: ${BACKEND_URL}${endpoint}`);
  const response = await fetch(`${BACKEND_URL}${endpoint}`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
}


export async function apiDelete(endpoint) {
  console.log(`DELETE: ${BACKEND_URL}${endpoint}`);
  const response = await fetch(`${BACKEND_URL}${endpoint}`, {
    method: "DELETE",
  });
  return handleResponse(response);
}
