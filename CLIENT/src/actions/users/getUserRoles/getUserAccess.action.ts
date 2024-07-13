import { Roles } from '@/data/models/roles.models';
import { auth } from '@clerk/nextjs/server';
import { getUserRoles } from '../getUserRoles.action';

export const getUserAccess = async (requiredRoles: Roles[]) => {
  const { userId } = auth();
  const { data: userRoles } = await getUserRoles(userId);

  return {
    hasAccess: !!userRoles?.find(role => requiredRoles.includes(role.id)),
  };
};
