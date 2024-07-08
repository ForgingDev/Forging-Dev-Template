import {
  SidebarItemType,
  SubSidebarItemType,
} from '@/data/types/sidebar.types';
import { cn } from '@/lib/utils';
import { ChevronDown, ChevronUp } from 'lucide-react';
import Link from 'next/link';
import { FC } from 'react';
import FGSubSidebarItem from './FGSubSidebarItem';

type Props = {
  item: SidebarItemType;
  isNavbarItemSelected: boolean;
  handleNavItemClick: (label: string) => void;
};

const FGSidebarItem: FC<Props> = ({
  item,
  isNavbarItemSelected,
  handleNavItemClick,
}) => {
  const hasSubItems = !!item.subItems.length;

  return (
    <div className='flex flex-col'>
      <Link
        onClick={() => handleNavItemClick(item.label)}
        className={cn(
          'flex items-center justify-between gap-x-3 rounded-lg px-2.5 py-2.5 text-zinc-300 transition-all',
          {
            'bg-zinc-700 text-white shadow-sm': isNavbarItemSelected,
            'hover:bg-zinc-700/50 hover:text-white hover:shadow-sm':
              !isNavbarItemSelected,
          }
        )}
        href={!!item.url ? item.url : ''}>
        <div className='flex items-center gap-x-2'>
          <item.icon className='p-0.5' />
          <div>{item.label}</div>
        </div>
        {hasSubItems ? (
          isNavbarItemSelected ? (
            <ChevronUp />
          ) : (
            <ChevronDown />
          )
        ) : null}
      </Link>
      <div
        className={cn(
          'my-2 ml-4 space-y-2 overflow-hidden transition-all duration-500',
          {
            'max-h-0': !isNavbarItemSelected,
            'max-h-[200px]': isNavbarItemSelected, // Adjust 500px to suit your content's needs
          }
        )}>
        {item.subItems.map((subItem: SubSidebarItemType) => (
          <FGSubSidebarItem
            key={subItem.label}
            subItem={subItem}
          />
        ))}
      </div>
    </div>
  );
};

export default FGSidebarItem;
