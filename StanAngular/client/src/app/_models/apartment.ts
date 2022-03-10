import { Adress } from "./adress";
import { Contract } from "./contract";

export interface Apartment{
    apartmentDescription: string;
    numberOfRooms: number;
    monthlyPrice: number;
    adress : Adress[]
    contracts: Contract[]
}