import { Roles } from '@/data/models/roles.models';
import { auth } from '@clerk/nextjs/server';
import { getUserRoles } from './getUserRoles.action';

type ReturnProps = {
  hasAccess: boolean;
};

export const getUserAccess = async (
  requiredRoles: Roles[]
): Promise<ReturnProps> => {
  const { userId } = auth();
  const { data: userRoles } = await getUserRoles(userId);

  return {
    hasAccess: !!userRoles?.find(role => requiredRoles.includes(role.id)),
  };
};
