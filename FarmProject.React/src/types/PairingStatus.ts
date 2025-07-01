export const PairingStatus = {
    Active: 'Active',
    Successful: 'Successful', 
    Failed: 'Failed'
} as const;

export type PairingStatus = typeof PairingStatus[keyof typeof PairingStatus];

export const pairingStatusOptions = Object.values(PairingStatus);

// Mapping for use in filters/API
export const pairingStatusStringToEnum: Record<string, number> = {
    Active: 0,
    Successful: 1,
    Failed: 2,
};

export const pairingStatusEnumToString: Record<number, PairingStatus> = {
    0: PairingStatus.Active,
    1: PairingStatus.Successful,
    2: PairingStatus.Failed,
};

export const getPairingStatusColor = (status: PairingStatus) => {
    switch (status) {
        case PairingStatus.Active:
            return 'info';
        case PairingStatus.Successful:
            return 'success';
        case PairingStatus.Failed:
            return 'error';
        default:
            return 'default';
    }
};
