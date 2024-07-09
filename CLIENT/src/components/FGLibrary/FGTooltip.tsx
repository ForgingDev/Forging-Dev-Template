import { Tooltip } from 'primereact/tooltip';
import { FC } from 'react';

type Props = {
  target: string;
  content: string;
};

const FGTooltip: FC<Props> = ({ content, target }) => {
  return (
    <Tooltip
      target={target}
      content={content}
      pt={{
        text: {
          className:
            'bg-zinc-700/80 glass-effect w-max text-sm py-1.5 px-2 rounded-lg shadow-md text-zinc-200',
        },
      }}
    />
  );
};

export default FGTooltip;
