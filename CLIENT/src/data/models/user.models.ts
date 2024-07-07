import { UserJSON } from '@clerk/nextjs/server';

export type UserModel = Pick<
  UserJSON,
  'username' | 'first_name' | 'last_name' | 'id' | 'image_url'
> & {
  email_addresses: string[];
  phone_numbers: string[];
};
