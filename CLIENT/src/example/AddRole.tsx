'use client';

import useAddRoleAction from '@/actions/roles/addRole/useAddRole.action';
import { Roles } from '@/data/models/roles.models';
import useUserAccess from '@/hooks/useUserAccess';
import { FC } from 'react';

const AddRole: FC = () => {
  const { addingRole, handleAddRole } = useAddRoleAction();
  const { hasAccess } = useUserAccess([Roles.Admin]);

  if (!hasAccess) {
    return <div>Unauthorized</div>;
  }

  return (
    <button
      onClick={handleAddRole}
      disabled={addingRole}>
      {addingRole ? 'Adding role...' : 'Add new role'}
    </button>
  );
};

export default AddRole;
