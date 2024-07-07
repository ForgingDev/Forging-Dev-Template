'use server';

export async function updateDB(input: string) {
  await fetch('https://forging-dev-auth.vercel.app/api/database', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ data: input }),
  });
}
