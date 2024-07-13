'use client';

import useUpdateRoleAction from '@/actions/roles/updateRole/useUpdateRole.action';
import { Roles } from '@/data/models/roles.models';
import { FC } from 'react';

const EditRole: FC = () => {
  const { handleUpdateRole, updatingRole } = useUpdateRoleAction();

  return (
    <button
      onClick={() => handleUpdateRole(Math.random().toString(), Roles.User)}
      disabled={updatingRole}>
      {updatingRole ? 'Updating role...' : 'Update role with ID: ' + Roles.User}
    </button>
  );
};

export default EditRole;
