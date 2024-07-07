import { Client } from 'pg'; // Import the Client class

export async function POST(req: Request): Promise<Response> {
  const data = await req.json();

  // Create a new client instance with the connection string from environment variables
  const client = new Client({
    connectionString: process.env.DATABASE_CONNECTION_STRING,
  });

  try {
    // Connect to the database
    await client.connect();

    console.log('-------- CONNECTED -------------');

    const queryText = `INSERT INTO test(label) VALUES('${data.data}');`;
    const res = await client.query(queryText);

    // Close the client connection
    await client.end();

    // Return a successful response with the inserted data
    return new Response(JSON.stringify(res.rows[0]), {
      headers: {
        'Content-Type': 'application/json',
      },
      status: 200,
    });
  } catch (err) {
    console.error('Database connection error');

    // Close the client connection in case of error
    await client.end();

    // Return an error response
    return new Response(JSON.stringify({ error: 'Failed to insert data' }), {
      headers: {
        'Content-Type': 'application/json',
      },
      status: 500,
    });
  }
}
