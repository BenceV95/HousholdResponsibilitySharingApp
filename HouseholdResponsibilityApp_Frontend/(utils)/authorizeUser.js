// put the payload in the token, then get it from there on the next.js server
import { jwtDecode } from 'jwt-decode';
import { cookies } from 'next/headers';

export default async function authorizeUser() {
    const clientCookies = await cookies();
    console.log(clientCookies)
    const token = clientCookies.get('token')?.value;

    // console.log("token", token)


    if (token) {
        try {
            // Decode the token and extract the payload
            const decodedToken = jwtDecode(token); // `token.value` is the actual JWT string

            console.log("decoded token:", decodedToken)

            const userName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
            const email = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'];
            const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
            const householdId = decodedToken["householdId"];

            // console.log('Decoded token:', decodedToken);
            // console.log(userName)
            // console.log(email)
            // console.log(role)
            return {
                userName,
                email,
                role,
                userId,
                householdId
            }
        } catch (error) {
            console.error('Error decoding token:', error);
        }
    } else {
        console.log('No token found');
        return null
    }
}
