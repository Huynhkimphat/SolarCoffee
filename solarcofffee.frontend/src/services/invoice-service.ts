import axios from "axios";
import { IInvoice } from "@/types/Invoice";
import { IServiceResponse } from "@/types/ServiceResponse";
import { IInventoryTimeline } from "@/types/InventoryGraph";

/*
 * Invoice Service
 * Provides UI business logic associated with Invoices
 */

export default class InvoiceService {
  API_URL = process.env.VUE_APP_API_URL;

  public async makeNewInvoice(
    invoice: IInvoice
  ): Promise<IServiceResponse<boolean>> {
    const now = new Date();
    invoice.createOn = now;
    invoice.updateOn = now;
    const result = await axios.post(`${this.API_URL}/order`,invoice);
    return result.data;
  }

  async getSnapshotHistory() :Promise<IInventoryTimeline>{
    const result:any=await axios.get(`${this.API_URL}/inventory/snapshot`);
    return result.data;
  }
}
