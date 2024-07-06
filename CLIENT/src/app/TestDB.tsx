'use client';

const TestDB = () => {
  return (
    <button
      onClick={async () => {
        const response = await fetch('/api/database', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({ data: 'alo alo alo' }),
        });

        console.log(await response.json());
      }}
      className='mt-12 rounded-md bg-red-500 p-4 text-xl font-medium text-white shadow-sm hover:bg-red-400'>
      Salut frate
    </button>
  );
};

export default TestDB;
