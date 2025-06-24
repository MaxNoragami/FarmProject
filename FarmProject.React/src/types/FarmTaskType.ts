export const FarmTaskType = {
    NestPreparation: 'NestPreparation',
} as const;

export type FarmTaskType = typeof FarmTaskType[keyof typeof FarmTaskType];

export const farmTaskTypeOptions = Object.values(FarmTaskType);

export const getFarmTaskTypeColor = (type: FarmTaskType) => {
    switch (type) {
        case FarmTaskType.NestPreparation:
            return 'primary';
        default:
            return 'default';
    }
};

export const getFarmTaskTypeLabel = (type: FarmTaskType) => {
    switch (type) {
        case FarmTaskType.NestPreparation:
            return 'Nest Preparation';
        default:
            return type;
    }
};
