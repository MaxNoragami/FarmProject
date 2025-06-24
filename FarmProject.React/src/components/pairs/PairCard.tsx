import { Card, CardContent, Box, Typography, Chip } from '@mui/material';
import { type PairData } from '../../data/mockPairData';
import { getPairingStatusColor } from '../../types/PairingStatus';

interface PairCardProps {
    pair: PairData;
}

const PairCard: React.FC<PairCardProps> = ({ pair }) => {
    const formatDate = (dateString: string | null) => {
        if (!dateString) return 'Ongoing';
        const date = new Date(dateString);
        return date.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    return (
        <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
            <CardContent sx={{ flex: 1 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
                    <Typography variant="h6" component="div">
                        Pair ID: {pair.pairId}
                    </Typography>
                    <Chip 
                        label={pair.status} 
                        color={getPairingStatusColor(pair.status)} 
                        size="small" 
                    />
                </Box>
                
                <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mb: 2 }}>
                    <Box>
                        <Typography variant="body2" color="text.secondary">
                            FEMALE ID
                        </Typography>
                        <Typography variant="body1" fontWeight="medium">
                            {pair.femaleRabbitId}
                        </Typography>
                    </Box>
                    <Box>
                        <Typography variant="body2" color="text.secondary">
                            MALE ID
                        </Typography>
                        <Typography variant="body1" fontWeight="medium">
                            {pair.maleRabbitId}
                        </Typography>
                    </Box>
                </Box>
                
                <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2 }}>
                    <Box>
                        <Typography variant="body2" color="text.secondary">
                            START DATE
                        </Typography>
                        <Typography variant="body2" fontWeight="medium">
                            {formatDate(pair.startDate)}
                        </Typography>
                    </Box>
                    <Box>
                        <Typography variant="body2" color="text.secondary">
                            END DATE
                        </Typography>
                        <Typography variant="body2" fontWeight="medium" color={pair.endDate ? 'inherit' : 'primary.main'}>
                            {formatDate(pair.endDate)}
                        </Typography>
                    </Box>
                </Box>
            </CardContent>
        </Card>
    );
};

export default PairCard;
