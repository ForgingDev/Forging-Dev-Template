export type CreateUserModel = {
  username: string | null;
  email: string[];
  phoneNumber: string[];
  id: string;
  firstName: string | null;
  lastName: string | null;
  imageUrl: string;
};

export type UpdateUserModel = Omit<CreateUserModel, 'id'>;
