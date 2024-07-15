'use server';

import { POSTResponseDataModel } from '@/data/models/common.models';
import { Roles } from '@/data/models/roles.models';
import { updateRoleRequest } from '@/services/roles.service';
import { revalidateTag } from 'next/cache';

export const updateRole = async (
  name: string,
  id: Roles
): Promise<POSTResponseDataModel> => {
  const result: POSTResponseDataModel = {
    success: undefined,
    error: undefined,
  };

  try {
    await updateRoleRequest(id, name);

    result.success = 'Role updated';

    revalidateTag('get-role');
    revalidateTag('get-all-roles');
  } catch (error) {
    result.error = 'Failed to update role';

    console.error(result.error, error);
  }

  return result;
};
