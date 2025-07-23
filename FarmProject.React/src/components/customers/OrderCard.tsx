import {
  Card,
  CardContent,
  Box,
  Typography,
  Chip,
  useMediaQuery,
  useTheme,
} from "@mui/material";
import { type Order } from "../../types/Customer";
import {
  getOrderStatusLabel,
  getOrderStatusColor,
} from "../../types/OrderStatus";

interface OrderCardProps {
  order: Order;
}

const OrderCard: React.FC<OrderCardProps> = ({ order }) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("md"));

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    if (isMobile) {
      return date.toLocaleDateString("en-US", {
        year: "numeric",
        month: "short",
        day: "numeric",
      });
    } else {
      return date.toLocaleDateString("en-US", {
        year: "numeric",
        month: "short",
        day: "numeric",
        hour: "2-digit",
        minute: "2-digit",
      });
    }
  };

  return (
    <Card sx={{ height: "100%", display: "flex", flexDirection: "column" }}>
      <CardContent sx={{ py: 2 }}>
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            textAlign: "center",
          }}
        >
          <Typography variant="body1" fontWeight="medium" sx={{ mb: 0.5 }}>
            Order #{order.id}
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
            {formatDate(order.orderDate)}
          </Typography>
          <Chip
            label={getOrderStatusLabel(order.orderStatus)}
            color={getOrderStatusColor(order.orderStatus)}
            size="small"
          />
        </Box>
      </CardContent>
    </Card>
  );
};

export default OrderCard;
