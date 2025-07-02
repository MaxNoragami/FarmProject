export interface ApiTaskDto {
  id: number;
  farmTaskType: number;
  message: string;
  isCompleted: boolean;
  createdOn: string;
  dueOn: string;
}

export interface TaskListRequest {
  pageIndex: number;
  pageSize: number;
  dueOn?: string;
}
