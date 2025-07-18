import { apiClient } from "../config";
import { type PaginatedResponse } from "../types/common";
import { type ApiCageDto, type CageListRequest } from "../types/cageTypes";

export interface CageListFilters {
  name?: string;
  offspringType?: number;
  isOccupied?: boolean;
  isSacrificable?: boolean;
  logicalOperator?: number;
  sort?: string;
}

export class CageService {
  private static readonly BASE_PATH = "/cages";

  static async getCages(
    request: CageListRequest & CageListFilters
  ): Promise<PaginatedResponse<ApiCageDto>> {
    const params = new URLSearchParams({
      pageIndex: request.pageIndex.toString(),
      pageSize: request.pageSize.toString(),
      LogicalOperator: (request.logicalOperator ?? 0).toString(),
    });

    if (request.name) params.append("Name", request.name);
    if (typeof request.offspringType === "number")
      params.append("OffspringType", request.offspringType.toString());
    if (typeof request.isOccupied === "boolean")
      params.append("IsOccupied", request.isOccupied.toString());
    if (typeof request.isSacrificable === "boolean")
      params.append("IsSacrificable", request.isSacrificable.toString());
    if (request.sort) params.append("sort", request.sort);

    const response = await apiClient.get<PaginatedResponse<ApiCageDto>>(
      `${this.BASE_PATH}?${params.toString()}`
    );

    return response.data;
  }
}
