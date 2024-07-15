import { MOBILE_BREAKPOINT } from '@/data/constants';
import { useEffect, useState } from 'react';

type ReturnProps = {
  isDesktopView: boolean;
};

const useViewport = (): ReturnProps => {
  const [isDesktopView, setIsDesktopView] = useState(false);

  useEffect(() => {
    const handleResize = () => {
      setIsDesktopView(window.innerWidth > MOBILE_BREAKPOINT);
    };

    window.addEventListener('resize', handleResize);

    handleResize();

    return () => window.removeEventListener('resize', handleResize);
  }, []);

  return { isDesktopView };
};

export default useViewport;
