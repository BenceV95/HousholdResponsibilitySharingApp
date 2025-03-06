export default function About({ data }) {
    return (
        <div>
            {data ? <h1>Logged in</h1> : <h2>Logged out</h2>}
        </div>
    )
}

// export default function Page({ data }) {
//     // Render data...
// }

// This gets called on every request
// export async function getServerSideProps() {
//     const user = await authorizeUser();

//     return { props: { user } }
// }