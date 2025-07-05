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
  cageId: number | null; 
}


const farmTaskTypeEnumToString: Record<number, FarmTaskType> = {
  0: FarmTaskType.NestPreparation,
  1: FarmTaskType.NestRemoval,
  2: FarmTaskType.Weaning,
  3: FarmTaskType.OffspringSeparation,
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
    cageId: apiTask.cageId ?? null, 
  };
};

export const mapApiTasksToUI = (apiTasks: ApiTaskDto[]): TaskData[] => {
  return apiTasks.map(mapApiTaskToUI);
};
