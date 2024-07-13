import { clerkMiddleware, createRouteMatcher } from '@clerk/nextjs/server';
import { PROTECTED_ROUTES } from './lib/routes';

const isProtectedRoute = createRouteMatcher(PROTECTED_ROUTES);

export const config = {
  matcher: ['/((?!.*\\..*|_next).*)', '/', '/(api|trpc)(.*)'],
};

export default clerkMiddleware((auth, req) => {
  if (isProtectedRoute(req)) {
    auth().protect();
  }
});
