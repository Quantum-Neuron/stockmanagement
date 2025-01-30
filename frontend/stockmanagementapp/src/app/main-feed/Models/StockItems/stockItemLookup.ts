import { ColourEnum } from "../../Enums/ColourEnum";

export interface PagedStockItems<T> {
    items: T[];
    pageNumber: number;
    pageSize: number;
    totalItems: number;
}
 
export interface StockItemLookup {
    stockItemId: number;
    registrationNumber: string;
    make: string;
    model: string;
    modelYear: number;
    kms: number;
    Colour: ColourEnum,
    vinNumber: string,
    price: number,
    costPrice: number
}

export function isValidModelYear(year: number): boolean {
    return year >= 1900 && year <= 2100;
}