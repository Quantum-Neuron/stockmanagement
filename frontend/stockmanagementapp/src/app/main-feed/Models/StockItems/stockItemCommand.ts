import { ColourEnum } from "../../Enums/ColourEnum";

export interface stockItemCommand {
    stockItemId: number;
    registrationNumber: string;
    make: string;
    model: string;
    modelYear: string;
    kms: number;
    Colour: ColourEnum;
    vin: string;
    price: number;
    costPrice: number;
    primaryImage: File | null;
    secondaryImages: File[];
}