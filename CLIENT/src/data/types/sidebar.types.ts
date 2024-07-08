import { LucideProps } from 'lucide-react';
import { ForwardRefExoticComponent, RefAttributes } from 'react';

export type SidebarContentType = {
  title: string;
  items: SidebarItemType[];
};

export type SidebarItemType = {
  icon: ForwardRefExoticComponent<
    Omit<LucideProps, 'ref'> & RefAttributes<SVGSVGElement>
  >;
  url?: string;
  label: string;
  isSelected: boolean;
  onClick?: () => void;
  subItems: SubSidebarItemType[];
};

export type SubSidebarItemType = {
  url: string;
  label: string;
};
