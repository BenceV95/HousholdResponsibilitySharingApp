import Link from "next/link"
import './Unauthorized.css';

export default async function Unauthorized() {

    return (
        <div className="Unauthorized">
            <h1>Unauthorized</h1>
            <Link className="btn btn-primary" href={"/"}>Go Home
            </Link>
        </div>
    )
}