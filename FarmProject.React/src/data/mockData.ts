import { BreedingStatus } from '../types/BreedingStatus';

export interface RabbitData {
  number: number;
  rabbitId: number;
  name: string;
  cageId: number;
  status: BreedingStatus;
}

export function createRabbitData(
  number: number,
  rabbitId: number,
  name: string,
  cageId: number,
  status: BreedingStatus,
): RabbitData {
  return { number, rabbitId, name, cageId, status };
}

export const mockRabbitsData: RabbitData[] = [
  createRabbitData(1, 332, 'Fluffy', 2, BreedingStatus.Available),
  createRabbitData(2, 123, 'Snowball', 3, BreedingStatus.Available),
  createRabbitData(3, 349, 'Thumper', 1, BreedingStatus.Available),
  createRabbitData(4, 993, 'Hopper', 4, BreedingStatus.Pregnant),
  createRabbitData(5, 201, 'Coco', 5, BreedingStatus.Available),
  createRabbitData(6, 88, 'Binky', 6, BreedingStatus.Available),
  createRabbitData(7, 77, 'Nibbles', 7, BreedingStatus.Available),
  createRabbitData(8, 45, 'Patches', 8, BreedingStatus.Available),
  createRabbitData(9, 66, 'Whiskers', 9, BreedingStatus.Available),
  createRabbitData(10, 101, 'Ginger', 10, BreedingStatus.Available),
  createRabbitData(11, 202, 'Pepper', 11, BreedingStatus.Available),
  createRabbitData(12, 303, 'Mittens', 12, BreedingStatus.Available),
  createRabbitData(13, 404, 'Shadow', 13, BreedingStatus.Available),
  createRabbitData(14, 505, 'Snowflake', 14, BreedingStatus.Available),
  createRabbitData(15, 606, 'Cinnamon', 15, BreedingStatus.Available),
  createRabbitData(16, 707, 'Mocha', 16, BreedingStatus.Available),
  createRabbitData(17, 808, 'Caramel', 17, BreedingStatus.Available),
  createRabbitData(18, 909, 'Hazel', 18, BreedingStatus.Available),
  createRabbitData(19, 111, 'Clover', 19, BreedingStatus.Available),
  createRabbitData(20, 222, 'Buttercup', 20, BreedingStatus.Available),
];
