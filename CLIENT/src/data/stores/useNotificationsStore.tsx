import { ToastMessage } from 'primereact/toast';
import { create } from 'zustand';

type TNotificationsState = {
  noitification:
    | Pick<ToastMessage, 'severity' | 'summary' | 'detail'>
    | undefined;
};

type Actions = {
  showNotification: (
    type: Pick<ToastMessage, 'severity' | 'summary' | 'detail'>
  ) => void;
};

export const useNotificationsStore = create<TNotificationsState & Actions>()(
  set => ({
    noitification: undefined,
    showNotification: type => set({ noitification: type }),
  })
);
