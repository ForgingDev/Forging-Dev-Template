import { GETResponseDataModel, IdNameModel } from '@/data/models/common.models';
import { Roles } from '@/data/models/role.models';
import { getRoleRequest } from '@/services/roles.service';

export const getRole = async (
  id: Roles
): Promise<GETResponseDataModel<IdNameModel<Roles>>> => {
  const result: GETResponseDataModel<IdNameModel<Roles>> = {
    data: undefined,
    error: undefined,
  };

  try {
    const response = await getRoleRequest(id);

    if (!response.ok) {
      throw new Error(`Failed to get role with id : ${id}`);
    }

    result.data = await response.json();
  } catch (error) {
    result.error = `Failed to get role with id : ${id}`;

    console.error(result.error, error);
  }

  return result;
};
