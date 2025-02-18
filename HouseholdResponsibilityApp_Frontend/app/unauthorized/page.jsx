import Link from "next/link"

export default function Unauthorized() {
    return (
        <div>
            <h1>Unauthorized</h1>
            <Link className="btn btn-primary" href={"/"}>Go Home
            </Link>
        </div>
    )
}