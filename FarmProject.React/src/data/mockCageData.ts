import { OffspringType } from '../types/OffspringType';

export interface CageData {
  id: number;
  name: string;
  cageId: number;
  rabbitId: number | null;
  offspringCount: number;
  offspringType: OffspringType;
}

export function createCageData(
  id: number,
  name: string,
  cageId: number,
  rabbitId: number | null,
  offspringCount: number,
  offspringType: OffspringType,
): CageData {
  return { id, name, cageId, rabbitId, offspringCount, offspringType };
}

export const mockCagesData: CageData[] = [
  createCageData(1, 'BestCage', 101, 332, 3, OffspringType.Mixed),
  createCageData(2, 'DeluxeCage', 102, 123, 0, OffspringType.None),
  createCageData(3, 'ComfortCage', 103, 349, 5, OffspringType.Female),
  createCageData(4, 'StandardCage', 104, 993, 4, OffspringType.Male),
  createCageData(5, 'PremiumCage', 105, 201, 2, OffspringType.Mixed),
  createCageData(6, 'BasicCage', 106, null, 0, OffspringType.None),
  createCageData(7, 'LuxuryCage', 107, 77, 6, OffspringType.Female),
  createCageData(8, 'EconomyCage', 108, 45, 3, OffspringType.Male),
  createCageData(9, 'SuperCage', 109, 66, 0, OffspringType.None),
  createCageData(10, 'MegaCage', 110, 101, 4, OffspringType.Mixed),
  createCageData(11, 'UltraCage', 111, 202, 2, OffspringType.Female),
  createCageData(12, 'MaxiCage', 112, 303, 0, OffspringType.None),
  createCageData(13, 'YoloCage', 113, 304, 0, OffspringType.Mixed),
  createCageData(14, 'EmptyCage', 114, null, 0, OffspringType.None),
  createCageData(15, 'VacantCage', 115, null, 0, OffspringType.None),
  createCageData(16, 'OccupiedCage', 116, 400, 1, OffspringType.Male),
];
