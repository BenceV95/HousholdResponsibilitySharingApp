"use client";
import Link from "next/link";
import "./Navbar.css";
import { useEffect, useState } from "react";
import { useAuth } from "../AuthContext/AuthProvider";

const Navbar = () => {
    const { user, logout } = useAuth();

    return (
      <header className="heading">
        <nav className='navbar'>
          <Link href='/'>
            <button className='btn btn-primary'>
              HOME
            </button>
          </Link>
  
          {
            !user ? (
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
                  {/*
                  <Link href="/users">
                    <button className='btn btn-primary'>Users</button>
                  </Link>
                  */}
                  <Link href="/profile">
                    <button className='btn btn-primary'>Profile(WIP)</button>
                  </Link>
                  {/*
                  <Link href="/households">
                    <button className='btn btn-primary'>Households</button>
                  </Link>
                  */}
                  {/*
                  <Link href="/groups">
                    <button className='btn btn-primary'>Groups</button>
                  </Link>
                  */}
                  <button onClick={logout} className="btn btn-danger">Logout</button>
                </>
              )
            )
          }
        </nav>
      </header>
    );
  };
  
  export default Navbar;
  