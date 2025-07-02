import { apiClient } from '../config';
import { type PaginatedResponse } from '../types/common';
import { type ApiTaskDto, type TaskListRequest } from '../types/taskTypes';

export interface TaskListFilters {
  dueOn?: string;
  farmTaskType?: number;
  isCompleted?: boolean;
  logicalOperator?: number;
  sort?: string;
}

export class TaskService {
  private static readonly BASE_PATH = '/farm-tasks';

  static async getTasks(request: TaskListRequest & TaskListFilters): Promise<PaginatedResponse<ApiTaskDto>> {
    const params = new URLSearchParams({
      pageIndex: request.pageIndex.toString(),
      pageSize: request.pageSize.toString(),
      LogicalOperator: (request.logicalOperator ?? 0).toString(),
    });

    if (request.dueOn) params.append('DueOn', request.dueOn);
    if (typeof request.farmTaskType === 'number') params.append('FarmTaskType', request.farmTaskType.toString());
    if (typeof request.isCompleted === 'boolean') params.append('IsCompleted', request.isCompleted.toString());
    if (request.sort) params.append('sort', request.sort);

    const response = await apiClient.get<PaginatedResponse<ApiTaskDto>>(
      `${this.BASE_PATH}?${params.toString()}`
    );
    
    return response.data;
  }

  static async completeTask(taskId: number): Promise<ApiTaskDto> {
    const response = await apiClient.put<ApiTaskDto>(`${this.BASE_PATH}/${taskId}`);
    
    return response.data;
  }
}

