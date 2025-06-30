import { apiClient } from '../config';
import { type PaginatedResponse } from '../types/common';
import { type ApiCageDto, type CageListRequest } from '../types/cageTypes';

// Add filter params to CageListRequest
export interface CageListFilters {
  name?: string;
  offspringType?: number;
  isOccupied?: boolean;
  logicalOperator?: number;
}

export class CageService {
  private static readonly BASE_PATH = '/cages';

  static async getCages(request: CageListRequest & CageListFilters): Promise<PaginatedResponse<ApiCageDto>> {
    const params = new URLSearchParams({
      pageIndex: request.pageIndex.toString(),
      pageSize: request.pageSize.toString(),
      LogicalOperator: (request.logicalOperator ?? 0).toString(),
    });

    if (request.name) params.append('Name', request.name);
    if (typeof request.offspringType === 'number') params.append('OffspringType', request.offspringType.toString());
    if (typeof request.isOccupied === 'boolean') params.append('IsOccupied', request.isOccupied.toString());

    const response = await apiClient.get<PaginatedResponse<ApiCageDto>>(
      `${this.BASE_PATH}?${params.toString()}`
    );
    
    return response.data;
  }
}
