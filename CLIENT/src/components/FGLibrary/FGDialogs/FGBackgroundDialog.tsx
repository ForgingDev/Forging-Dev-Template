'use client';

import { useGlobalDialogsStore } from '@/data/stores/useGlobalDialogsStore';
import { GlobalDialogsEnum } from '@/data/types/global-dialogs.types';
import { FC } from 'react';

const FGBackgroundDialog: FC = () => {
  const { dialog, hideDialog } = useGlobalDialogsStore();

  if (dialog !== GlobalDialogsEnum.BACKGROUND_DIALOG) return null;

  return (
    <div
      onClick={() => {
        hideDialog();
      }}
      className='bg-off fixed inset-0 min-h-screen w-screen bg-black bg-opacity-50 md:hidden'
    />
  );
};

export default FGBackgroundDialog;
