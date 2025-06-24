import { Card, CardContent, Box, Typography, Chip } from '@mui/material';
import { type CageData } from '../../data/mockCageData';
import { getOffspringTypeColor } from '../../types/OffspringType';

interface CageCardProps {
    cage: CageData;
}

const CageCard: React.FC<CageCardProps> = ({ cage }) => {
    return (
        <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
            <CardContent sx={{ flex: 1 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
                    <Typography variant="h6" component="div">
                        {cage.name}
                    </Typography>
                    <Chip 
                        label={cage.offspringType} 
                        color={getOffspringTypeColor(cage.offspringType)} 
                        size="small" 
                    />
                </Box>
                
                <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mb: 2 }}>
                    <Box>
                        <Typography variant="body2" color="text.secondary">
                            CAGE ID
                        </Typography>
                        <Typography variant="body1" fontWeight="medium">
                            {cage.cageId}
                        </Typography>
                    </Box>
                    <Box>
                        <Typography variant="body2" color="text.secondary">
                            RABBIT ID
                        </Typography>
                        <Typography variant="body1" fontWeight="medium">
                            {cage.rabbitId || 'Empty'}
                        </Typography>
                    </Box>
                </Box>
                
                <Box>
                    <Typography variant="body2" color="text.secondary">
                        OFFSPRING AMOUNT
                    </Typography>
                    <Typography variant="h6" color="primary.main">
                        {cage.offspringCount}
                    </Typography>
                </Box>
            </CardContent>
        </Card>
    );
};

export default CageCard;
