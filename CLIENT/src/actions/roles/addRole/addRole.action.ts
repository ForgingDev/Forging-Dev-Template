'use server';

import { POSTResponseDataModel } from '@/data/models/common.models';
import { Roles } from '@/data/models/roles.models';
import { addRoleRequest } from '@/services/roles.service';
import { revalidateTag } from 'next/cache';

export const addRole = async (name: Roles): Promise<POSTResponseDataModel> => {
  const result: POSTResponseDataModel = {
    success: undefined,
    error: undefined,
  };

  try {
    const response = await addRoleRequest(name);

    if (!response.ok) {
      throw new Error('Failed to add role');
    }

    result.success = 'Role added';

    revalidateTag('get-all-roles');
  } catch (error) {
    result.error = 'Failed to add role';

    console.error(result.error, error);
  }

  return result;
};
