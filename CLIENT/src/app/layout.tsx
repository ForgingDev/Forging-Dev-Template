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
            <Script id='google-tag-manager'>
              {`
                (function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
                new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
                j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
                'https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
                })(window,document,'script','dataLayer','GTM-T75XCCSR');
              `}
            </Script>
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
            <noscript>
              <iframe
                src='https://www.googletagmanager.com/ns.html?id=GTM-T75XCCSR'
                height='0'
                width='0'
                // @ts-ignore
                style='display:none;visibility:hidden'></iframe>
            </noscript>
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
