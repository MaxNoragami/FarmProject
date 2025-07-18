export const OffspringType = {
  None: "None",
  Mixed: "Mixed",
  Male: "Male",
  Female: "Female",
} as const;

export type OffspringType = (typeof OffspringType)[keyof typeof OffspringType];

export const offspringTypeOptions = Object.values(OffspringType);

export const getOffspringTypeColor = (type: OffspringType) => {
  switch (type) {
    case OffspringType.None:
      return "default";
    case OffspringType.Mixed:
      return "warning";
    case OffspringType.Male:
      return "info";
    case OffspringType.Female:
      return "secondary";
    default:
      return "default";
  }
};

// Mapping for use in filters/API
export const offspringTypeStringToEnum: Record<string, number> = {
  None: 0,
  Mixed: 1,
  Male: 2,
  Female: 3,
};
