import { apiClient } from '../config';
import { type PaginatedResponse } from '../types/common';
import { type ApiPairDto, type PairListRequest } from '../types/pairTypes';

export interface PairListFilters {
  pairingStatus?: number;
  femaleRabbitId?: number;
  maleRabbitId?: number;
  startDate?: string;
  endDate?: string;
  logicalOperator?: number;
  sort?: string;
}

export class PairService {
  private static readonly BASE_PATH = '/pairs';

  static async getPairs(request: PairListRequest & PairListFilters): Promise<PaginatedResponse<ApiPairDto>> {
    const params = new URLSearchParams({
      pageIndex: request.pageIndex.toString(),
      pageSize: request.pageSize.toString(),
      LogicalOperator: (request.logicalOperator ?? 0).toString(),
    });

    if (typeof request.pairingStatus === 'number') params.append('PairingStatus', request.pairingStatus.toString());
    if (typeof request.femaleRabbitId === 'number') params.append('FemaleRabbitId', request.femaleRabbitId.toString());
    if (typeof request.maleRabbitId === 'number') params.append('MaleRabbitId', request.maleRabbitId.toString());
    if (request.startDate) params.append('StartDate', request.startDate);
    if (request.endDate) params.append('EndDate', request.endDate);
    if (request.sort) params.append('sort', request.sort);

    const response = await apiClient.get<PaginatedResponse<ApiPairDto>>(
      `${this.BASE_PATH}?${params.toString()}`
    );
    
    return response.data;
  }

  static async addPair(femaleRabbitId: number, maleRabbitId: number): Promise<ApiPairDto> {
    const response = await apiClient.post<ApiPairDto>(this.BASE_PATH, {
      femaleRabbitId,
      maleRabbitId
    });
    
    return response.data;
  }

  static async updatePairStatus(pairId: number, pairingStatus: number): Promise<ApiPairDto> {
    const response = await apiClient.put<ApiPairDto>(`${this.BASE_PATH}/${pairId}`, {
      pairingStatus
    });
    
    return response.data;
  }
}
