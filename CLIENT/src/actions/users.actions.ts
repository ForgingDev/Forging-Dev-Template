import { IdNameModel } from '@/data/models/common.models';
import { Roles } from '@/data/models/role.models';
import { CreateUserModel, UpdateUserModel } from '@/data/models/user.models';

const endpoint = 'https://forging-dev-api.fly.dev';
const BASE_URL = 'users';

export async function createUser(user: CreateUserModel): Promise<void> {
  await fetch(`${endpoint}/${BASE_URL}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ ...user }),
  });
}

export async function updateUser(
  user: UpdateUserModel,
  id: string
): Promise<void> {
  await fetch(`${endpoint}/${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ ...user }),
  });
}

export async function deleteUser(id: string): Promise<void> {
  await fetch(`${endpoint}/${BASE_URL}/${id}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
  });
}

// ROLES

export async function getUserRoles(): Promise<IdNameModel[]> {
  const res = await fetch(`${endpoint}/${BASE_URL}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  return res.json();
}

export async function addUserRole(
  userId: string,
  roleId: Roles
): Promise<void> {
  await fetch(`${endpoint}/${BASE_URL}/${userId}/roles/${roleId}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
  });
}

export async function removeUserRole(
  userId: string,
  roleId: Roles
): Promise<void> {
  await fetch(`${endpoint}/${BASE_URL}/${userId}/roles/${roleId}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
  });
}
