'use client';

import { useNotificationsStore } from '@/data/stores/useNotificationsStore';
import { Toast } from 'primereact/toast';
import { FC, useCallback, useEffect, useRef } from 'react';

const FGNotification: FC = () => {
  const toast = useRef<Toast>(null);
  const { noitification } = useNotificationsStore();

  const showNotification = useCallback(() => {
    if (!noitification) return;

    toast.current?.show(noitification);
  }, [noitification]);

  useEffect(() => {
    if (noitification) {
      showNotification();
    }
  }, [noitification, showNotification]);

  return <Toast ref={toast} />;
};

export default FGNotification;
