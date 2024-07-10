import { CreateUserModel, UpdateUserModel } from '@/data/models/user.models';

const endpoint = 'https://forging-dev-api.fly.dev';

export async function createUser(user: CreateUserModel): Promise<void> {
  await fetch(`${endpoint}/users`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ ...user, description: 'Delete later please' }),
  });
}

export async function updateUser(
  user: UpdateUserModel,
  id: string
): Promise<void> {
  await fetch(`${endpoint}/users/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ updateUserDto: user }),
  });
}

export async function deleteUser(id: string): Promise<void> {
  await fetch(`${endpoint}/users/${id}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
  });
}
