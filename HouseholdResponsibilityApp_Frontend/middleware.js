import { NextResponse } from "next/server";


export function middleware(request) {

    const { pathname } = request.nextUrl;

    const unprotectedPaths = ["/login", "/", "/unauthorized"]

    const token = request.cookies.get("token")?.value;

    if (!unprotectedPaths.includes(pathname) && !token) {

        const url = request.nextUrl.clone();
        url.pathname = "/unauthorized";
        return NextResponse.redirect(new URL("/unauthorized", request.url));

    }

    return NextResponse.next();
}

export const config = {
    matcher: [
        /*
         
    Match all request paths except for the ones starting with:
       
    api (API routes)
    _next/static (static files)
    _next/image (image optimization files)
    favicon.ico, sitemap.xml, robots.txt (metadata files)
    */
        {
            source: '/((?!api|_next/static|_next/image|favicon.ico|sitemap.xml|robots.txt).*)',
            missing: [{ type: 'header', key: 'next-router-prefetch' }, { type: 'header', key: 'purpose', value: 'prefetch' },],
        },

        {
            source:
                '/((?!api|_next/static|_next/image|favicon.ico|sitemap.xml|robots.txt).*)',
            has: [
                { type: 'header', key: 'next-router-prefetch' },
                { type: 'header', key: 'purpose', value: 'prefetch' },
            ],
        },

        {
            source:
                '/((?!api|_next/static|_next/image|favicon.ico|sitemap.xml|robots.txt).*)',
            has: [{ type: 'header', key: 'x-present' }],
            missing: [{ type: 'header', key: 'x-missing', value: 'prefetch' }],
        },
    ],

}