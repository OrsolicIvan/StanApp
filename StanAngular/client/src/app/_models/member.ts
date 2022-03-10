import {Photo} from './photo';
import { Apartment } from "./apartment";
import { Contract } from "./contract";

export interface Member {
    id: number;
    userName: string;
    age: number;
    photos: Photo[];
    rentedApartments: Contract[]
    ownedApartments: Apartment[]
}
