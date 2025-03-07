/** @type {import('next').NextConfig} */

// so this makes more sense this way
const BACKEND_URL = process.env.NEXT_PUBLIC_BACKEND_URL;

const nextConfig = {
    reactStrictMode: true,
    async rewrites() {
      return [
        {
          source: "/api/:path*",
          destination: `${BACKEND_URL}/:path*`,
        },
      ];
    },
  };

export default nextConfig;
