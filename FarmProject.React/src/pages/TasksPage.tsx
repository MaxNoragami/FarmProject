import { Typography } from '@mui/material';
import { Helmet } from 'react-helmet-async';

const TasksPage = () => {
    return (
        <>
            <Helmet>
                <title>Tasks Management - Farm Project</title>
            </Helmet>
            
            <Typography variant="h5" sx={{ mb: 2 }}>
                Tasks Management
            </Typography>
            <Typography variant="body1" color="text.secondary">
                Manage your farm tasks here.
            </Typography>
        </>
    );
};

export default TasksPage;
