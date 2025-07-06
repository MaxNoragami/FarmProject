export interface PairSortColumn {
  id: string;
  label: string;
  sortable?: boolean;
}

export const pairSortColumns: readonly PairSortColumn[] = [
  { id: "pairId", label: "PAIR ID", sortable: true },
  { id: "femaleRabbitId", label: "FEMALE ID", sortable: true },
  { id: "maleRabbitId", label: "MALE ID", sortable: true },
  { id: "status", label: "STATUS", sortable: true },
  { id: "startDate", label: "START DATE", sortable: true },
  { id: "endDate", label: "END DATE", sortable: true },
];

export const getSortablePairColumns = () => {
  return pairSortColumns.filter((col) => col.sortable);
};
