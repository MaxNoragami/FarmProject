import { Card, CardContent, Box, Typography } from "@mui/material";
import { type CustomerData } from "../../utils/customerMappers";

interface CustomerCardProps {
  customer: CustomerData;
}

const CustomerCard: React.FC<CustomerCardProps> = ({ customer }) => {
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
          <Typography variant="h6" component="div">
            {customer.fullName}
          </Typography>
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
              ID
            </Typography>
            <Typography variant="body1" fontWeight="medium">
              {customer.id}
            </Typography>
          </Box>
          <Box>
            <Typography variant="body2" color="text.secondary">
              EMAIL
            </Typography>
            <Typography variant="body1" fontWeight="medium">
              {customer.email}
            </Typography>
          </Box>
          <Box>
            <Typography variant="body2" color="text.secondary">
              PHONE
            </Typography>
            <Typography variant="body1" fontWeight="medium">
              {customer.phoneNum}
            </Typography>
          </Box>
        </Box>
      </CardContent>
    </Card>
  );
};

export default CustomerCard;
