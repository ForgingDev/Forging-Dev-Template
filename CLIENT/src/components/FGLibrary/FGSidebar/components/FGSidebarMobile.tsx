import { SIDEBAR_ITEMS } from '@/data/constants';
import {
  SidebarContentType,
  SidebarItemType,
} from '@/data/types/sidebar.types';
import { cn } from '@/lib/utils';
import { useClerk, useUser } from '@clerk/nextjs';
import { ChevronsRight, LogIn, LogOut } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';
import { Dispatch, FC, SetStateAction, useState } from 'react';
import FGSidebarItem from './FGSidebarItem';

type Props = {
  openedSideNavbar: boolean;
  setOpenedNavbar: Dispatch<SetStateAction<boolean>>;
};

const FGSidebarMobile: FC<Props> = ({
  openedSideNavbar,
  setOpenedNavbar,
}): JSX.Element => {
  const [sidebarItems, setSidebarItems] =
    useState<SidebarContentType[]>(SIDEBAR_ITEMS);

  const { user, isSignedIn } = useUser();
  const { openUserProfile, signOut, openSignIn } = useClerk();

  const handleNavItemClick = (label: string) => {
    setSidebarItems(prevItems =>
      prevItems.map(section => ({
        ...section,
        items: section.items.map(item =>
          item.label === label
            ? { ...item, isSelected: !item.isSelected }
            : { ...item, isSelected: false }
        ),
      }))
    );
  };

  return (
    <div
      className={cn(
        'fixed right-0 top-0 flex h-screen min-w-72 max-w-xs flex-col items-center overflow-hidden rounded-md bg-zinc-900 text-zinc-300 shadow-xl shadow-black transition-all duration-300 md:w-1/4',
        {
          'translate-x-0': openedSideNavbar,
          'translate-x-full': !openedSideNavbar,
        }
      )}>
      <div className='flex w-full items-center justify-between bg-zinc-800 p-5'>
        <Link
          href='/'
          className='font-medium text-zinc-300 transition-all hover:text-white'>
          Forging Dev
        </Link>
        <ChevronsRight
          className='cursor-pointer transition-all hover:text-white'
          onClick={() => setOpenedNavbar(false)}
        />
      </div>
      <div className='scrollbar h-screen w-full overflow-y-auto'>
        {sidebarItems.map((sidebarItems: SidebarContentType) => {
          const { title, items } = sidebarItems;

          return (
            <div
              key={title}
              className='flex flex-col gap-y-2 border-t border-zinc-600 border-opacity-50 p-4'>
              <div className='my-2 mb-4 text-sm font-semibold uppercase text-zinc-500'>
                {title}
              </div>
              {items.map((item: SidebarItemType) => {
                return (
                  <FGSidebarItem
                    handleNavItemClick={handleNavItemClick}
                    isNavbarItemSelected={item.isSelected}
                    item={item}
                    key={item.label}
                  />
                );
              })}
            </div>
          );
        })}
      </div>
      <div
        onClick={() => {
          if (isSignedIn) {
            openUserProfile();
          } else {
            openSignIn();
          }
        }}
        className='group mb-2 flex w-11/12 cursor-pointer items-center gap-x-2 rounded-lg bg-zinc-800 px-2 py-5 shadow-inner shadow-zinc-700'>
        {isSignedIn ? (
          <>
            <Image
              src={user.imageUrl}
              width={100}
              height={100}
              className='w-10 rounded-full object-cover object-center'
              alt={user.fullName || 'User Profile'}
            />
            <div className='flex flex-col gap-y-0.5'>
              <div className='text-sm text-white'>{user?.fullName}</div>
              <div className='max-w-full overflow-hidden text-xs text-zinc-300'>
                {user?.primaryEmailAddress?.emailAddress}
              </div>
            </div>
            <LogOut
              className='ml-auto'
              onClick={e => {
                e.stopPropagation();

                signOut();
              }}
            />
          </>
        ) : (
          <div className='mx-auto flex items-center gap-x-4 transition-all group-hover:text-white'>
            Log In
            <LogIn />
          </div>
        )}
      </div>
    </div>
  );
};

export default FGSidebarMobile;
