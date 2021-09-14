import { IProduct } from "@/types/Product";

export interface IInvoice {
  customerId: number;
  lineItems:ILineItem[];
  createOn: Date;
  updateOn: Date;
}
export interface ILineIteme{
  product?:IProduct;
  quantity: number
}