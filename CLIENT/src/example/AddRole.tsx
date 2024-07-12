'use client';

import useAddRoleAction from '@/actions/roles/addRole/useAddRole.action';
import { FC } from 'react';

const AddRole: FC = () => {
  const { addingRole, handleAddRole } = useAddRoleAction();

  return (
    <button
      onClick={handleAddRole}
      disabled={addingRole}>
      {addingRole ? 'Adding role...' : 'Add new role'}
    </button>
  );
};

export default AddRole;
