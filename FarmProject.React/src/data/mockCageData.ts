import { OffspringType } from '../types/OffspringType';

export interface CageData {
  id: number;
  name: string;
  rabbitId: number | null;
  offspringCount: number;
  offspringType: OffspringType;
}

export function createCageData(
  id: number,
  name: string,
  rabbitId: number | null,
  offspringCount: number,
  offspringType: OffspringType,
): CageData {
  return { id, name, rabbitId, offspringCount, offspringType };
}

export const mockCagesData: CageData[] = [
  createCageData(1, 'BestCage', 332, 3, OffspringType.Mixed),
  createCageData(2, 'DeluxeCage', 123, 0, OffspringType.None),
  createCageData(3, 'ComfortCage', 349, 5, OffspringType.Female),
  createCageData(4, 'StandardCage', 993, 4, OffspringType.Male),
  createCageData(5, 'PremiumCage', 201, 2, OffspringType.Mixed),
  createCageData(6, 'BasicCage', null, 0, OffspringType.None),
  createCageData(7, 'LuxuryCage', 77, 6, OffspringType.Female),
  createCageData(8, 'EconomyCage', 45, 3, OffspringType.Male),
  createCageData(9, 'SuperCage', 66, 0, OffspringType.None),
  createCageData(10, 'MegaCage', 101, 4, OffspringType.Mixed),
  createCageData(11, 'UltraCage', 202, 2, OffspringType.Female),
  createCageData(12, 'MaxiCage', 303, 0, OffspringType.None),
  createCageData(13, 'YoloCage', 304, 0, OffspringType.Mixed),
  createCageData(14, 'EmptyCage', null, 0, OffspringType.None),
  createCageData(15, 'VacantCage', null, 0, OffspringType.None),
  createCageData(16, 'OccupiedCage', 400, 1, OffspringType.Male),
  createCageData(17, 'FreeCage', null, 0, OffspringType.None),
  createCageData(18, 'OpenCage', null, 0, OffspringType.None),
  createCageData(19, 'AvailableCage', null, 0, OffspringType.None),
  createCageData(20, 'SpareCage', null, 0, OffspringType.None),
  createCageData(21, 'ReserveCage', null, 0, OffspringType.None),
  createCageData(22, 'BackupCage', null, 0, OffspringType.None),
  createCageData(23, 'NewCage', null, 0, OffspringType.None),
  createCageData(24, 'CleanCage', null, 0, OffspringType.None),
  createCageData(25, 'FreshCage', null, 0, OffspringType.None),
  createCageData(26, 'ReadyCage', null, 0, OffspringType.None),
  createCageData(27, 'IdleCage', null, 0, OffspringType.None),
  createCageData(28, 'WaitingCage', null, 0, OffspringType.None),
  createCageData(29, 'StandbyCage', null, 0, OffspringType.None),
  createCageData(30, 'UnusedCage', null, 0, OffspringType.None),
];