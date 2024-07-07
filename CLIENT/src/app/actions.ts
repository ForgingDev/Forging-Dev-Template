'use server';

import { revalidateTag } from 'next/cache';

export async function fetchDataFromBE(): Promise<{
  data?: string[];
  error?: string;
}> {
  const API_URL = 'http://localhost:5296/base';

  try {
    const response = await fetch(`${API_URL}?param=200`, {
      method: 'GET',
      next: {
        tags: ['fetch'],
      },
    });

    const data: string[] = await response.json();

    return {
      data,
    };
  } catch (error) {
    return {
      error: 'Error fetching data from BE',
    };
  }
}

export async function postDataToBE(input: string | undefined): Promise<void> {
  const API_URL = 'http://localhost:5296/base';

  if (!input) throw new Error('Input is required');

  try {
    const response = await fetch(API_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(input),
    });

    const data = await response.json();

    revalidateTag('fetch');

    return data;
  } catch (error) {
    console.log(error);
  }
}

export async function updateDB(input: string) {
  await fetch('https://forging-dev-auth.vercel.app/api/database', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ data: input }),
  });
}
