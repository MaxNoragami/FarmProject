import {
  Card,
  CardContent,
  Box,
  Typography,
} from "@mui/material";
import { type CustomerData } from "../../utils/customerMappers";

interface CustomerCardProps {
  customer: CustomerData;
  onCustomerClick: (customer: CustomerData) => void;
  isSelected?: boolean;
}

const CustomerCard: React.FC<CustomerCardProps> = ({
  customer,
  onCustomerClick,
  isSelected = false,
}) => {
  return (
    <Card
      sx={{
        height: "100%",
        cursor: "pointer",
        border: isSelected ? 2 : 1,
        borderColor: isSelected ? "primary.main" : "divider",
        "&:hover": {
          borderColor: "primary.main",
          boxShadow: 2,
        },
      }}
      onClick={() => onCustomerClick(customer)}
    >
      <CardContent sx={{ p: 2 }}>
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            textAlign: "center",
          }}
        >
          <Typography variant="body1" fontWeight="medium" sx={{ mb: 0.5 }}>
            {customer.firstName} {customer.lastName}
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
            ID: {customer.id}
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
            {customer.email}
          </Typography>
          <Typography variant="body2" color="text.secondary">
            {customer.phoneNum}
          </Typography>
        </Box>
      </CardContent>
    </Card>
  );
};

export default CustomerCard;
