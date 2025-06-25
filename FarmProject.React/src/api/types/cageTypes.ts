export interface ApiCageDto {
  id: number;
  name: string;
  breedingRabbitId: number | null;
  offspringCount: number;
  offspringType: number;
}

export interface CageListRequest {
  pageIndex: number;
  pageSize: number;
  logicalOperator?: number;
}
