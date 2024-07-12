'use server';

import { POSTResponseDataModel } from '@/data/models/common.models';
import { Roles } from '@/data/models/role.models';
import { addRoleRequest } from '@/services/roles.service';
import { revalidateTag } from 'next/cache';

export const addRole = async (
  roleName: Roles
): Promise<POSTResponseDataModel> => {
  const result: POSTResponseDataModel = {
    success: undefined,
    error: undefined,
  };

  try {
    await addRoleRequest(roleName);

    result.success = 'Role added';

    revalidateTag('get-all-roles');
  } catch (error) {
    result.error = 'Failed to add role';

    console.error(result.error, error);
  }

  return result;
};
