import { IdNameModel } from './common.models';

export type CreateUserModel = {
  username: string | null;
  email: string[];
  phoneNumber: string[];
  id: string;
  firstName: string | null;
  lastName: string | null;
  imageUrl: string;
  roles: IdNameModel[];
};

export type UpdateUserModel = Omit<CreateUserModel, 'id' | 'roles'> & {
  roles?: IdNameModel[];
};
