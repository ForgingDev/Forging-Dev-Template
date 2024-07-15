import { ToastMessage } from 'primereact/toast';
import { create } from 'zustand';

type NotificationsStoreStateType = {
  noitification:
    | Pick<ToastMessage, 'severity' | 'summary' | 'detail'>
    | undefined;
};

type NotificationsStoreActionsType = {
  showNotification: (
    type: Pick<ToastMessage, 'severity' | 'summary' | 'detail'>
  ) => void;
};

export const useNotificationsStore = create<
  NotificationsStoreStateType & NotificationsStoreActionsType
>()(set => ({
  noitification: undefined,
  showNotification: type => set({ noitification: type }),
}));
