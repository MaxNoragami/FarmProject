import { Typography } from '@mui/material';
import { Helmet } from 'react-helmet-async';

const CagesPage = () => {
    return (
        <>
            <Typography variant="h5" sx={{ mb: 2 }}>
                Cages Management
            </Typography>
            <Typography variant="body1" color="text.secondary">
                Manage your cage inventory here.
            </Typography>
        </>
    );
};

export default CagesPage;
