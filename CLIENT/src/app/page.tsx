'use client';

import { createUser } from '@/actions/database.actions';
import { CreateUserModel } from '@/data/models/user.models';
import { FC } from 'react';

const Homepage: FC = () => {
  return (
    <div
      onClick={async () => {
        const user: CreateUserModel = {
          id: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
          email: ['pateu@gmail.com'],
          phoneNumber: ['+40727892022'],
          firstName: 'Pateu',
          imageUrl: 'string',
          lastName: 'Pateutz',
          username: 'Pateutz24',
        };

        try {
          const res = await createUser(user);

          console.log(res);
        } catch (error) {
          console.error('Error creating user:', error);
        }
      }}>
      Add user
    </div>
  );
};

export default Homepage;
