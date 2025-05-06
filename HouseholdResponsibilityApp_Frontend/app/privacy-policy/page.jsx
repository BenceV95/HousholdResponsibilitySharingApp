import Link from 'next/link';
import './PrivacyPolicy.css';

export default function PrivacyPolicyPage() {
  return (
    <div className="privacy-container">
      <h1>Privacy Policy</h1>

      <section>
        <h2>What We Collect</h2>
        <p>When you register on our site, we collect basic information, such as:</p>
        <ul>
          <li>Full name</li>
          <li>Email address</li>
          <li>Household and task-related data</li>
        </ul>
      </section>

      <section>
        <h2>How We Use Your Information</h2>
        <p>This is a learning project. The data you provide is only used to make the application functional. We do not sell or share any information, and all data may be deleted at any time.</p>
      </section>

      <section>
        <h2>Cookies</h2>
        <p>We use cookies to manage user sessions. Since this is a portfolio/learning project, no GDPR compliance is implemented.</p>
      </section>

      <section>
        <h2>Your Rights</h2>
        <p>You have the right to:</p>
        <ul>
          <li>Access your personal data</li>
          <li>Request correction of inaccurate information</li>
          <li>Delete your account and data at any time</li>
        </ul>
      </section>

      <section>
        <h2>Security</h2>
        <p>Basic security measures are implemented, but we recommend not using real personal data.</p>
      </section>

      <section>
        <h2>Changes to This Policy</h2>
        <p>This policy may be updated in the future. The latest version will always be available on this page.</p>
      </section>

      <section>
        <h2>Contact Us</h2>
        <p>
          If you have any questions about this policy or your data, please reach out via the{' '}
          <Link href="/contact" className="contact-link">
            Contact
          </Link>{' '}
          page.
        </p>
      </section>
    </div>
  );
}
