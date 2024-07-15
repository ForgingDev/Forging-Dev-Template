'use client';

import { FC, PropsWithChildren } from 'react';
import { QueryClient, QueryClientProvider } from 'react-query';

const HydrationProvider: FC<PropsWithChildren> = ({ children }) => {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: 5 * 60 * 1000, // this sets the cache time to 5 minutes
        retry: 1,
      },
      mutations: {
        retry: 1,
      },
    },
  });

  return (
    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  );
};

export default HydrationProvider;
