import { ILineIteme } from "@/types/Invoice";
import { ICustomer } from "@/types/Customer";

export interface ISalesOrder {
  id: number;
  createdOn: Date;
  updatedOn?: Date;
  customer: ICustomer;
  isPaid: boolean;
  salesOrderItems: ILineIteme;
}
