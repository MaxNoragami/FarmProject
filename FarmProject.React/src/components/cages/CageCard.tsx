import { Card, CardContent, Box, Typography, Chip } from "@mui/material";
import { Cake, NoMeals, Restaurant } from "@mui/icons-material";
import { type CageData } from "../../utils/cageMappers";
import { getOffspringTypeColor } from "../../types/OffspringType";

interface CageCardProps {
  cage: CageData;
  onCageClick?: (cage: CageData) => void;
  isCageClickable?: (cage: CageData) => boolean;
}

const CageCard: React.FC<CageCardProps> = ({
  cage,
  onCageClick,
  isCageClickable = () => false,
}) => {
  const formatBirthDate = (date: Date | null) => {
    if (!date) return "None";
    return new Date(date).toLocaleDateString();
  };

  const isClickable = isCageClickable(cage);

  const handleClick = () => {
    if (isClickable && onCageClick) {
      onCageClick(cage);
    }
  };

  return (
    <Card
      sx={{
        height: "100%",
        display: "flex",
        flexDirection: "column",
        cursor: isClickable ? "pointer" : "default",
        transition: "all 0.2s ease-in-out",
        "&:hover":
          isClickable && cage.isSacrificable
            ? {
                boxShadow: 2,
                transform: "translateY(-2px)",
              }
            : {},
      }}
      onClick={handleClick}
    >
      <CardContent sx={{ flex: 1 }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "flex-start",
            mb: 2,
          }}
        >
          <Typography variant="h6" component="div">
            {cage.name}
          </Typography>
          <Chip
            label={cage.offspringType}
            color={getOffspringTypeColor(cage.offspringType)}
            size="small"
          />
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
            <Typography variant="body2" color="text.secondary">
              CAGE ID
            </Typography>
            <Typography variant="body1" fontWeight="medium">
              {cage.id}
            </Typography>
          </Box>
          <Box>
            <Typography variant="body2" color="text.secondary">
              RABBIT ID
            </Typography>
            <Typography variant="body1" fontWeight="medium">
              {cage.rabbitId || "Empty"}
            </Typography>
          </Box>
        </Box>

        <Box
          sx={{
            display: "grid",
            gridTemplateColumns: "1fr 1fr",
            gap: 2,
          }}
        >
          <Box>
            <Typography variant="body2" color="text.secondary">
              AMOUNT
            </Typography>
            <Typography variant="h6" color="primary.main">
              {cage.offspringCount}
            </Typography>
          </Box>
          <Box>
            <Box sx={{ display: "flex", alignItems: "center", gap: 0.5 }}>
              <Cake fontSize="small" color="action" />
              <Typography variant="body2" color="text.secondary">
                {formatBirthDate(cage.birthDate)}
              </Typography>
            </Box>
            <Box sx={{ display: "flex", alignItems: "center", mt: 0.5 }}>
              {cage.isSacrificable ? (
                <Restaurant fontSize="small" color="success" />
              ) : (
                <NoMeals fontSize="small" color="error" />
              )}
            </Box>
          </Box>
        </Box>
      </CardContent>
    </Card>
  );
};

export default CageCard;
