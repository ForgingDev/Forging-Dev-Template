import { Roles } from '@/data/models/role.models';
import { useNotificationsStore } from '@/data/stores/useNotificationsStore';
import { useState } from 'react';
import { deleteRole } from './deleteRole.action';

type ReturnProps = {
  handleDeleteRole: (id: Roles) => void;
  deletingRole: boolean;
  error: string | undefined;
};

const useDeleteRoleAction = (): ReturnProps => {
  const { showNotification } = useNotificationsStore();

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | undefined>(undefined);

  const handleDeleteRole = async (id: Roles) => {
    setLoading(true);
    const { error, success } = await deleteRole(id);

    if (error) {
      showNotification({
        detail: error,
        severity: 'error',
        summary: 'Error',
      });

      setError(error);

      setLoading(false);

      return;
    }

    if (success) {
      showNotification({
        detail: 'Success',
        severity: 'success',
        summary: success,
      });

      setLoading(false);
    }
  };

  return {
    error,
    handleDeleteRole,
    deletingRole: loading,
  };
};

export default useDeleteRoleAction;
