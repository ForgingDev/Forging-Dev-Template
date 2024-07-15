'use server';

import { GETResponseDataModel, IdNameModel } from '@/data/models/common.models';
import { Roles } from '@/data/models/roles.models';
import { getUserRolesRequest } from '@/services/roles.service';

export const getUserRoles = async (
  id: string | null | undefined
): Promise<GETResponseDataModel<IdNameModel<Roles>[]>> => {
  const result: GETResponseDataModel<IdNameModel<Roles>[]> = {
    data: undefined,
    error: undefined,
  };

  if (!id) {
    result.error = 'User id is required to get roles';

    console.error(result.error);

    return result;
  }

  try {
    const response = await getUserRolesRequest(id);

    if (!response.ok) {
      throw new Error(`Failed to get user roles`);
    }

    result.data = await response.json();
  } catch (error) {
    result.error = `Failed to get user roles`;

    console.error(result.error, error);
  }

  return result;
};
