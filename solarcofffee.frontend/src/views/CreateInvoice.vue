<template>
  <div>
    <h1 id="invoiceTitle">Create Invoice</h1>
    <hr />
    <div v-if="invoiceStep === 1" class="invoice-step">
      <h2>Step 1: Select Customer</h2>
      <div v-if="customers.length" class="invoice-step-detail">
        <label for="customer">Customer: </label>
        <select
          id="customer"
          v-model="selectedCustomerId"
          class="invoice-customers"
        >
          <option disabled value="">Please select a customer</option>
          <option v-for="c in customers" :key="c.id" :value="c.id">
            {{ c.firstName + " " + c.lastName }}
          </option>
        </select>
      </div>
    </div>
    <div v-if="invoiceStep === 2" class="invoice-step">
      <h2>Step 2: Create Order</h2>

      <div v-if="inventory.length" class="invoice-step-detail">
        <label for="product">Product: </label>
        <select id="product" v-model="newItem.product" class="invoiceLineItem">
          <option disabled value="">Please select a product</option>
          <option v-for="i in inventory" :key="i.product.id" :value="i.product">
            {{ i.product.name }}
          </option>
        </select>
        <label for="quantity">Quantity: </label>
        <input id="quantity" v-model="newItem.quantity" min="0" type="number" />
      </div>

      <div class="invoice-item-actions">
        <solar-button
          :disabled="!newItem.product || !newItem.quantity"
          @button:click="addLineItem"
        >
          Add Line Item
        </solar-button>

        <solar-button
          :disabled="!lineItems.length"
          @button:click="finalizeOrder"
        >
          Finalize Order
        </solar-button>
      </div>

      <div v-if="lineItems.length" class="invoice-order-list">
        <div class="runningTotal">
          <h3>Running Total:</h3>
          {{ runningTotal | price }}
        </div>
        <hr />
        <table class="table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Description</th>
              <th>Qty.</th>
              <th>Price</th>
              <th>Subtotal</th>
            </tr>
          </thead>
          <tr
            v-for="lineItem in lineItems"
            :key="`index_${lineItem.product.id}`"
          >
            <td>{{ lineItem.product.name }}</td>
            <td>{{ lineItem.product.description }}</td>
            <td>{{ lineItem.quantity }}</td>
            <td>{{ lineItem.product.price }}</td>
            <td>
              {{ (lineItem.product.price * lineItem.quantity) | price }}
            </td>
          </tr>
        </table>
      </div>
    </div>
    <div v-if="invoiceStep === 3" class="invoice-step">
      <h2>Step 3: Review and Submit</h2>
      <solar-button @button:click="submitInvoice">Submit Invoice</solar-button>
      <hr />
      <div id="invoice" ref="invoice" class="invoice-step-detail">
        <div class="invoice-logo">
          <img
            id="imgLogo"
            alt="SolarCoffeeLogo"
            src="../assets/images/SolarCoffeeLogo.png"
          />
          <h3>1337 Solar Lane</h3>
          <h3>San Somewhere, CA 90000</h3>
          <h3>USA</h3>
          <div v-if="lineItems.length" class="invoice-order-list">
            <div class="invoice-header">
              <h3>Invoice: {{ new Date() | humanizeDate }}</h3>
              <h3>
                Customer:
                {{
                  this.selectedCustomer.firstName +
                  " " +
                  this.selectedCustomer.lastName
                }}
              </h3>
              <h3>
                Address: {{ this.selectedCustomer.primaryAddress.addressLine1 }}
              </h3>
              <h3 v-if="this.selectedCustomer.primaryAddress.addressLine2">
                Address: {{ this.selectedCustomer.primaryAddress.addressLine2 }}
              </h3>
              <h3 v-if="this.selectedCustomer.primaryAddress.addressLine3">
                Address: {{ this.selectedCustomer.primaryAddress.addressLine3 }}
              </h3>
              <h3>
                {{ this.selectedCustomer.primaryAddress.city }}
                {{ this.selectedCustomer.primaryAddress.state }}
                {{ this.selectedCustomer.primaryAddress.postalCode }}
              </h3>
              <h3>
                {{ this.selectedCustomer.primaryAddress.country }}
              </h3>
            </div>
            <table class="table">
              <thead>
                <tr>
                  <th>Product</th>
                  <th>Description</th>
                  <th>Qty.</th>
                  <th>Price</th>
                  <th>Subtotal</th>
                </tr>
              </thead>
              <tr
                v-for="lineItem in lineItems"
                :key="`index_${lineItem.product.id}`"
              >
                <td>{{ lineItem.product.name }}</td>
                <td>{{ lineItem.product.description }}</td>
                <td>{{ lineItem.quantity }}</td>
                <td>{{ lineItem.product.price }}</td>
                <td>
                  {{ (lineItem.product.price * lineItem.quantity) | price }}
                </td>
              </tr>
              <tr>
                <th colspan="4"></th>
                <th>Grand Total</th>
              </tr>
              <tfoot>
                <tr>
                  <td class="due" colspan="4">Balance due upon receipt:</td>
                  <td class="price-final">{{ runningTotal | price }}</td>
                </tr>
              </tfoot>
            </table>
          </div>
        </div>
      </div>
    </div>
    <hr />
    <div class="invoice-step-action">
      <solar-button :disabled="!canGoPrev" @button:click="prev"
        >Previous
      </solar-button>
      <solar-button :disabled="!canGoNext" @button:click="next"
        >Next
      </solar-button>
      <solar-button @button:click="startOver">Start Over</solar-button>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IInvoice, ILineIteme } from "@/types/Invoice";
