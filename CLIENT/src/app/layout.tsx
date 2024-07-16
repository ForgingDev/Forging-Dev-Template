import FGNotification from '@/components/FGLibrary/FGNotification';
import FGSidebar from '@/components/FGLibrary/FGSidebar/FGSidebar';
import GlobalDialogs from '@/components/GlobalDialogs';
import { cn } from '@/lib/utils';
import { ClerkProvider } from '@clerk/nextjs';
import type { Metadata } from 'next';
import { Inter } from 'next/font/google';
import Script from 'next/script';
import 'primeicons/primeicons.css';
import { PrimeReactProvider } from 'primereact/api';
import 'primereact/resources/themes/lara-light-cyan/theme.css';
import { ReactNode } from 'react';
import HydrationProvider from '../providers/HydrationProvider';
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
          <head>
            <Script
              async
              src='https://www.googletagmanager.com/gtag/js?id=G-Q5BDT93GBS'></Script>
            <Script id='google-analytics'>
              {`  
                  window.dataLayer = window.dataLayer || [];
                  function gtag(){dataLayer.push(arguments);}
                  gtag('js', new Date());
                  gtag('config', 'G-Q5BDT93GBS');
              `}
            </Script>
          </head>
          <body className={cn(inter.className, 'bg-zinc-800 text-white')}>
            <FGSidebar />
            <GlobalDialogs />
            <FGNotification />
            <HydrationProvider>
              <main className='container mx-auto px-2 pt-16'>{children}</main>
            </HydrationProvider>
          </body>
        </html>
      </PrimeReactProvider>
    </ClerkProvider>
  );
}
