export interface ApiPairDto {
  id: number;
  femaleRabbitId: number;
  maleRabbitId: number;
  startDate: string;
  endDate: string | null;
  pairingStatus: number;
}

export interface PairListRequest {
  pageIndex: number;
  pageSize: number;
  logicalOperator?: number;
}
