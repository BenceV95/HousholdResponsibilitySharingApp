import React from 'react'
import Link from "next/link";
import './Navbar.css'

const Navbar = () => {
    return (
        <heading className="heading">
            <nav className='navbar'>
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
            </nav>
        </heading>
    )
}

export default Navbar