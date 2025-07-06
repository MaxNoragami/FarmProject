import { OffspringType } from '../types/OffspringType';
import { type CageData } from './cageMappers';

export const getCageLabel = (cage: CageData): string => {
  if (!cage) return 'Empty';

  if (cage.rabbitId) {
    return 'Occupied';
  } else if (cage.offspringCount > 0) {
    return `${cage.offspringCount} offspring`;
  } else {
    return 'Empty';
  }
};

export const getCageChipColor = (cage: CageData): 'default' | 'primary' | 'secondary' | 'error' | 'info' | 'success' | 'warning' => {
  if (!cage) return 'default';

  if (cage.rabbitId) {
    return 'primary';
  } else if (cage.offspringCount > 0) {
    return 'success';
  } else {
    return 'default';
  }
};
 