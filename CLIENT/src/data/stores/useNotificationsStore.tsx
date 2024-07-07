import { ToastMessage } from 'primereact/toast';
import { create } from 'zustand';

type TNotificationsState = {
  noitification: ToastMessage | undefined;
};

type Actions = {
  showNotification: (type: ToastMessage) => void;
};

export const useNotificationsStore = create<TNotificationsState & Actions>()(
  set => ({
    noitification: undefined,
    showNotification: type => set({ noitification: type }),
  })
);