import { ICustomer } from "@/types/Customer";
import { IProductInventory } from "@/types/Product";
import CustomerService from "@/services/customer-service";
import { InventoryService } from "@/services/inventory-service";
import InvoiceService from "@/services/invoice-service";
import SolarButton from "@/components/SolarButton.vue";
import jsPDF from "jspdf";
import html2canvas from "html2canvas";

const customerService = new CustomerService();
const inventoryService = new InventoryService();
const invoiceService = new InvoiceService();

@Component({
  name: "CreateInvoice",
  components: { SolarButton },
})
export default class CreateInvoice extends Vue {
  invoiceStep = 1;
  invoice: IInvoice = {
    customerId: 0,
    lineItems: [],
    createOn: new Date(),
    updateOn: new Date(),
  };
  customers: ICustomer[] = [];
  selectedCustomerId = 0;
  inventory: IProductInventory[] = [];
  lineItems: ILineIteme[] = [];
  newItem: ILineIteme = { product: undefined, quantity: 0 };

  get canGoPrev() {
    return this.invoiceStep !== 1;
  }

  get canGoNext() {
    if (this.invoiceStep === 1) {
      return this.selectedCustomerId !== 0;
    }
    if (this.invoiceStep === 2) {
      return this.lineItems.length;
    }
    if (this.invoiceStep === 3) {
      return false;
    }
    return false;
  }

  get runningTotal() {
    return this.lineItems.reduce(
      (a, b) => a + b["product"]["price"] * b["quantity"],
      0
    );
  }

  get selectedCustomer() {
    return this.customers.find((c) => c.id == this.selectedCustomerId);
  }

  startOver(): void {
    this.invoice = {
      customerId: 0,
      lineItems: [],
      createOn: new Date(),
      updateOn: new Date(),
    };
    this.selectedCustomerId = 0;
    this.lineItems = [];
    this.invoiceStep = 1;
  }

  addLineItem() {
    let newItem: ILineIteme = {
      product: this.newItem.product,
      quantity: Number(this.newItem.quantity),
    };
    let existingItems = this.lineItems.map((item) => item.product.id);

    if (existingItems.includes(newItem.product.id)) {
      let lineItem = this.lineItems.find(
        (item) => item.product.id === newItem.product.id
      );
      lineItem.quantity = Number(lineItem.quantity) + newItem.quantity;
    } else {
      this.lineItems.push(this.newItem);
    }
    this.newItem = { product: undefined, quantity: 0 };
    console.log(this.lineItems);
  }

  finalizeOrder() {
    this.invoiceStep = 3;
  }

  async initialize(): Promise<void> {
    this.customers = await customerService.getCustomer();
    this.inventory = await inventoryService.getInventory();
  }

  prev(): void {
    if (this.invoiceStep == 1) {
      return;
    }
    this.invoiceStep -= 1;
  }

  next(): void {
    if (this.invoiceStep == 3) {
      return;
    }
    this.invoiceStep += 1;
  }

  async created() {
    await this.initialize();
  }

  async submitInvoice(): Promise<void> {
    this.invoice = {
      customerId: this.selectedCustomerId,
      lineItems: this.lineItems,
      createOn: new Date(),
      updateOn: new Date(),
    };
    await invoiceService.makeNewInvoice(this.invoice);

    this.downloadPdf();
    await this.$router.push("/orders");
  }

  downloadPdf() {
    let pdf = new jsPDF("p", "pt", "a4", true);
    let invoice = document.getElementById("invoice");
    // let width=this.$refs.invoice.clientWidth;
    // let height=this.$refs.invoice.clientHeight;
    const pageWidth = pdf.internal.pageSize.getWidth();
    const pageHeight = pdf.internal.pageSize.getHeight();
    html2canvas(invoice).then((canvas) => {
      let image = canvas.toDataURL("image/png");
      const widthRatio = pageWidth / canvas.width;
      const heightRatio = pageHeight / canvas.height;
      const ratio = widthRatio > heightRatio ? heightRatio : widthRatio;

      const canvasWidth = canvas.width * ratio;
      const canvasHeight = canvas.height * ratio;

      const marginX = (pageWidth - canvasWidth) / 2;
      const marginY = (pageHeight - canvasHeight) / 2;
      pdf.addImage(image, "PNG", marginX, marginY, canvasWidth, canvasHeight);
      pdf.save("invoice");
    });
  }
}
</script>

<style lang="scss" scoped>
@import "@/scss/global.scss";

.invoice-step-action {
  display: flex;
  width: 100%;
}

.invoice-step {
}

.invoice-step-detail {
  margin: 1.2rem;
}

.invoice-order-list {
  margin-top: 1.2rem;
  padding: 0.8rem;

  .line-item {
    display: flex;
    border-bottom: 1px dashed #ccc;
    padding: 0.8rem;
  }

  .item-col {
    flex-grow: 1;
  }
}

.invoice-item-actions {
  display: flex;
}

.price-pre-tax {
  font-weight: bold;
}

.price-final {
  font-weight: bold;
  color: $solar-green;
}

.due {
  font-weight: bold;
}

.invoice-header {
  width: 100%;
  margin-bottom: 1.2rem;
}

.invoice-logo {
  padding-top: 1.4rem;
  text-align: center;

  img {
    width: 280px;
  }
}
</style>
