'use client';

import { updateDB } from './actions';

const TestDB = () => {
  return (
    <button
      onClick={async () => await updateDB('sal')}
      className='mt-12 rounded-md bg-red-500 p-4 text-xl font-medium text-white shadow-sm hover:bg-red-400'>
      Salut frate
    </button>
  );
};

export default TestDB;
