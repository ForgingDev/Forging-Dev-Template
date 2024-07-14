import { getAllRoles } from '@/actions/roles/getAllRoles.action';
import { getUserAccess } from '@/actions/users/getUserAccess.action';
import { Roles } from '@/data/models/roles.models';
import { FC } from 'react';

const RolesList: FC = async () => {
  const { data: roles, error } = await getAllRoles();
  const { hasAccess } = await getUserAccess([Roles.Admin]);

  if (!hasAccess) {
    return <div>Unauthorized</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  if (!roles) {
    return <div>No roles found</div>;
  }

  return <ul>{JSON.stringify(roles)}</ul>;
};

export default RolesList;
