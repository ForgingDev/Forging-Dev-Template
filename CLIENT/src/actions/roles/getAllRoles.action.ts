import { GETResponseDataModel, IdNameModel } from '@/data/models/common.models';
import { Roles } from '@/data/models/roles.models';
import { getAllRolesRequest } from '@/services/roles.service';

export const getAllRoles = async (): Promise<
  GETResponseDataModel<IdNameModel<Roles>[]>
> => {
  const result: GETResponseDataModel<IdNameModel<Roles>[]> = {
    data: undefined,
    error: undefined,
  };

  try {
    const response = await getAllRolesRequest();

    if (!response.ok) {
      throw new Error('Failed to get roles');
    }

    result.data = await response.json();
  } catch (error) {
    result.error = 'Failed to get roles';

    console.error(result.error, error);
  }

  return result;
};
