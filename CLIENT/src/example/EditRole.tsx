'use client';

import useUpdateRoleAction from '@/actions/roles/updateRole/useUpdateRole.action';
import { Roles } from '@/data/models/role.models';
import { FC } from 'react';

const EditRole: FC = () => {
  const { handleUpdateRole, updatingRole } = useUpdateRoleAction();

  return (
    <button
      onClick={() => handleUpdateRole('Apdeitid', Roles.User)}
      disabled={updatingRole}>
      {updatingRole
        ? 'Updating role...'
        : 'Update role with ID: "00000000-0000-0000-0000-000000000000"'}
    </button>
  );
};

export default EditRole;
