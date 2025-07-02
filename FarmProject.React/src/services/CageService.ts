import { apiClient } from '../api/config';
import { type PaginatedResponse } from '../api/types/common';

interface CageListRequest {
  pageIndex: number;
  pageSize: number;
  filters?: {
    name?: string;
    offspringType?: number;
    isOccupied?: boolean;
  };
  logicalOperator?: number;
  sort?: string;
}

export class CageService {
  private static readonly BASE_PATH = '/cages';

  static async addCage(name: string): Promise<any> {
    const response = await apiClient.post(this.BASE_PATH, { name });
    return response.data;
  }

  static async getCages(request: CageListRequest): Promise<PaginatedResponse<any>> {
    const params = new URLSearchParams({
      pageIndex: request.pageIndex.toString(),
      pageSize: request.pageSize.toString(),
      LogicalOperator: (request.logicalOperator ?? 0).toString(),
    });

    if (request.filters) {
      if (request.filters.name) params.append('Name', request.filters.name);
      if (typeof request.filters.offspringType === 'number') {
        params.append('OffspringType', request.filters.offspringType.toString());
      }
      if (typeof request.filters.isOccupied === 'boolean') {
        params.append('IsOccupied', request.filters.isOccupied.toString());
      }
    }
    
    if (request.sort) params.append('sort', request.sort);

    const response = await apiClient.get<PaginatedResponse<any>>(
      `${this.BASE_PATH}?${params.toString()}`
    );
    
    return response.data;
  }
}
