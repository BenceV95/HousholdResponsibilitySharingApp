import './OtherInfo.css';

export default function OtherInformationPage() {
  return (
    <div className="otherinfo-container">
      <h1>Other Information</h1>

      <section>
        <p>
          Pura Domus is a full-stack household responsibility-sharing application developed for learning and portfolio
          purposes. It aims to help households organize, assign, and manage recurring tasks in a transparent and efficient way.
        </p>
      </section>

      <section>
        <h2>Key Features</h2>
        <ul>
          <li>User authentication and session management</li>
          <li>Household creation, joining, and management</li>
          <li>Task assignment and scheduling (daily, weekly, monthly)</li>
          <li>Interactive calendar for tracking tasks</li>
          <li>User roles and permissions (Admin, Member, Guest)</li>
          <li>Automated task distribution</li>
        </ul>
      </section>

      <section>
        <h2>Technology Stack</h2>
        <ul>
          <li>Frontend: Next.js, React, React Hook Form, React Big Calendar</li>
          <li>Backend: ASP.NET Core, RESTful API</li>
          <li>Database: PostgreSQL, Entity Framework Core</li>
          <li>Authentication: JWT</li>
          <li>Other: Docker, xUnit</li>
        </ul>
      </section>

      <section>
        <h2>Roadmap Highlights</h2>
        <ul>
          <li>Household invitations</li>
          <li>Email or push notifications for task reminders</li>
          <li>Task history and completion tracking</li>
          <li>Mobile app design</li>
          <li>Multi-language support</li>
          <li>UI/UX enhancements</li>
        </ul>
      </section>

      <section>
        <h2>Disclaimer</h2>
        <p>
          This is a demo/learning project. All data is stored temporarily and is not intended for production use.
          Please avoid entering real personal information.
        </p>
      </section>

      <section>
        <h2>Contact & Source</h2>
        <p>
          For any suggestions or feedback, please visit the{' '}
          <a href="/contact" className="other-link">Contact</a> page or check out the project on{' '}
          <a href="https://github.com/BenceV95/HousholdResponsibilitySharingApp" target="_blank" className="other-link">
            GitHub
          </a>.
        </p>
      </section>
    </div>
  );
}
