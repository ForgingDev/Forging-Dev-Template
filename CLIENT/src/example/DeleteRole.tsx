'use client';

import useDeleteRoleAction from '@/actions/roles/deleteRole/useDeleteRole.action';
import { Roles } from '@/data/models/role.models';
import { FC } from 'react';

const DeleteRole: FC = () => {
  const { handleDeleteRole, deletingRole, error } = useDeleteRoleAction();

  if (deletingRole) {
    return <p>Deleting role...</p>;
  }

  if (error) {
    return <p>Error: {error}</p>;
  }

  return (
    <button
      onClick={() => handleDeleteRole(Roles.TestRoleNew)}
      disabled={deletingRole}>
      Delete role with ID: {Roles.TestRoleNew}
    </button>
  );
};

export default DeleteRole;
