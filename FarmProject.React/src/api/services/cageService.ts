import { apiClient } from '../config';
import { type PaginatedResponse } from '../types/common';
import { type ApiCageDto, type CageListRequest } from '../types/cageTypes';

export class CageService {
  private static readonly BASE_PATH = '/cages';

  static async getCages(request: CageListRequest): Promise<PaginatedResponse<ApiCageDto>> {
    const params = new URLSearchParams({
      pageIndex: request.pageIndex.toString(),
      pageSize: request.pageSize.toString(),
      LogicalOperator: (request.logicalOperator || 0).toString(),
    });

    const response = await apiClient.get<PaginatedResponse<ApiCageDto>>(
      `${this.BASE_PATH}?${params.toString()}`
    );
    
    return response.data;
  }
}
