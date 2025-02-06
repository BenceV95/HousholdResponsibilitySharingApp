import React from 'react'
import Link from "next/link";
import './Navbar.css'

const Navbar = () => {
    return (
        <heading className="heading">
            <nav className='navbar'>
                {/* once we have auth then we need to do a landing page and a home page */}
                <Link href='/'>
                    <button className='btn btn-primary'>
                        <span>HOME</span>
                    </button>
                </Link>
                <Link href="/tasks">
                    <button className='btn btn-primary'>
                        <span>Tasks</span>
                    </button>
                </Link>
                <Link href="/users">
                    <button className='btn btn-primary'>
                        <span>Users</span>
                    </button>
                </Link>
                <Link href="/settings">
                    <button className='btn btn-primary'>
                        <span>Settings</span>
                    </button>
                </Link>
            </nav>
        </heading>
    )
}

export default Navbar