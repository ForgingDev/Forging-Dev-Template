/**
 * Updates the database with the provided input data.
 *
 * @param input The data to be stored in the database.
 * @returns A promise that resolves once the database is successfully updated.
 */
export async function updateDB(input: string): Promise<void> {
  await fetch('https://forging-dev-auth.vercel.app/api/database', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ data: input }),
  });
}
