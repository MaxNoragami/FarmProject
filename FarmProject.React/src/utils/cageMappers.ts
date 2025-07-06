import { type ApiCageDto } from '../api/types/cageTypes';
import { OffspringType } from '../types/OffspringType';

export interface CageData {
  id: number;
  name: string;
  rabbitId: number | null;
  offspringCount: number;
  offspringType: OffspringType;
}

// Map API enum numbers to OffspringType strings
const mapOffspringTypeFromApi = (apiType: number): OffspringType => {
  switch (apiType) {
    case 0:
      return OffspringType.None;
    case 1:
      return OffspringType.Male;
    case 2:
      return OffspringType.Female;
    case 3:
      return OffspringType.Mixed;
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
  };
};

export const mapApiCagesToUI = (apiCages: ApiCageDto[]): CageData[] => {
  return apiCages.map(mapApiCageToUI);
};

// Helper to check if error is CORS-related
export const isCorsError = (error: any): boolean => {
  return (
    error?.code === 'ERR_NETWORK' ||
    error?.message?.includes('CORS') ||
    error?.message?.includes('Network Error') ||
    error?.response?.status === 0
  );
};
