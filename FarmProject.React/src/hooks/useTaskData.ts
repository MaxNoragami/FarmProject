import { useState, useEffect, useCallback } from "react";
import { TaskService } from "../api/services/taskService";
import { OrderService } from "../api/services/orderService";
import { mapApiTasksToUI, type TaskData } from "../utils/taskMappers";
import { mapApiOrdersToUI, type OrderData } from "../utils/orderMappers";

interface UseTaskDataResult {
  tasks: TaskData[];
  orders: OrderData[];
  loading: boolean;
  error: string | null;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  refetch: () => Promise<void>;
  completeTask: (
    taskId: number,
    newCageId?: number,
    otherCageId?: number,
    femaleOffspringCount?: number
  ) => Promise<void>;
}

interface UseTaskDataOptions {
  pageIndex: number;
  pageSize: number;
  dueOn?: string;
  filters?: {
    farmTaskType?: number;
    isCompleted?: boolean;
    orderStatus?: number;
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
  sort = "",
}: UseTaskDataOptions): UseTaskDataResult => {
  const [tasks, setTasks] = useState<TaskData[]>([]);
  const [orders, setOrders] = useState<OrderData[]>([]);
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
      const [taskResponse, orderResponse] = await Promise.all([
        TaskService.getTasks({
          pageIndex: pageIndex + 1,
          pageSize,
          dueOn,
          logicalOperator,
          farmTaskType: filters.farmTaskType,
          isCompleted: filters.isCompleted,
          ...(sort ? { sort } : {}),
        }),
        OrderService.getOrders({
          pageIndex: 1,
          pageSize: 100,
          filters: {
            orderDate: dueOn,
            ...(filters.orderStatus !== undefined
              ? { orderStatus: filters.orderStatus }
              : {}),
          },
          logicalOperator: 0,
        }),
      ]);

      const uiTasks = mapApiTasksToUI(taskResponse.items);
      const uiOrders = mapApiOrdersToUI(orderResponse.items);

      setTasks(uiTasks);
      setOrders(uiOrders);
      setTotalPages(taskResponse.totalPages);
      setTotalCount(taskResponse.totalPages * pageSize);
      setHasNextPage(taskResponse.hasNextPage);
      setHasPreviousPage(taskResponse.hasPreviousPage);
    } catch (err) {
      console.error("Error fetching tasks and orders:", err);
      setError("Failed to load tasks and orders. Please try again.");
      setTasks([]);
      setOrders([]);
      setTotalCount(0);
      setTotalPages(0);
      setHasNextPage(false);
      setHasPreviousPage(false);
    } finally {
      setLoading(false);
    }
  }, [pageIndex, pageSize, dueOn, filters, logicalOperator, sort]);

  const completeTask = useCallback(
    async (
      taskId: number,
      newCageId?: number,
      otherCageId?: number,
      femaleOffspringCount?: number
    ) => {
      try {
        const requestBody: any = {};

        if (newCageId !== undefined) {
          requestBody.newCageId = newCageId;
        }

        if (otherCageId !== undefined) {
          requestBody.otherCageId = otherCageId;
        }

        if (femaleOffspringCount !== undefined) {
          requestBody.femaleOffspringCount = femaleOffspringCount;
        }

        const updatedTask = await TaskService.completeTask(taskId, requestBody);

        setTasks((prevTasks) =>
          prevTasks.map((task) =>
            task.id === taskId
              ? {
                  ...task,
                  isCompleted: updatedTask.isCompleted,
                  message: updatedTask.message,
                  farmTaskType: updatedTask.farmTaskType,
                }
              : task
          )
        );
      } catch (err) {
        console.error("Error completing task:", err);
        throw err;
      }
    },
    []
  );

  useEffect(() => {
    fetchTasks();
  }, [fetchTasks]);

  return {
    tasks,
    orders,
    loading,
    error,
    totalCount,
    totalPages,
    hasNextPage,
    hasPreviousPage,
    refetch: fetchTasks,
    completeTask,
  };
};
