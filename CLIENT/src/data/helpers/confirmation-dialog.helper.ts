import { ConfirmDialogProps, confirmDialog } from 'primereact/confirmdialog';

const showConfirmationDialog = ({
  icon,
  defaultFocus,
  accept,
  reject,
  message,
  header,
}: ConfirmDialogProps): void => {
  confirmDialog({
    message,
    header,
    icon,
    defaultFocus,
    accept,
    reject,
    dismissableMask: true,
    rejectClassName: 'px-4',
  });
};

export const ConfirmationDialogHelper = {
  showConfirmationDialog,
};
