export interface IInventoryTimeline {
  productInventorySnapshots: ISnapshot[];
  timelines: Date[];
}
export interface ISnapshot {
  productId: number;
  quantityOnHand: number[];
}
