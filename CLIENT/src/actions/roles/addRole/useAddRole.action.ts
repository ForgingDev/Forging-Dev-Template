import { Roles } from '@/data/models/roles.models';
import { useNotificationsStore } from '@/data/stores/useNotificationsStore';
import { useState } from 'react';
import { addRole } from './addRole.action';

type ReturnProps = {
  handleAddRole: (role: Roles) => void;
  addingRole: boolean;
};

const useAddRoleAction = (): ReturnProps => {
  const { showNotification } = useNotificationsStore();

  const [addingRole, setAddingRole] = useState(false);

  const handleAddRole = async (role: Roles) => {
    setAddingRole(true);
    const { error, success } = await addRole(role);

    if (error) {
      showNotification({
        detail: 'Error',
        severity: 'error',
        summary: error,
      });
    } else if (success) {
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
