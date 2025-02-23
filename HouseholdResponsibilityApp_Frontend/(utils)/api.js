const BACKEND_URL = "/api";

//we should make our fetches uniform, and make the backend send unform error messages as well!
//also, not just saying API request failed, cause we wont know what exactly went wrong

/* 
usage: apiFetch("/tasks")
*/
export async function apiFetch(endpoint) {

  const response = await fetch(`${BACKEND_URL}${endpoint}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  });

  const parsedResponse = await response.json()

  if (!response.ok) {

    throw new Error("API request failed", parsedResponse.message);
  }

  return parsedResponse;
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


  const parsedResponse = await response.json()

  if (!response.ok) {

    throw new Error("API request failed", parsedResponse.message);
  }

  return parsedResponse;
}

export async function apiPost(endpoint, data) {

  const response = await fetch(`${BACKEND_URL}${endpoint}`, {
    method: "POST",
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data)
  });


  if (!response.ok) {
    const errorMsg = await response.text();
    console.log("response text", errorMsg);
    throw new Error(errorMsg);
  }

  const parsedResponse = await response.json()
  return parsedResponse;
}

export async function apiDelete(endpoint) {

  const response = await fetch(`${BACKEND_URL}${endpoint}`, {
    method: "DELETE",
  });

  if (!response.ok) {
    throw new Error("API request failed");
  }

}