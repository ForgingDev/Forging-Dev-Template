import { Roles } from '@/data/models/role.models';

const endpoint = 'http://localhost:5296';
const BASE_URL = 'roles';

export async function getAllRolesRequest(): Promise<Response> {
  const response = await fetch(`${endpoint}/${BASE_URL}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
    next: {
      tags: ['get-all-roles'],
    },
  });

  return response;
}

export async function getRoleRequest(id: string): Promise<Response> {
  const response = await fetch(`${endpoint}/${BASE_URL}/${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
    next: {
      tags: ['get-role'],
    },
  });

  return response;
}

export async function addRoleRequest(roleName: Roles): Promise<void> {
  const response = await fetch(`${endpoint}/${BASE_URL}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ roleName }),
  });

  return response.json();
}

export async function updateRoleRequest(
  name: string,
  id: Roles
): Promise<void> {
  const response = await fetch(`${endpoint}/${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ name }),
  });

  return response.json();
}

export async function deleteRoleRequest(id: Roles): Promise<void> {
  const response = await fetch(`${endpoint}/${BASE_URL}/${id}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  return response.json();
}
