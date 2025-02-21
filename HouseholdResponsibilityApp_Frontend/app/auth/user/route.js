import authorizeUser from "../../../(utils)/authorizeUser";

export async function GET() {
    const user = await authorizeUser(); // Server-side authentication check
    return Response.json(user);
}