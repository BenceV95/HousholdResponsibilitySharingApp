"use client";
import styles from "./page.module.css";
import { useEffect, useState } from "react";

export default function Home() {

  const [text, setText] = useState("");

  useEffect(() => {
    fetch("https://localhost:7153/hello")
      .then((response) => response.text())
      .then((data) => setText(data));
  }, []);


  return (
    <div className={styles.page}>
      <main className={styles.main}>
        <h1>{text}</h1>
      </main>

    </div>
  );
}
