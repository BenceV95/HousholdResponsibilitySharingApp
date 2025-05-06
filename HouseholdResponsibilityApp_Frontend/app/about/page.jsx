import './About.css';

export default function AboutPage() {
  return (
    <div className="about-container">
      <h1>Welcome to Pura Domus!</h1>
      <p>
        This application was built to help households manage responsibilities fairly and transparently.
        We believe that a clean and organized home starts with teamwork and clarity.
      </p>
      <p>
        Whether you're living with roommates, family, or friends, Pura Domus lets you assign, schedule,
        and track recurring chores through an intuitive interface.
      </p>
      <h2>How to use the app?</h2>
      <ul>
        <li>Register or Log in</li>
        <li>Create or join a household</li>
        <li>Assign and schedule tasks</li>
        <li>Work together and keep things clean!</li>
      </ul>
    </div>
  );
}
