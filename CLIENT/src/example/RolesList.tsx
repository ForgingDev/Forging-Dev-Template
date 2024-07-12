import { getAllRoles } from '@/actions/roles/getAllRoles.action';
import { FC } from 'react';

const RolesList: FC = async () => {
  const { data: roles, error } = await getAllRoles();

  if (error) {
    return <div>{error}</div>;
  }

  if (!roles) {
    return <div>No roles found</div>;
  }

  return <ul>{JSON.stringify(roles)}</ul>;
};

export default RolesList;
