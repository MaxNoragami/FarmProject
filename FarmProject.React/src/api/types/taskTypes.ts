export interface ApiTaskDto {
  id: number;
  farmTaskType: number;
  message: string;
  isCompleted: boolean;
  createdOn: string;
  dueOn: string;
  cageId: number | null; 
}

export interface TaskListRequest {
  pageIndex: number;
  pageSize: number;
  dueOn?: string;
}
