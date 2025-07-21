import { Card, CardContent, Box, Typography, Chip } from "@mui/material";
import { type OrderData } from "../../utils/orderMappers";
import {
  getOrderStatusLabel,
  getOrderStatusColor,
} from "../../types/OrderStatus";

interface OrderCardProps {
  order: OrderData;
}

const OrderCard: React.FC<OrderCardProps> = ({ order }) => {
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
            Order #{order.id}
          </Typography>
          <Chip
            label={getOrderStatusLabel(order.orderStatus)}
            color={getOrderStatusColor(order.orderStatus)}
            size="small"
          />
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
              CUSTOMER ID
            </Typography>
            <Typography variant="body1" fontWeight="medium">
              {order.customerId}
            </Typography>
          </Box>
          <Box>
            <Typography variant="body2" color="text.secondary">
              ORDER DATE
            </Typography>
            <Typography variant="body1" fontWeight="medium">
              {order.formattedDate}
            </Typography>
          </Box>
        </Box>
      </CardContent>
    </Card>
  );
};

export default OrderCard;
