import { getRole } from '@/actions/roles/getRole.action';
import { Roles } from '@/data/models/roles.models';
import { FC } from 'react';

const SingleRole: FC = async () => {
  const { data: role, error } = await getRole(Roles.User);

  if (error) {
    return <div>{error}</div>;
  }

  if (!role) {
    return <div>Role not found</div>;
  }

  return <div>{JSON.stringify(role)}</div>;
};

export default SingleRole;
