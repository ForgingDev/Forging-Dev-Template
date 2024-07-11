import { IdNameModel } from '@/data/models/common.models';
import { Roles } from '@/data/models/role.models';

const endpoint = 'https://forging-dev-api.fly.dev';
const BASE_URL = 'roles';

export async function getRoles(): Promise<IdNameModel<Roles>[]> {
  const res = await fetch(`${endpoint}/${BASE_URL}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  return res.json();
}

export async function addRole(role: IdNameModel<Roles>): Promise<void> {
  await fetch(`${endpoint}/${BASE_URL}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ ...role }),
  });
}

export async function updateRole(name: string, id: Roles): Promise<void> {
  await fetch(`${endpoint}/${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ name }),
  });
}

export async function deleteRole(id: Roles): Promise<void> {
  await fetch(`${endpoint}/${BASE_URL}/${id}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
  });
}
