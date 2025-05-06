"use client";
import Link from "next/link";
import "./Navbar.css";
import { useEffect, useState } from "react";
import { useAuth } from "../AuthContext/AuthProvider";

const Navbar = () => {
  const { user, logout } = useAuth();
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  return (
    <header className="heading">
      <nav className='navbar'>
        <Link href='/' className="nav-brand">
          <button className='btn btn-primary'>HOME</button>
        </Link>

        <div className="hamburger" onClick={toggleMenu}>
          <span></span>
          <span></span>
          <span></span>
        </div>

        <div className={`nav-links ${isMenuOpen ? 'active' : ''}`}>
          {!user ? (
            <Link href="/login">
              <button className="btn btn-success">Login</button>
            </Link>
          ) : (
            !user.householdId ? (
              <>
                <Link href="/profile">
                  <button className='btn btn-primary'>Profile</button>
                </Link>
                <button onClick={logout} className="btn btn-danger">Logout</button>
              </>
            ) : (
              <>
                <Link href="/tasks">
                  <button className='btn btn-primary'>Tasks</button>
                </Link>
                <Link href="/calendar">
                  <button className='btn btn-primary'>Calendar</button>
                </Link>
                <Link href="/profile">
                  <button className='btn btn-primary'>Profile(WIP)</button>
                </Link>
                <button onClick={logout} className="btn btn-danger">Logout</button>
              </>
            )
          )}
        </div>
      </nav>
    </header>
  );
};

export default Navbar;
