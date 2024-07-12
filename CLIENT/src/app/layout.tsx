import FGNotification from '@/components/FGLibrary/FGNotification';
import FGSidebar from '@/components/FGLibrary/FGSidebar/FGSidebar';
import GlobalDialogs from '@/components/GlobalDialogs';
import { cn } from '@/lib/utils';
import { ClerkProvider } from '@clerk/nextjs';
import type { Metadata } from 'next';
import { Inter } from 'next/font/google';
import 'primeicons/primeicons.css';
import { PrimeReactProvider } from 'primereact/api';
import 'primereact/resources/themes/lara-light-cyan/theme.css';
import { ReactNode } from 'react';
import HydrationBoundary from './HydrationBoundary';
import './globals.css';

const inter = Inter({ subsets: ['latin'] });

export const metadata: Metadata = {
  title: 'Forging Dev - Starter Template',
  description: 'Starter Template for Forging Dev',
};

export default function RootLayout({
  children,
}: Readonly<{
  children: ReactNode;
}>): JSX.Element {
  return (
    <ClerkProvider>
      <PrimeReactProvider>
        <html lang='en'>
          <body className={cn(inter.className, 'bg-zinc-800 text-white')}>
            <FGSidebar />
            <GlobalDialogs />
            <FGNotification />
            <HydrationBoundary>
              <main className='container mx-auto px-2 pt-16'>{children}</main>
            </HydrationBoundary>
          </body>
        </html>
      </PrimeReactProvider>
    </ClerkProvider>
  );
}
