'use client';

import { useGlobalDialogsStore } from '@/data/stores/useGlobalDialogsStore';
import { GlobalDialogsEnum } from '@/data/types/global-dialogs.types';
import { ChevronsLeft } from 'lucide-react';
import { usePathname } from 'next/navigation';
import { useClickOutside } from 'primereact/hooks';
import { FC, useEffect, useRef, useState } from 'react';
import FGLogo from '../FGLogo';
import FGSidebarMobile from './components/FGSidebarMobile';

const FGSidebar: FC = () => {
  const [openedNavbar, setOpenedNavbar] = useState(false);
  const pathname = usePathname();
  const navbarRef = useRef(null);

  const { showDialog, hideDialog } = useGlobalDialogsStore();

  useClickOutside(navbarRef, () => {
    closeSidebar();
  });

  const closeSidebar = () => {
    setOpenedNavbar(false);
  };

  useEffect(() => {
    if (openedNavbar) {
      showDialog(GlobalDialogsEnum.BACKGROUND_DIALOG);
    } else {
      hideDialog();
    }
  }, [hideDialog, openedNavbar, showDialog]);

  useEffect(() => {
    closeSidebar();
  }, [pathname]);

  return (
    <nav
      ref={navbarRef}
      id='navbar'
      className='glass-effect border-b-1 bg-dark-purple fixed left-1/2 top-0 z-40 flex w-screen -translate-x-1/2 transform items-center justify-between border-zinc-200 border-opacity-15 bg-opacity-75 px-4 py-1 shadow-md'>
      <div className='container mx-auto flex items-center justify-between'>
        <FGLogo />
        <FGSidebarMobile
          openedSideNavbar={openedNavbar}
          setOpenedNavbar={setOpenedNavbar}
        />
        <ChevronsLeft
          id='chevron-left'
          className='cursor-pointer'
          aria-label='open-sidebar'
          onClick={() => setOpenedNavbar(true)}
        />
      </div>
    </nav>
  );
};

export default FGSidebar;
