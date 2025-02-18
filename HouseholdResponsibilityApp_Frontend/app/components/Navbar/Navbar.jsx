"use client"
import Link from "next/link";
import './Navbar.css'
import { useEffect, useState } from "react";
import { useAuth } from "../AuthContext/AuthProvider";

const Navbar = () => {
    const { user, logout } = useAuth();





    return (
        <header className="heading">
            <nav className='navbar'>
                {/* once we have auth then we need to do a landing page and a home page */}
                <Link href='/'>
                    <button className='btn btn-primary'>
                        <span>HOME</span>
                    </button>
                </Link>
                {
                    !user ?
                        <>
                            <Link href="/login">
                                <button className="btn btn-primary">Login</button>
                            </Link>
                            <Link href="/register">
                                <button className="btn btn-primary">Register</button>
                            </Link>
                        </> :
                        <button onClick={logout}>Logout</button>
                }

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
                <Link href="/households" >
                    <button className='btn btn-primary'>Households</button>
                </Link>
            </nav>
            <div>
                <h2>{user ? "van user" : "nincs user"}</h2>
                {user?.userName}
            </div>
        </header>
    )
}

export default Navbar