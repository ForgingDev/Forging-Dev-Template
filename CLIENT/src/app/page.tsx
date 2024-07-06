import { FC, Suspense } from 'react';
import FetchData from './FetchData';
import TestDB from './TestDB';
import TriggerFetchData from './TriggerFetchData';

const Homepage: FC = () => {
  return (
    <>
      <TriggerFetchData />
      <Suspense fallback={<div>Loading fetch data...</div>}>
        <FetchData />
      </Suspense>
      <TestDB />
    </>
  );
};

export default Homepage;
