import Link from "next/link"
import authorizeUser from "../../(utils)/authorizeUser"

export default async function Unauthorized() {

    await authorizeUser()


    return (
        <div>
            <h1>Unauthorized</h1>
            <h2>asd</h2>
            <Link className="btn btn-primary" href={"/"}>Go Home
            </Link>
        </div>
    )
}