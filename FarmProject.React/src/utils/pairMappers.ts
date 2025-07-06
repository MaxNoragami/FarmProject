import { type ApiPairDto } from "../api/types/pairTypes";
import {
  pairingStatusEnumToString,
  PairingStatus,
} from "../types/PairingStatus";

export interface PairData {
  id: number;
  pairId: string;
  femaleRabbitId: number;
  maleRabbitId: number;
  startDate: string;
  endDate: string | null;
  status: PairingStatus;
}

export const mapApiPairToUI = (apiPair: ApiPairDto): PairData => {
  return {
    id: apiPair.id,
    pairId: apiPair.id.toString(),
    femaleRabbitId: apiPair.femaleRabbitId,
    maleRabbitId: apiPair.maleRabbitId,
    startDate: apiPair.startDate,
    endDate: apiPair.endDate,
    status:
      pairingStatusEnumToString[apiPair.pairingStatus] || PairingStatus.Active,
  };
};

export const mapApiPairsToUI = (apiPairs: ApiPairDto[]): PairData[] => {
  return apiPairs.map(mapApiPairToUI);
};
