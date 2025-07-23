import { type CageData } from "./cageMappers";

export const getCageLabel = (cage: CageData): string => {
  const availableCount = cage.offspringCount - cage.reservedOffspringCount;
  if (cage.offspringCount > 0) {
    return `${availableCount}/${cage.offspringCount} offspring`;
  }
  return cage.rabbitId ? "Occupied" : "Empty";
};

export const getCageChipColor = (
  cage: CageData
):
  | "default"
  | "primary"
  | "secondary"
  | "error"
  | "info"
  | "success"
  | "warning" => {
  const availableCount = cage.offspringCount - cage.reservedOffspringCount;
  if (cage.isSacrificable && availableCount > 0) {
    return "success";
  }
  if (cage.offspringCount > 0) {
    return availableCount > 0 ? "warning" : "error";
  }
  if (cage.rabbitId) {
    return "info";
  }
  return "default";
};
