import { apiClient } from "../config";
import { type PaginatedResponse } from "../types/common";
import {
  type ApiRabbitDto,
  type RabbitListRequest,
} from "../types/rabbitTypes";

export interface RabbitListFilters {
  name?: string;
  breedingStatus?: number;
  cageId?: number;
  logicalOperator?: number;
  sort?: string;
}

export class RabbitService {
  private static readonly BASE_PATH = "/breeding-rabbits";

  static async getRabbits(
    request: RabbitListRequest & RabbitListFilters
  ): Promise<PaginatedResponse<ApiRabbitDto>> {
    const params = new URLSearchParams({
      pageIndex: request.pageIndex.toString(),
      pageSize: request.pageSize.toString(),
      LogicalOperator: (request.logicalOperator ?? 0).toString(),
    });

    if (request.name) params.append("Name", request.name);
    if (typeof request.breedingStatus === "number")
      params.append("BreedingStatus", request.breedingStatus.toString());
    if (typeof request.cageId === "number")
      params.append("CageId", request.cageId.toString());
    if (request.sort) params.append("sort", request.sort);

    const response = await apiClient.get<PaginatedResponse<ApiRabbitDto>>(
      `${this.BASE_PATH}?${params.toString()}`
    );

    return response.data;
  }

  static async addRabbit(name: string, cageId: number): Promise<ApiRabbitDto> {
    const response = await apiClient.post<ApiRabbitDto>(this.BASE_PATH, {
      name,
      cageId,
    });

    return response.data;
  }

  public static async registerBirth(
    rabbitId: number,
    offspringCount: number
  ): Promise<void> {
    await apiClient.post(`/breeding-rabbits/${rabbitId}/birth`, {
      offspringCount,
    });
  }
}
