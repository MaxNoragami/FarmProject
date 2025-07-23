import { Card, CardContent, Box, Typography, Chip } from "@mui/material";
import { type OrderData } from "../../utils/orderMappers";
import {
  getOrderStatusLabel,
  getOrderStatusColor,
} from "../../types/OrderStatus";

interface OrderCardProps {
  order: OrderData;
  onClick?: () => void;
}

const OrderCard: React.FC<OrderCardProps> = ({ order, onClick }) => {
  return (
    <Card
      sx={{
        height: "100%",
        display: "flex",
        flexDirection: "column",
        cursor: onClick ? "pointer" : "default",
        transition: "box-shadow 0.2s, transform 0.2s",
        ...(onClick && {
          "&:hover": {
            boxShadow: 4,
            transform: "translateY(-2px) scale(1.02)",
          },
          "&:active": {
            boxShadow: 2,
            transform: "translateY(0px) scale(0.99)",
          },
        }),
        position: "relative",
      }}
      onClick={onClick}
      onTouchStart={onClick}
    >
      {onClick && (
        <button
          type="button"
          aria-label={`View details for Order #${order.id}`}
          onClick={onClick}
          style={{
            position: "absolute",
            inset: 0,
            width: "100%",
            height: "100%",
            opacity: 0,
            zIndex: 1,
            cursor: "pointer",
            border: "none",
            background: "none",
            padding: 0,
          }}
        />
      )}
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
