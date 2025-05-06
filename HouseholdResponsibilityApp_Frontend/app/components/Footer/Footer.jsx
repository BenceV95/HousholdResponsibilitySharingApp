import React from 'react'
import './Footer.css'
import Link from "next/link";

const Footer = () => {
  return (
    <footer className='footer'>
        <span>Made with ❤️ by: Akos, Balint, Bence</span>
        <li>
            <ol><Link href={"/about"} className='footer-link'>About</Link></ol>
            <ol><Link href={"/contact"} className='footer-link'>Contact</Link></ol>
            <ol><Link href={"/privacy-policy"} className='footer-link'>Privacy Policy</Link></ol>
            <ol><Link href={"/terms-and-conditions"} className='footer-link'>Terms and Conditions</Link></ol>
            <ol><Link href={"/other-information"} className='footer-link'>Other Information</Link></ol>
        </li>
    </footer>
  )
}

export default Footer