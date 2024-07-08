import { create } from 'zustand';
import { GlobalDialogsEnum } from '../types/global-dialogs.types';

type TGlobalDialogsState = {
  dialog: GlobalDialogsEnum | undefined;
};

type Actions = {
  showDialog: (type: GlobalDialogsEnum) => void;
  hideDialog: () => void;
};

export const useGlobalDialogsStore = create<TGlobalDialogsState & Actions>()(
  set => ({
    dialog: undefined,
    showDialog: type => set({ dialog: type }),
    hideDialog: () => set({ dialog: undefined }),
  })
);
