import { Card, CardContent, Box, Typography, IconButton } from "@mui/material";
import { WatchLater, CheckCircle } from "@mui/icons-material";
import { type OrderData } from "../../utils/orderMappers";
import { getOrderStatusColor } from "../../types/OrderStatus";

interface OrderTaskCardProps {
  order: OrderData;
  onClick?: () => void;
}

const OrderTaskCard: React.FC<OrderTaskCardProps> = ({ order, onClick }) => {
  const isCompleted = order.orderStatus === 1;
  const cardBg = isCompleted ? "#f5f5f5" : "#fff8e1";

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  return (
    <Card
      sx={{
        height: "100%",
        display: "flex",
        flexDirection: "column",
        backgroundColor: cardBg,
        opacity: isCompleted ? 0.7 : 1,
        transition: "background-color 0.2s",
        cursor: onClick ? "pointer" : "default",
        "&:hover": onClick
          ? {
              boxShadow: 2,
              transform: "translateY(-1px)",
            }
          : {},
      }}
      onClick={onClick}
    >
      <CardContent sx={{ flex: 1 }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            mb: 2,
          }}
        >
          <Typography variant="h6" component="div">
            Order #{order.id}
          </Typography>
          <IconButton
            size="small"
            sx={{
              p: 0.5,
              cursor: "default",
            }}
          >
            {isCompleted ? (
              <CheckCircle sx={{ color: "success.main" }} />
            ) : (
              <WatchLater sx={{ color: "#f57c00" }} />
            )}
          </IconButton>
        </Box>

        <Typography
          variant="body2"
          sx={{
            mb: 2,
            minHeight: "2.5em",
            display: "-webkit-box",
            WebkitLineClamp: 2,
            WebkitBoxOrient: "vertical",
            overflow: "hidden",
          }}
        >
          Order for Customer #{order.customerId}
        </Typography>

        <Box sx={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 2 }}>
          <Box>
            <Typography variant="body2" color="text.secondary">
              CUSTOMER ID
            </Typography>
            <Typography variant="body2" fontWeight="medium">
              {order.customerId}
            </Typography>
          </Box>
          <Box>
            <Typography variant="body2" color="text.secondary">
              ORDER DATE
            </Typography>
            <Typography variant="body2" fontWeight="medium">
              {formatDate(order.orderDate)}
            </Typography>
          </Box>
        </Box>
      </CardContent>
    </Card>
  );
};

export default OrderTaskCard;
