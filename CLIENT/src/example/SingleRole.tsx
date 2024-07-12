import { getRole } from '@/actions/roles/getRole.action';
import { FC } from 'react';

const SingleRole: FC = async () => {
  const { data: role, error } = await getRole(
    '00000000-0000-0000-0000-000000000000'
  );

  if (error) {
    return <div>{error}</div>;
  }

  if (!role) {
    return <div>Role not found</div>;
  }

  return <div>{JSON.stringify(role)}</div>;
};

export default SingleRole;
