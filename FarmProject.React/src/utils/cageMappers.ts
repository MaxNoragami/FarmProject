import { type ApiCageDto } from '../api/types/cageTypes';
import { type CageData, mockCagesData } from '../data/mockCageData';
import { OffspringType } from '../types/OffspringType';

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

// Fallback function to get mock data with pagination simulation
export const getMockCagesWithPagination = (pageIndex: number, pageSize: number) => {
  const startIndex = pageIndex * pageSize;
  const endIndex = startIndex + pageSize;
  const paginatedItems = mockCagesData.slice(startIndex, endIndex);

  return {
    items: paginatedItems,
    pageIndex: pageIndex + 1, // API uses 1-based indexing
    pageSize,
    totalCount: mockCagesData.length,
    totalPages: Math.ceil(mockCagesData.length / pageSize),
    hasNextPage: endIndex < mockCagesData.length,
    hasPreviousPage: pageIndex > 0,
  };
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
