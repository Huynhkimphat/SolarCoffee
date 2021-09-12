<template>
  <div class="inventory-container">
    <h1 id="inventory-title">Inventory Dashboard</h1>
    <hr />

    <div class="inventory-actions">
      <solar-button id="addNewBtn" @click.native="showNewProductModal">
        Add new item
      </solar-button>
      <solar-button id="receiveShipmentBtn" @click.native="showShipmentModal">
        Receive Shipment
      </solar-button>
    </div>

    <table id="inventory-table" class="table">
      <tr>
        <th>Item</th>
        <th>Quantity On-hand</th>
        <th>Unit Price</th>
        <th>Taxable</th>
        <th>Delete</th>
      </tr>

      <tr v-for="item in inventory" :key="item.id">
        <td>
          {{ item.product.name }}
        </td>
        <td>
          {{ item.quantityOnHand }}
        </td>
        <td>
          {{ item.product.price | price }}
        </td>
        <td>
          <span v-if="item.product.isTaxable">
            Yes
          </span>
          <span v-else>
            No
          </span>
        </td>
        <td>
          <div>x</div>
        </td>
      </tr>
    </table>

    <new-product-modal
      v-if="isNewProductVisible"
      @close="closeModals"
      @save:product="saveNewProduct"
    />
    <shipment-modal
      v-if="isShipmentVisible"
      :inventory="inventory"
      @close="closeModals"
      @save:shipment="saveNewShipment"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IProduct, IProductInventory } from "@/types/Product";
import SolarButton from "@/components/SolarButton.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import NewProductModal from "@/components/modals/NewProductModal.vue";
import { IShipment } from "@/types/Shipment";
import { InventoryService } from "@/services/inventory-service";

const inventoryService=new InventoryService();
@Component({
  name: "Inventory",
  components: { SolarButton, ShipmentModal, NewProductModal }
})
export default class Inventory extends Vue {
  isNewProductVisible = false;
  isShipmentVisible = false;

  inventory: IProductInventory[] = [];

  closeModals() {
    this.isNewProductVisible = false;
    this.isShipmentVisible = false;
  }

  showNewProductModal() {
    this.isNewProductVisible = true;
  }

  showShipmentModal() {
    this.isShipmentVisible = true;

  }

  saveNewProduct(newProduct: IProduct) {
    console.log("SaveNewProduct");
    console.log(newProduct);

  }

  saveNewShipment(shipment: IShipment) {
    console.log("SaveNewShipment");
    console.log(shipment);
  }

  async initialize() {
      this.inventory=await inventoryService.getInventory();
  }

  async created() {
    await this.initialize();
  }

}
</script>

<style lang="scss" scoped>
</style>
