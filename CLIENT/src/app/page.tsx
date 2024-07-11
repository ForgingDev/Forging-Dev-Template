'use client';

import { updateUser } from '@/actions/database.actions';
import { UpdateUserModel } from '@/data/models/user.models';
import { FC } from 'react';

const Homepage: FC = () => {
  return (
    <div
      onClick={async () => {
        const user: UpdateUserModel = {
          email: ['pizdamatii@gmail.com'],
          phoneNumber: ['+40727892123'],
          firstName: 'Pizda',
          imageUrl: 'ceimagevreicoaie',
          lastName: 'Matii',
          username: 'PulaCalulului',
        };

        try {
          const res = await updateUser(
            user,
            'user_2j6i9te3EXTPZdta6eInBBAVwZy'
          );

          console.log(res);
        } catch (error) {
          console.error('Error creating user:', error);
        }
      }}>
      Add user BLUD AGAIN again
    </div>
  );
};

export default Homepage;
