import { Card, CardContent, Box, Typography, Chip, IconButton } from '@mui/material';
import { CheckCircle, Cancel } from '@mui/icons-material';
import { type TaskData } from '../../data/mockTaskData';
import { getFarmTaskTypeColor, getFarmTaskTypeLabel } from '../../types/FarmTaskType';

interface TaskCardProps {
    task: TaskData;
    onToggleComplete?: (taskId: string, newStatus: boolean) => void;
}

const TaskCard: React.FC<TaskCardProps> = ({ task, onToggleComplete }) => {
    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return date.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    const handleToggleComplete = () => {
        if (onToggleComplete) {
            onToggleComplete(task.taskId, !task.isCompleted);
        }
    };

    return (
        <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
            <CardContent sx={{ flex: 1 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <Typography variant="h6" component="div">
                        Task ID: {task.taskId}
                    </Typography>
                    
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <Chip 
                            label={getFarmTaskTypeLabel(task.taskType)} 
                            color={getFarmTaskTypeColor(task.taskType)} 
                            size="small" 
                        />
                        <IconButton 
                            onClick={handleToggleComplete}
                            size="small"
                            sx={{ p: 0.5 }}
                        >
                            {task.isCompleted ? (
                                <CheckCircle sx={{ color: 'success.main' }} />
                            ) : (
                                <Cancel sx={{ color: 'error.main' }} />
                            )}
                        </IconButton>
                    </Box>
                </Box>
                
                <Typography 
                    variant="body2" 
                    sx={{ 
                        mb: 2, 
                        minHeight: '2.5em',
                        display: '-webkit-box',
                        WebkitLineClamp: 2,
                        WebkitBoxOrient: 'vertical',
                        overflow: 'hidden'
                    }}
                >
                    {task.message}
                </Typography>
                
                <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2 }}>
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
                            color={new Date(task.dueOn) < new Date() && !task.isCompleted ? 'error.main' : 'inherit'}
                        >
                            {formatDate(task.dueOn)}
                        </Typography>
                    </Box>
                </Box>
            </CardContent>
        </Card>
    );
};

export default TaskCard;
