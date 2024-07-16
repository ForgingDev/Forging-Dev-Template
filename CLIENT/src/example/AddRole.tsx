'use client';

import useAddRoleAction from '@/actions/roles/addRole/useAddRole.action';
import { Roles } from '@/data/models/roles.models';
import useUserAccess from '@/hooks/useUserAccess';
import { sendGTMEvent } from '@next/third-parties/google';
import { FC } from 'react';

const AddRole: FC = () => {
  const { addingRole, handleAddRole } = useAddRoleAction();
  const { hasAccess } = useUserAccess([Roles.Admin]);

  if (!hasAccess) {
    return <div>Unauthorized</div>;
  }

  return (
    <button
      onClick={() => {
        sendGTMEvent({ event: 'buttonClicked', value: 'xyz' });

        handleAddRole(Roles.Admin);
      }}
      disabled={addingRole}>
      {addingRole ? 'Adding role...' : 'Add new role'}
    </button>
  );
};

export default AddRole;
