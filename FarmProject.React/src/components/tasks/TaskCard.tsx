import { Card, CardContent, Box, Typography, IconButton } from "@mui/material";
import { CheckCircle, Cancel } from "@mui/icons-material";
import { type TaskData } from "../../utils/taskMappers";
import {
  FarmTaskType,
  getFarmTaskTypeColor,
  getFarmTaskTypeLabel,
} from "../../types/FarmTaskType";
import CompleteWeaningTaskModal from "../modals/CompleteWeaningTaskModal";
import CompleteOffspringSeparationTaskModal from "../modals/CompleteOffspringSeparationTaskModal";
import { useState } from "react";
import {
  type CompleteWeaningTaskFormFields,
  type CompleteOffspringSeparationTaskFormFields,
} from "../../schemas/taskSchemas";

const pastelColors: Record<string, string> = {
  NestPreparation: "#e3f2fd",
  NestRemoval: "#fff3e0",
  Weaning: "#e8f5e9",
  OffspringSeparation: "#f3e5f5",
};

const greyedOutColor = "#f5f5f5";

interface TaskCardProps {
  task: TaskData;
  onCompleteTask?: (
    taskId: number,
    newCageId?: number,
    otherCageId?: number,
    femaleOffspringCount?: number
  ) => Promise<void>;
}

const TaskCard: React.FC<TaskCardProps> = ({ task, onCompleteTask }) => {
  const [weaningModalOpen, setWeaningModalOpen] = useState(false);
  const [weaningError, setWeaningError] = useState<string | null>(null);
  const [separationModalOpen, setSeparationModalOpen] = useState(false);
  const [separationError, setSeparationError] = useState<string | null>(null);

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const handleCompleteTask = () => {
    if (task.isCompleted) return;

    if (task.taskType === FarmTaskType.Weaning) {
      setWeaningError(null);
      setWeaningModalOpen(true);
      return;
    }

    if (task.taskType === FarmTaskType.OffspringSeparation) {
      setSeparationError(null);
      setSeparationModalOpen(true);
      return;
    }

    if (onCompleteTask) {
      onCompleteTask(task.id);
    }
  };

  const handleCompleteWeaningTask = async (
    data: CompleteWeaningTaskFormFields
  ) => {
    setWeaningError(null);
    try {
      if (onCompleteTask) {
        await onCompleteTask(task.id, data.newCageId);

        setWeaningModalOpen(false);
      }
    } catch (err: any) {
      setWeaningError(
        err?.response?.data?.message ||
          err?.message ||
          "An unexpected error occurred while completing the task."
      );
    }
  };

  const handleCompleteOffspringSeparationTask = async (
    data: CompleteOffspringSeparationTaskFormFields
  ) => {
    setSeparationError(null);
    try {
      if (onCompleteTask) {
        const result = onCompleteTask(
          task.id,
          undefined,
          data.otherCageId,
          data.femaleOffspringCount
        );

        if (result instanceof Promise) {
          await result;
        }

        setSeparationModalOpen(false);
      }
    } catch (err: any) {
      console.error("Error completing offspring separation task:", err);
      setSeparationError(
        err?.response?.data?.message ||
          err?.message ||
          "An unexpected error occurred while completing the task."
      );
    }
  };

  const cardBg = task.isCompleted
    ? greyedOutColor
    : pastelColors[task.taskType] || "#e0e0e0";

  return (
    <>
      <Card
        sx={{
          height: "100%",
          display: "flex",
          flexDirection: "column",
          backgroundColor: cardBg,
          opacity: task.isCompleted ? 0.7 : 1,
          transition: "background-color 0.2s",
        }}
      >
        <CardContent sx={{ flex: 1 }}>
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              mb: 2,
            }}
          >
            <Typography variant="h6" component="div">
              {getFarmTaskTypeLabel(task.taskType)}
            </Typography>
            <IconButton
              onClick={handleCompleteTask}
              disabled={task.isCompleted}
              size="small"
              sx={{
                p: 0.5,
                cursor: task.isCompleted ? "default" : "pointer",
              }}
            >
              {task.isCompleted ? (
                <CheckCircle sx={{ color: "success.main" }} />
              ) : (
                <Cancel sx={{ color: "error.main" }} />
              )}
            </IconButton>
          </Box>

          <Typography
            variant="body2"
            sx={{
              mb: 2,
              minHeight: "2.5em",
              display: "-webkit-box",
              WebkitLineClamp: 2,
              WebkitBoxOrient: "vertical",
              overflow: "hidden",
            }}
          >
            {task.message}
          </Typography>

          <Box sx={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 2 }}>
            <Box>
              <Typography variant="body2" color="text.secondary">
                CREATED ON
              </Typography>
              <Typography variant="body2" fontWeight="medium">
                {formatDate(task.createdOn)}
              </Typography>
            </Box>
            <Box>
              <Typography variant="body2" color="text.secondary">
                DUE ON
              </Typography>
              <Typography
                variant="body2"
                fontWeight="medium"
                color={
                  new Date(task.dueOn) < new Date() && !task.isCompleted
                    ? "error.main"
                    : "inherit"
                }
              >
                {formatDate(task.dueOn)}
              </Typography>
            </Box>
          </Box>
        </CardContent>
      </Card>

      <CompleteWeaningTaskModal
        open={weaningModalOpen}
        onClose={() => setWeaningModalOpen(false)}
        onSubmit={handleCompleteWeaningTask}
        error={weaningError}
        task={task}
      />

      <CompleteOffspringSeparationTaskModal
        open={separationModalOpen}
        onClose={() => setSeparationModalOpen(false)}
        onSubmit={handleCompleteOffspringSeparationTask}
        error={separationError}
        task={task}
      />
    </>
  );
};

export default TaskCard;
