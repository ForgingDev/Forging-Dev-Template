import { sendGTMEvent } from '@next/third-parties/google';
import { FC, Suspense } from 'react';
import AddRole from './AddRole';
import DeleteRole from './DeleteRole';
import EditRole from './EditRole';
import RolesList from './RolesList';
import SingleRole from './SingleRole';

const RolesExample: FC = () => {
  return (
    <div>
      <button
        onClick={() => sendGTMEvent({ event: 'buttonClicked', value: 'xyz' })}>
        Test GTM Button
      </button>
      <div>Roles List</div>
      <Suspense fallback={<div>Loading roles list...</div>}>
        <RolesList />
      </Suspense>
      <br />
      <hr />
      <br />
      <div>Single Role</div>
      <Suspense fallback={<div>Loading single role...</div>}>
        <SingleRole />
      </Suspense>
      <br />
      <hr />
      <br />
      <AddRole />
      <br />
      <br />
      <EditRole />
      <br />
      <br />
      <DeleteRole />
    </div>
  );
};

export default RolesExample;
