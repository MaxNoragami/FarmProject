import { OffspringType } from '../types/OffspringType';
import { type CageData as ApiCageData } from '../api/types/cageTypes';
import { type CageData as MockCageData } from '../data/mockCageData';

export const getCageLabel = (cage: ApiCageData | MockCageData): string => {
  if (!cage) return 'Empty';

  if (typeof cage.offspringType === 'number') {
    switch (cage.offspringType) {
      case 0:
        return OffspringType.None;
      case 1:
        return OffspringType.Mixed;
      case 2:
        return OffspringType.Female;
      case 3:
        return OffspringType.Male;
      default:
        return 'Empty';
    }
  }

  if (typeof cage.offspringType === 'string') {
    return cage.offspringType;
  }

  return 'Empty';
};

export const getCageChipColor = (cage: ApiCageData | MockCageData): any => {
  const label = getCageLabel(cage);
  if (label === OffspringType.None || label === 'Empty') {
    return 'success';
  }

  switch (label) {
    case OffspringType.Mixed:
      return 'warning';
    case OffspringType.Female:
      return 'secondary';
    case OffspringType.Male:
      return 'info';
    default:
      return 'default';
  }
};
