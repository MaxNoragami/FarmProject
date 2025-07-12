import { type ApiCageDto } from "../api/types/cageTypes";
import { OffspringType } from "../types/OffspringType";

export interface CageData {
  id: number;
  name: string;
  rabbitId: number | null;
  offspringCount: number;
  offspringType: OffspringType;
  birthDate: Date | null;
  isSacrificable: boolean;
}

const mapOffspringTypeFromApi = (apiType: number): OffspringType => {
  switch (apiType) {
    case 0:
      return OffspringType.None;
    case 1:
      return OffspringType.Mixed;
    case 2:
      return OffspringType.Male;
    case 3:
      return OffspringType.Female;
    default:
      return OffspringType.None;
  }
};

export const mapApiCageToUI = (apiCage: ApiCageDto): CageData => {
  return {
    id: apiCage.id,
    name: apiCage.name,
    rabbitId: apiCage.breedingRabbitId,
    offspringCount: apiCage.offspringCount,
    offspringType: mapOffspringTypeFromApi(apiCage.offspringType),
    birthDate: apiCage.birthDate ? new Date(apiCage.birthDate) : null,
    isSacrificable: apiCage.isSacrificable || false,
  };
};

export const mapApiCagesToUI = (apiCages: ApiCageDto[]): CageData[] => {
  return apiCages.map(mapApiCageToUI);
};

export const isCorsError = (error: any): boolean => {
  return (
    error?.code === "ERR_NETWORK" ||
    error?.message?.includes("CORS") ||
    error?.message?.includes("Network Error") ||
    error?.response?.status === 0
  );
};
