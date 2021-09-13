export interface ICustomer {
  id: number;
  createOn: Date;
  updateOn?: Date;
  firstName: string;
  lastName: string;
  primaryAddress: ICustomerAddress;
}

export interface ICustomerAddress {
  id: number;
  createOn: Date;
  updateOn?: Date;
  addressLine1: string;
  addressLine2: string;
  addressLine3: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
}
