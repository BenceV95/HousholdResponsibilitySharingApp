import Link from "next/link"
import authorizeUser from "../../(utils)/authorizeUser"
import './Unauthorized.css';

export default async function Unauthorized() {

    await authorizeUser()


    return (
        <div className="Unauthorized">
            <h1>Unauthorized</h1>
            <Link className="btn btn-primary" href={"/"}>Go Home
            </Link>
        </div>
    )
}