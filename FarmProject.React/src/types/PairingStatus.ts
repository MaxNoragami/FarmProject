export const PairingStatus = {
    Active: 'Active',
    Successful: 'Successful',
    Failed: 'Failed'
} as const;

export type PairingStatus = typeof PairingStatus[keyof typeof PairingStatus];

export const pairingStatusOptions = Object.values(PairingStatus);

export const getPairingStatusColor = (status: PairingStatus) => {
    switch (status) {
        case PairingStatus.Active:
            return 'primary'; // Blue
        case PairingStatus.Successful:
            return 'success'; // Green
        case PairingStatus.Failed:
            return 'error'; // Red
        default:
            return 'default';
    }
};
