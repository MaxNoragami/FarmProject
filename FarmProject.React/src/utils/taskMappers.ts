import { type ApiTaskDto } from "../api/types/taskTypes";
import { FarmTaskType } from "../types/FarmTaskType";

export interface TaskData {
  id: number;

  taskType: FarmTaskType;
  message: string;
  isCompleted: boolean;
  createdOn: string;
  dueOn: string;
  cageId: number | null;
}

const mapTaskTypeFromApi = (apiType: number): FarmTaskType => {
  switch (apiType) {
    case 0:
      return FarmTaskType.NestPreparation;
    case 1:
      return FarmTaskType.NestRemoval;
    case 2:
      return FarmTaskType.Weaning;
    case 3:
      return FarmTaskType.OffspringSeparation;
    default:
      return FarmTaskType.NestPreparation;
  }
};

export const mapApiTaskToUI = (apiTask: ApiTaskDto): TaskData => {
  return {
    id: apiTask.id,
    taskType: mapTaskTypeFromApi(apiTask.farmTaskType),
    message: apiTask.message,
    isCompleted: apiTask.isCompleted,
    createdOn: apiTask.createdOn,
    dueOn: apiTask.dueOn,
    cageId: apiTask.cageId,
  };
};

export const mapApiTasksToUI = (apiTasks: ApiTaskDto[]): TaskData[] => {
  return apiTasks.map(mapApiTaskToUI);
};
