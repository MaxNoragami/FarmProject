export const FarmTaskType = {
  NestPreparation: "NestPreparation",
  NestRemoval: "NestRemoval",
  Weaning: "Weaning",
  OffspringSeparation: "OffspringSeparation",
  BreedingStatusUpdate: "BreedingStatusUpdate",
} as const;

export type FarmTaskType = (typeof FarmTaskType)[keyof typeof FarmTaskType];

export const farmTaskTypeOptions = Object.values(FarmTaskType);

export const getFarmTaskTypeColor = (type: FarmTaskType) => {
  switch (type) {
    case FarmTaskType.NestPreparation:
      return "primary";
    case FarmTaskType.NestRemoval:
      return "warning";
    case FarmTaskType.Weaning:
      return "success";
    case FarmTaskType.OffspringSeparation:
      return "secondary";
    case FarmTaskType.BreedingStatusUpdate:
      return "info";
    default:
      return "default";
  }
};

export const getFarmTaskTypeLabel = (type: FarmTaskType) => {
  switch (type) {
    case FarmTaskType.NestPreparation:
      return "Nest Preparation";
    case FarmTaskType.NestRemoval:
      return "Nest Removal";
    case FarmTaskType.Weaning:
      return "Weaning";
    case FarmTaskType.OffspringSeparation:
      return "Offspring Separation";
    case FarmTaskType.BreedingStatusUpdate:
      return "Breeding Status Update";
    default:
      return type;
  }
};

export const farmTaskTypeStringToEnum: Record<string, number> = {
  NestPreparation: 0,
  NestRemoval: 1,
  Weaning: 2,
  OffspringSeparation: 3,
  BreedingStatusUpdate: 4,
};
