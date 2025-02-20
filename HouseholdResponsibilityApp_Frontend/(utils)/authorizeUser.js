// put the payload in the token, then get it from there on the next.js server
import { jwtDecode } from 'jwt-decode';
import { cookies } from 'next/headers';

export default async function authorizeUser() {
    const clientCookies = await cookies();

    const token = clientCookies.get('token')?.value;


    if (token) {
        try {

            // Decode the token and extract the payload
            const decodedToken = jwtDecode(token); // `token.value` is the actual JWT string


            const userName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
            const email = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'];
            const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
            const householdId = decodedToken["householdId"];

            console.log("username from token", userName)
            console.log("householdID from the token", householdId)
            return {
                userName,
                email,
                role,
                userId,
                householdId
            }
        } catch (error) {
            console.error('Error decoding token:', error);
            return null;
        }
    } else {
        console.log('No token found');
        return null
    }
}
