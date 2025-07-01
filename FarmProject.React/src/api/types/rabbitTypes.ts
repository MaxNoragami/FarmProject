export interface ApiRabbitDto {
  id: number;
  name: string;
  cageId: number;
  breedingStatus: number;
}

export interface RabbitListRequest {
  pageIndex: number;
  pageSize: number;
}
