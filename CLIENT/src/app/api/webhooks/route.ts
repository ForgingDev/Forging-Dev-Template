import { createUser, deleteUser, updateUser } from '@/actions/database.actions';
import { CreateUserModel, UpdateUserModel } from '@/data/models/user.models';
import { WebhookEvent } from '@clerk/nextjs/server';
import { headers } from 'next/headers';
import { Webhook } from 'svix';

export async function POST(req: Request): Promise<Response> {
  // Get the body
  const payload = await req.json();
  const body = JSON.stringify(payload);

  // You can find this in the Clerk Dashboard -> Webhooks -> choose the endpoint
  const WEBHOOK_SECRET = process.env.WEBHOOK_SECRET;

  if (!WEBHOOK_SECRET) {
    throw new Error(
      'Please add WEBHOOK_SECRET from Clerk Dashboard to .env or .env.local'
    );
  }

  // Get the headers
  const headerPayload = headers();
  const svix_id = headerPayload.get('svix-id');
  const svix_timestamp = headerPayload.get('svix-timestamp');
  const svix_signature = headerPayload.get('svix-signature');

  // If there are no headers, error out
  if (!svix_id || !svix_timestamp || !svix_signature) {
    return new Response('Error occured -- no svix headers', {
      status: 400,
    });
  }

  // Create a new Svix instance with your secret.
  const wh = new Webhook(WEBHOOK_SECRET);

  let evt: WebhookEvent;

  // Verify the payload with the headers
  try {
    evt = wh.verify(body, {
      'svix-id': svix_id,
      'svix-timestamp': svix_timestamp,
      'svix-signature': svix_signature,
    }) as WebhookEvent;
  } catch (err) {
    console.error('Error verifying webhook:', err);
    return new Response('Error occured', {
      status: 400,
    });
  }

  const eventType = evt.type;

  if (eventType === 'user.created') {
    const user: CreateUserModel = {
      id: evt.data.id,
      username: evt.data.username,
      email: evt.data.email_addresses.map(email => email.email_address),
      phoneNumber: evt.data.phone_numbers.map(phone => phone.phone_number),
      firstName: evt.data.first_name,
      lastName: evt.data.last_name,
      imageUrl: evt.data.image_url,
      roles: [],
    };

    try {
      await createUser(user);
    } catch (error) {
      console.error('Error creating user:', error);

      return new Response('Error occured', {
        status: 400,
      });
    }
  } else if (eventType === 'user.updated') {
    const user: UpdateUserModel = {
      username: evt.data.username,
      email: evt.data.email_addresses.map(email => email.email_address),
      phoneNumber: evt.data.phone_numbers.map(phone => phone.phone_number),
      firstName: evt.data.first_name,
      lastName: evt.data.last_name,
      imageUrl: evt.data.image_url,
    };

    try {
      await updateUser(user, evt.data.id);
    } catch (error) {
      console.error('Error updating user:', error);

      return new Response('Error occured', {
        status: 400,
      });
    }
  } else if (eventType === 'user.deleted') {
    if (!evt.data.id)
      return new Response('User Id can not be undefined when deleting', {
        status: 400,
      });

    try {
      await deleteUser(evt.data.id);
    } catch (error) {
      console.error('Error updating user:', error);

      return new Response('Error occured', {
        status: 400,
      });
    }
  }

  return new Response('', { status: 200 });
}
