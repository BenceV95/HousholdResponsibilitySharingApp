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
                <Link href='/'>
                    <button className='btn btn-primary'>
                        <span>HOME</span>
                    </button>
                </Link>
                {/* once we have auth then we need to do a landing page and a home page */}
                {
                    !user ?
                        <>
                            <Link href="/login">
                                <button className="btn btn-success">Login</button>
                            </Link>
                        </>
                        :
                        <>

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
                            <Link href="/profile">
                                <button className='btn btn-primary'>
                                    <span>Profile(WIP)</span>
                                </button>
                            </Link>
                            <Link href="/households" >
                                <button className='btn btn-primary'>Households</button>
                            </Link>
                            <button onClick={logout} className="btn btn-danger">Logout</button>
                        </>
                }
            </nav>
            <div>
                <h2 style={{ textAlign: "center" }}>{user ? "van user: " + user.userName : "nincs user"}</h2>
            </div>
        </header>
    )
}

export default Navbar