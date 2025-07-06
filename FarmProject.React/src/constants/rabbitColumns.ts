export interface Column {
  id: string;
  label: string;
  minWidth?: number;
  align?: "right" | "center";
  sortable?: boolean;
  numeric?: boolean;
  filterable?: boolean;
}

export const rabbitColumns: readonly Column[] = [
  {
    id: "number",
    label: "#",
    minWidth: 50,
    align: "center",
    sortable: false,
    filterable: false,
  },
  {
    id: "rabbitId",
    label: "RABBIT ID",
    minWidth: 100,
    align: "center",
    sortable: true,
    numeric: true,
    filterable: false,
  },
  {
    id: "name",
    label: "NAME",
    minWidth: 150,
    sortable: true,
    numeric: false,
    filterable: true,
  },
  {
    id: "cageId",
    label: "CAGE ID",
    minWidth: 100,
    align: "center",
    sortable: false,
    numeric: true,
    filterable: false,
  },
  {
    id: "status",
    label: "STATUS",
    minWidth: 120,
    align: "center",
    sortable: true,
    numeric: false,
    filterable: true,
  },
];

export const getSortableColumns = () => {
  return rabbitColumns.filter((col) => col.sortable);
};
