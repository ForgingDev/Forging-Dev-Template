'use client';

import useAddRoleAction from '@/actions/roles/addRole/useAddRole.action';
import { Roles } from '@/data/models/roles.models';
import useUserRoles from '@/hooks/useUserRoles';
import { FC } from 'react';

const AddRole: FC = () => {
  const { addingRole, handleAddRole } = useAddRoleAction();
  const { hasAccess } = useUserRoles([Roles.Admin]);

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
