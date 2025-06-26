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

export function createTaskData(
  id: number,
  taskId: string,
  taskType: FarmTaskType,
  message: string,
  isCompleted: boolean,
  createdOn: string,
  dueOn: string,
): TaskData {
  return { id, taskId, taskType, message, isCompleted, createdOn, dueOn };
}

export const mockTasksData: TaskData[] = [
  createTaskData(1, '1', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #332', false, '2025-06-01T09:00:00', '2025-06-28T09:00:00'),
  createTaskData(444, '444', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #3332', false, '2025-06-01T10:00:00', '2025-06-28T10:00:00'),
  createTaskData(2, '2', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #349', true, '2025-06-02T08:00:00', '2025-06-29T08:00:00'),
  createTaskData(3, '3', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #105', false, '2025-06-03T10:30:00', '2025-06-30T10:30:00'),
  createTaskData(4, '4', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #201', true, '2025-06-04T14:00:00', '2025-07-01T14:00:00'),
  createTaskData(5, '5', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #505', false, '2025-06-05T11:00:00', '2025-07-02T11:00:00'),
  createTaskData(6, '6', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #077', true, '2025-06-06T09:30:00', '2025-07-03T09:30:00'),
  createTaskData(7, '7', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #123', false, '2025-06-07T15:00:00', '2025-07-04T15:00:00'),
  createTaskData(8, '8', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #456', true, '2025-06-08T16:00:00', '2025-07-05T16:00:00'),
  createTaskData(9, '9', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #789', false, '2025-06-09T13:00:00', '2025-07-06T13:00:00'),
  createTaskData(10, '10', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #111', false, '2025-06-10T07:00:00', '2025-07-07T07:00:00'),
  createTaskData(11, '11', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #222', true, '2025-06-11T10:00:00', '2025-07-08T10:00:00'),
  createTaskData(12, '12', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #333', false, '2025-06-12T08:00:00', '2025-07-09T08:00:00'),
  createTaskData(13, '13', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #444', true, '2025-06-13T11:30:00', '2025-07-10T11:30:00'),
  createTaskData(14, '14', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #555', false, '2025-06-14T09:15:00', '2025-07-11T09:15:00'),
  createTaskData(15, '15', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #666', true, '2025-06-15T13:45:00', '2025-07-12T13:45:00'),
  createTaskData(16, '16', FarmTaskType.NestPreparation, 'Prepare nest in cage for rabbit #777', false, '2025-06-16T10:00:00', '2025-07-13T10:00:00'),
];
