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

// Map farm task type number to string
const farmTaskTypeEnumToString: Record<number, FarmTaskType> = {
  0: FarmTaskType.NestPreparation,
};

export const mapApiTaskToUI = (apiTask: ApiTaskDto): TaskData => {
  return {
    id: apiTask.id,
    taskId: apiTask.id.toString(),
    taskType: farmTaskTypeEnumToString[apiTask.farmTaskType] || FarmTaskType.NestPreparation,
    message: apiTask.message,
    isCompleted: apiTask.isCompleted,
    createdOn: apiTask.createdOn,
    dueOn: apiTask.dueOn,
  };
};

export const mapApiTasksToUI = (apiTasks: ApiTaskDto[]): TaskData[] => {
  return apiTasks.map(mapApiTaskToUI);
};
