import { CreateUserModel, UpdateUserModel } from '@/data/models/users.models';

const endpoint = process.env.NEXT_PUBLIC_API_URL;
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
