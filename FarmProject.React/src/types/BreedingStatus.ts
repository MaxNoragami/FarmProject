export const BreedingStatus = {
    Available: 'Available',
    Paired: 'Paired',
    Pregnant: 'Pregnant',
    Nursing: 'Nursing',
    Recovering: 'Recovering',
    Inapt: 'Inapt'
} as const;

export type BreedingStatus = typeof BreedingStatus[keyof typeof BreedingStatus];

export const breedingStatusOptions = Object.values(BreedingStatus);
