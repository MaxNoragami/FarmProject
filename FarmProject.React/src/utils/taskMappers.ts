import { type ApiTaskDto } from '../api/types/taskTypes';
import { FarmTaskType } from '../types/FarmTaskType';

export interface TaskData {
  id: number;
  taskId: string;
  taskType: FarmTaskType;
  message: string;
  isCompleted: boolean;
  createdOn: string;
  dueOn: string;
}

// Map task type from API to UI enum
const mapTaskTypeFromApi = (apiType: number): FarmTaskType => {
  switch (apiType) {
    case 0:
      return FarmTaskType.NestPreparation;
    // Add other cases as needed
    default:
      return FarmTaskType.NestPreparation;
  }
};

export const mapApiTaskToUI = (apiTask: ApiTaskDto): TaskData => {
  return {
    id: apiTask.id,
    taskId: String(apiTask.id),
    taskType: mapTaskTypeFromApi(apiTask.farmTaskType),
    message: apiTask.message,
    isCompleted: apiTask.isCompleted,
    createdOn: apiTask.createdOn,
    dueOn: apiTask.dueOn
  };
};

export const mapApiTasksToUI = (apiTasks: ApiTaskDto[]): TaskData[] => {
  return apiTasks.map(mapApiTaskToUI);
};
