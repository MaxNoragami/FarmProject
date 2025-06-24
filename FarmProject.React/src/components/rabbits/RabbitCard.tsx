import { Card, CardContent, Box, Typography, Chip } from '@mui/material';
import type { RabbitData } from '../../data/mockData';

interface RabbitCardProps {
    rabbit: RabbitData;
}

const RabbitCard: React.FC<RabbitCardProps> = ({ rabbit }) => {
    return (
        <Card sx={{ mb: 2 }}>
            <CardContent>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
                    <Typography variant="h6" component="div">
                        {rabbit.name}
                    </Typography>
                    <Chip 
                        label={rabbit.status} 
                        color={rabbit.status === 'Available' ? 'success' : 'warning'} 
                        size="small" 
                    />
                </Box>
                <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 1 }}>
                    <Box>
                        <Typography variant="body2" color="text.secondary">
                            RABBIT ID
                        </Typography>
                        <Typography variant="body1">
                            {rabbit.rabbitId}
                        </Typography>
                    </Box>
                    <Box>
                        <Typography variant="body2" color="text.secondary">
                            CAGE ID
                        </Typography>
                        <Typography variant="body1">
                            {rabbit.cageId}
                        </Typography>
                    </Box>
                </Box>
            </CardContent>
        </Card>
    );
};

export default RabbitCard;
