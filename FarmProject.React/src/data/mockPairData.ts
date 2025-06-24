import { PairingStatus } from '../types/PairingStatus';

export interface PairData {
  id: number;
  pairId: string;
  femaleRabbitId: number;
  maleRabbitId: number;
  status: PairingStatus;
  startDate: string;
  endDate: string | null;
}

export function createPairData(
  id: number,
  pairId: string,
  femaleRabbitId: number,
  maleRabbitId: number,
  status: PairingStatus,
  startDate: string,
  endDate: string | null,
): PairData {
  return { id, pairId, femaleRabbitId, maleRabbitId, status, startDate, endDate };
}

export const mockPairsData: PairData[] = [
  createPairData(1, '1', 332, 123, PairingStatus.Active, '2025-01-15T10:30:00', null),
  createPairData(2, '2', 349, 77, PairingStatus.Successful, '2024-12-20T14:15:00', '2025-01-10T16:45:00'),
  createPairData(3, '3', 201, 45, PairingStatus.Active, '2025-01-20T09:00:00', null),
  createPairData(4, '4', 88, 66, PairingStatus.Failed, '2024-12-15T11:30:00', '2024-12-25T13:20:00'),
  createPairData(5, '5', 202, 101, PairingStatus.Successful, '2024-11-30T15:45:00', '2024-12-28T10:15:00'),
  createPairData(6, '6', 303, 404, PairingStatus.Active, '2025-01-18T08:15:00', null),
  createPairData(7, '7', 505, 606, PairingStatus.Failed, '2024-12-10T12:00:00', '2024-12-18T14:30:00'),
  createPairData(8, '8', 707, 808, PairingStatus.Successful, '2024-11-25T16:20:00', '2024-12-30T11:45:00'),
  createPairData(9, '9', 909, 111, PairingStatus.Active, '2025-01-22T13:10:00', null),
  createPairData(10, '10', 222, 993, PairingStatus.Failed, '2024-12-05T10:45:00', '2024-12-12T15:30:00'),
  createPairData(11, '11', 400, 304, PairingStatus.Successful, '2024-12-01T09:30:00', '2025-01-05T12:00:00'),
  createPairData(12, '12', 77, 332, PairingStatus.Active, '2025-01-25T14:45:00', null),
    createPairData(13, '13', 777, 172, PairingStatus.Active, '2025-01-25T14:45:00', null),
];
