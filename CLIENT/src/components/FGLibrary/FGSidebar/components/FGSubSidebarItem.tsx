import { SubSidebarItemType } from '@/data/types/sidebar.types';
import { Circle } from 'lucide-react';
import Link from 'next/link';
import { FC } from 'react';

type Props = {
  subItem: SubSidebarItemType;
};

const FGSubSidebarItem: FC<Props> = ({ subItem }) => {
  const { label, url } = subItem;

  return (
    <Link
      href={url}
      className='flex items-center gap-x-4 rounded-lg px-2.5 py-2.5 text-zinc-300 transition-all hover:bg-zinc-700/50 hover:text-white'>
      <Circle className='p-1.5' />
      {label}
    </Link>
  );
};

export default FGSubSidebarItem;
