import './Contact.css';

export default function ContactPage() {
  return (
    <div className="contact-container">
      <h1>Contact & Contributors</h1>

      <section>
        <p>
          This project was built by a small team of developers with a shared interest in full-stack web development and collaboration tools.
        </p>
      </section>

      <section>
        <h2>GitHub Profiles</h2>
        <ul>
          <li>
            <a href="https://github.com/vulpes556" target="_blank" rel="noopener noreferrer">
              @vulpes556
            </a>
          </li>
          <li>
            <a href="https://github.com/BenceV95" target="_blank" rel="noopener noreferrer">
              @BenceV95
            </a>
          </li>
          <li>
            <a href="https://github.com/bukovinszkiakos" target="_blank" rel="noopener noreferrer">
              @bukovinszkiakos
            </a>
          </li>
        </ul>
      </section>

      <section>
        <h2>Project Repository</h2>
        <p>
          You can find the full source code and ongoing development of the project on GitHub:
        </p>
        <a
          href="https://github.com/BenceV95/HousholdResponsibilitySharingApp/tree/main"
          target="_blank"
          rel="noopener noreferrer"
          className="project-link"
        >
          Pura Domus GitHub Repository
        </a>
      </section>
    </div>
  );
}
