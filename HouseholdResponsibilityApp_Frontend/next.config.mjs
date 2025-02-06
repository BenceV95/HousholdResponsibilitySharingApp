/** @type {import('next').NextConfig} */
const nextConfig = {
    reactStrictMode: true,
    async rewrites() {
      return [
        {
          source: "/api/:path*",
          destination: "http://localhost:5141/:path*", // Target ASP.NET API
        },
      ];
    },
  };

export default nextConfig;
