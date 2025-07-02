import { useState, useEffect, useCallback } from 'react';
import { TaskService } from '../api/services/taskService';
import { mapApiTasksToUI, type TaskData } from '../utils/taskMappers';

interface UseTaskDataResult {
  tasks: TaskData[];
  loading: boolean;
  error: string | null;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  refetch: () => Promise<void>;
  updateTaskStatus: (taskId: number, isCompleted: boolean) => Promise<void>;
}

interface UseTaskDataOptions {
  pageIndex: number;
  pageSize: number;
  dueOn?: string;
  filters?: {
    farmTaskType?: number;
    isCompleted?: boolean;
  };
  logicalOperator?: number;
  sort?: string;
}

export const useTaskData = ({
  pageIndex,
  pageSize,
  dueOn,
  filters = {},
  logicalOperator = 0,
  sort = '',
}: UseTaskDataOptions): UseTaskDataResult => {
  const [tasks, setTasks] = useState<TaskData[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);

  const fetchTasks = useCallback(async () => {
    setLoading(true);
    setError(null);
    
    try {
      const response = await TaskService.getTasks({
        pageIndex: pageIndex + 1,
        pageSize,
        dueOn,
        logicalOperator,
        ...filters,
        ...(sort ? { sort } : {}),
      });

      const uiTasks = mapApiTasksToUI(response.items);
      setTasks(uiTasks);
      setTotalPages(response.totalPages);
      setTotalCount(response.totalPages * pageSize);
      setHasNextPage(response.hasNextPage);
      setHasPreviousPage(response.hasPreviousPage);
    } catch (err) {
      console.error('Error fetching tasks:', err);
      setError('Failed to load tasks. Please try again.');
      setTasks([]);
      setTotalCount(0);
      setTotalPages(0);
      setHasNextPage(false);
      setHasPreviousPage(false);
    } finally {
      setLoading(false);
    }
  }, [pageIndex, pageSize, dueOn, filters, logicalOperator, sort]);

  const updateTaskStatus = useCallback(async (taskId: number, isCompleted: boolean) => {
    try {
      await TaskService.updateTaskStatus(taskId, isCompleted);
      
      // Update local state
      setTasks(prevTasks => 
        prevTasks.map(task => 
          task.id === taskId 
            ? { ...task, isCompleted }
            : task
        )
      );
    } catch (err) {
      console.error('Error updating task status:', err);
      throw err;
    }
  }, []);

  useEffect(() => {
    fetchTasks();
  }, [fetchTasks]);

  return {
    tasks,
    loading,
    error,
    totalCount,
    totalPages,
    hasNextPage,
    hasPreviousPage,
    refetch: fetchTasks,
    updateTaskStatus,
  };
};
