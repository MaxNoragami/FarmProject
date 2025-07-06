import { TableCell } from "@mui/material";
import { type RabbitData } from "../../utils/rabbitMappers";
import type { Column } from "../../constants/rabbitColumns";

interface RabbitTableRowProps {
  rabbit: RabbitData;
  columns: readonly Column[];
}

const RabbitTableRow: React.FC<RabbitTableRowProps> = ({ rabbit, columns }) => {
  return (
    <>
      {columns.map((column) => {
        const value = rabbit[column.id as keyof RabbitData];
        return (
          <TableCell key={column.id} align={column.align}>
            {value}
          </TableCell>
        );
      })}
    </>
  );
};

export default RabbitTableRow;
