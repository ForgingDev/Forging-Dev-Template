import { ROUTES } from '@/lib/routes';
import {
  BarChart4,
  CircleHelp,
  ClipboardList,
  House,
  Settings,
  TriangleAlert,
  Users,
} from 'lucide-react';
import { SidebarContentType } from './types/sidebar.types';

export const SIDEBAR_ITEMS: SidebarContentType[] = [
  {
    title: 'Menu',
    items: [
      {
        icon: House,
        label: 'Home',
        url: ROUTES.HOME_ROUTE,
        isSelected: false,
        subItems: [],
      },
      {
        label: 'Activity',
        icon: BarChart4,
        isSelected: false,
        subItems: [
          {
            label: 'Overview',
            url: ROUTES.OVERVIEW_ROUTE,
          },
          {
            label: 'Analytics',
            url: ROUTES.ANALYTICS_ROUTE,
          },
          {
            label: 'Projects',
            url: ROUTES.PROJECTS_ROUTE,
          },
        ],
      },
      {
        label: 'Tasks',
        icon: ClipboardList,
        isSelected: false,
        subItems: [
          {
            label: 'Overview',
            url: ROUTES.OVERVIEW_ROUTE,
          },
          {
            label: 'Analytics',
            url: ROUTES.ANALYTICS_ROUTE,
          },
          {
            label: 'Projects',
            url: ROUTES.PROJECTS_ROUTE,
          },
        ],
        // url: ROUTES.TASKS_ROUTE,
      },
      {
        label: 'Reporting',
        icon: TriangleAlert,
        isSelected: false,
        subItems: [],
        url: ROUTES.REPORTING_ROUTE,
      },
      {
        label: 'Users',
        isSelected: false,
        icon: Users,
        subItems: [],
        url: ROUTES.USERS_ROUTE,
      },
    ],
  },
  {
    title: 'Other',
    items: [
      {
        icon: CircleHelp,
        label: 'Help',
        subItems: [],
        isSelected: false,
        url: ROUTES.HELP_ROUTE,
      },
      {
        icon: Settings,
        label: 'Settings',
        subItems: [],
        isSelected: false,
        url: ROUTES.SETTINGS_ROUTE,
      },
    ],
  },
];
