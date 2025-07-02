export interface CageSortColumn {
  id: string;
  label: string;
  sortable?: boolean;
}

export const cageSortColumns: readonly CageSortColumn[] = [
  { id: 'name', label: 'NAME', sortable: true },
  { id: 'cageId', label: 'CAGE ID', sortable: true },
  { id: 'rabbitId', label: 'RABBIT ID', sortable: true },
  { id: 'offspringCount', label: 'OFFSPRING COUNT', sortable: true },
  { id: 'offspringType', label: 'OFFSPRING TYPE', sortable: true },
];

export const getSortableCageColumns = () => {
    return cageSortColumns.filter(col => col.sortable);
};
