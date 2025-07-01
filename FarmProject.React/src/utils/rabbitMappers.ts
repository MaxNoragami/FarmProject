import { type ApiRabbitDto } from '../api/types/rabbitTypes';
import { type RabbitData } from '../data/mockData';
import { BreedingStatus } from '../types/BreedingStatus';

// Map breeding status number to string
const breedingStatusEnumToString: Record<number, BreedingStatus> = {
  0: BreedingStatus.Available,
  1: BreedingStatus.Paired,
  2: BreedingStatus.Pregnant,
  3: BreedingStatus.Nursing,
  4: BreedingStatus.Recovering,
  5: BreedingStatus.Inapt,
};

export const mapApiRabbitToUI = (apiRabbit: ApiRabbitDto): RabbitData => {
  return {
    number: apiRabbit.id,
    rabbitId: apiRabbit.id,
    name: apiRabbit.name,
    cageId: apiRabbit.cageId,
    status: breedingStatusEnumToString[apiRabbit.breedingStatus] || BreedingStatus.Available,
  };
};

export const mapApiRabbitsToUI = (apiRabbits: ApiRabbitDto[]): RabbitData[] => {
  return apiRabbits.map(mapApiRabbitToUI);
};
