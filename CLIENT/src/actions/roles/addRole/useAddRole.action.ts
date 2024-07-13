import { useNotificationsStore } from '@/data/stores/useNotificationsStore';
import { useState } from 'react';
import { addRole } from './addRole.action';

type ReturnProps = {
  handleAddRole: () => void;
  addingRole: boolean;
};

const useAddRoleAction = (): ReturnProps => {
  const { showNotification } = useNotificationsStore();

  const [addingRole, setAddingRole] = useState(false);

  const handleAddRole = async () => {
    setAddingRole(true);
    const { error, success } = await addRole('Test Role New');

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

    setAddingRole(false);
  };

  return {
    handleAddRole,
    addingRole,
  };
};

export default useAddRoleAction;
