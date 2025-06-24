import { Typography } from '@mui/material';
import { Helmet } from 'react-helmet-async';

const PairsPage = () => {
    return (
        <>
            <Typography variant="h5" sx={{ mb: 2 }}>
                Breeding Pairs Management
            </Typography>
            <Typography variant="body1" color="text.secondary">
                Manage your rabbit breeding pairs here.
            </Typography>
        </>
    );
};

export default PairsPage;
