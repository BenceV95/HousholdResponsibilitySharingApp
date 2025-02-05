import React from 'react'
import './Footer.css'
import Link from "next/link";

const Footer = () => {
  return (
    <footer className='footer'>
        <span>Made with ❤️ by: Akos, Balint, Bence</span>
        <li>
            <ol><Link href={"/about"} className='footer-link'>About</Link></ol>
            <ol><Link href={"/privacy"} className='footer-link'>Privacy Policy</Link></ol>
            <ol><Link href={"/tnc"} className='footer-link'>Terms and Conditions</Link></ol>
            <ol><Link href={"/learn-more"} className='footer-link'>Other Information</Link></ol>
        </li>
    </footer>
  )
}

export default Footer