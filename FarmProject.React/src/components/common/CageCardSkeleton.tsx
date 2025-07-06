import { Card, CardContent, Box, Skeleton } from "@mui/material";

const CageCardSkeleton: React.FC = () => {
  return (
    <Card sx={{ height: "100%", display: "flex", flexDirection: "column" }}>
      <CardContent sx={{ flex: 1 }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "flex-start",
            mb: 2,
          }}
        >
          <Skeleton variant="text" width="60%" height={28} />
          <Skeleton variant="rounded" width={60} height={24} />
        </Box>

        <Box
          sx={{
            display: "grid",
            gridTemplateColumns: "1fr 1fr",
            gap: 2,
            mb: 2,
          }}
        >
          <Box>
            <Skeleton variant="text" width="50%" height={16} />
            <Skeleton variant="text" width="70%" height={20} />
          </Box>
          <Box>
            <Skeleton variant="text" width="60%" height={16} />
            <Skeleton variant="text" width="40%" height={20} />
          </Box>
        </Box>

        <Box>
          <Skeleton variant="text" width="70%" height={16} />
          <Skeleton variant="text" width="30%" height={32} />
        </Box>
      </CardContent>
    </Card>
  );
};

export default CageCardSkeleton;
