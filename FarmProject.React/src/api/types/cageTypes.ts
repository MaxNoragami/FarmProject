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

export interface CageData {
  id: number;
  name: string;
  rabbitId: number | null;
  offspringTypeName?: string;
  offspringType?: number;
  isOccupied?: boolean;
}
