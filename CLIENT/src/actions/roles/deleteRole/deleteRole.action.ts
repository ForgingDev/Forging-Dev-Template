'use server';

import { POSTResponseDataModel } from '@/data/models/common.models';
import { Roles } from '@/data/models/roles.models';
import { deleteRoleRequest } from '@/services/roles.service';
import { revalidateTag } from 'next/cache';

export const deleteRole = async (id: Roles): Promise<POSTResponseDataModel> => {
  const result: POSTResponseDataModel = {
    success: undefined,
    error: undefined,
  };

  try {
    const response = await deleteRoleRequest(id);

    if (!response.ok) {
      throw new Error('Failed to delete role');
    }

    result.success = 'Role deleted successfully';

    revalidateTag('get-all-roles');
  } catch (error) {
    result.error = 'Failed to delete role';

    console.error(error);
  }

  return result;
};
