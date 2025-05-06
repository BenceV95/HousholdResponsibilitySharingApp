import './Terms.css';

export default function TermsPage() {
  return (
    <div className="terms-container">
      <h1>Terms & Conditions</h1>

      <section>
        <p>
          Please read the documents linked in the footer before using this site. By accessing and using Pura Domus,
          you agree to the following terms and conditions.
        </p>
      </section>

      <section>
        <h2>Purpose</h2>
        <p>
          This project was created for educational and portfolio purposes only. It is not a commercial product and should
          not be used to manage sensitive or real-world data.
        </p>
      </section>

      <section>
        <h2>Data Handling</h2>
        <p>
          All user data is temporary and may be deleted at any time without prior notice. You are advised not to use
          real personal information when registering.
        </p>
      </section>

      <section>
        <h2>Responsibility</h2>
        <p>
          The developers are not responsible for any misuse of the site or any data loss. This project is not subject
          to data protection regulations such as GDPR.
        </p>
      </section>

      <section>
        <h2>Feedback</h2>
        <p>
          If you encounter bugs or have suggestions, we would love to hear from you. Please use the{' '}
          <a href="/contact" className="terms-link">Contact</a> page.
        </p>
      </section>

      <section>
        <h2>Final Note</h2>
        <p>
          Thank you for visiting Pura Domus! We hope this tool inspires better collaboration in every household.
        </p>
      </section>
    </div>
  );
}
