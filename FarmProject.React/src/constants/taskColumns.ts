export interface TaskSortColumn {
  id: string;
  label: string;
  sortable?: boolean;
}

export const taskSortColumns: readonly TaskSortColumn[] = [
  { id: "taskId", label: "TASK ID", sortable: true },
  { id: "createdOn", label: "CREATED ON", sortable: true },
  { id: "dueOn", label: "DUE ON", sortable: true },
];

export const getSortableTaskColumns = () => {
  return taskSortColumns.filter((col) => col.sortable);
};
