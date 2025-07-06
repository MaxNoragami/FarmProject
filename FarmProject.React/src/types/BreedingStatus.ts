export const BreedingStatus = {
  Available: "Available",
  Paired: "Paired",
  Pregnant: "Pregnant",
  Nursing: "Nursing",
  Recovering: "Recovering",
  Inapt: "Inapt",
} as const;

export type BreedingStatus =
  (typeof BreedingStatus)[keyof typeof BreedingStatus];

export const breedingStatusOptions = Object.values(BreedingStatus);

export const breedingStatusStringToEnum: Record<string, number> = {
  Available: 0,
  Paired: 1,
  Pregnant: 2,
  Nursing: 3,
  Recovering: 4,
  Inapt: 5,
};

export const getBreedingStatusColor = (status: BreedingStatus) => {
  switch (status) {
    case BreedingStatus.Available:
      return "success";
    case BreedingStatus.Paired:
      return "info";
    case BreedingStatus.Pregnant:
      return "warning";
    case BreedingStatus.Nursing:
      return "secondary";
    case BreedingStatus.Recovering:
      return "default";
    case BreedingStatus.Inapt:
      return "error";
    default:
      return "default";
  }
};
