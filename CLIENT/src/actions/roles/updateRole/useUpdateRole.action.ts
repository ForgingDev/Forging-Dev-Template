import { Roles } from '@/data/models/roles.models';
import { useNotificationsStore } from '@/data/stores/useNotificationsStore';
import { useState } from 'react';
import { updateRole } from './updateRole.action';

type ReturnProps = {
  handleUpdateRole: (name: string, id: Roles) => void;
  updatingRole: boolean;
};

const useUpdateRoleAction = (): ReturnProps => {
  const { showNotification } = useNotificationsStore();

  const [updatingRole, setUpdatingRole] = useState(false);

  const handleUpdateRole = async (name: string, id: Roles) => {
    setUpdatingRole(true);
    const { error, success } = await updateRole(name, id);

    if (error) {
      showNotification({
        detail: 'Error',
        severity: 'error',
        summary: error,
      });

      return;
    }

    if (success) {
      showNotification({
        detail: 'Success',
        severity: 'success',
        summary: success,
      });
    }

    setUpdatingRole(false);
  };

  return {
    handleUpdateRole,
    updatingRole,
  };
};

export default useUpdateRoleAction;
