import { getUserRoles } from '@/actions/users/getUserRoles.action';
import { IdNameModel } from '@/data/models/common.models';
import { Roles } from '@/data/models/roles.models';
import { useAuth } from '@clerk/nextjs';
import { useEffect, useState } from 'react';

type ReturnProps = {
  hasAccess: boolean;
};

const useUserAccess = (requiredRoles: Roles[]): ReturnProps => {
  const { userId } = useAuth();

  const [userRoles, setUserRoles] = useState<IdNameModel<Roles>[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      const { data: userRoles, error } = await getUserRoles(userId);

      if (error) {
        console.error(error);

        return;
      }

      setUserRoles(userRoles || []);
    };

    fetchData();
  }, [userId]);

  const hasAccess = !!userRoles?.find(role => requiredRoles.includes(role.id));

  return { hasAccess };
};

export default useUserAccess;
