export interface IProduct {
  id: number;
  createOn: Date;
  updateOn: Date;
  name: string;
  description: string;
  price: number;
  isTaxable: boolean;
  isArchived: boolean;
}

export interface IProductInventory {
  id: number;
  product: IProdcut;
  quantityOnHand: number;
  idealQuantity: number;
}