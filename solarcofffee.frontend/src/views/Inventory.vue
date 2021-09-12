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

@Component({
  name: "Inventory",
  components: { SolarButton, ShipmentModal, NewProductModal }
})
export default class Inventory extends Vue {
  isNewProductVisible= false;
  isShipmentVisible = false;

  inventory: IProductInventory[] = [
    {
      id: 1,
      product: {
        id: 1,
        name: "Some Products",
        createOn: new Date(),
        updateOn: new Date(),
        description: "Good Stuff",
        price: 100,
        isTaxable: true,
        isArchived: false
      },
      quantityOnHand: 100,
      idealQuantity: 100
    },
    {
      id: 2,
      product: {
        id: 2,
        name: "Another Products 2",
        createOn: new Date(),
        updateOn: new Date(),
        description: "Good Stuff",
        price: 100,
        isTaxable: true,
        isArchived: false
      },
      quantityOnHand: 40,
      idealQuantity: 20
    }
  ];

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

  saveNewProduct(newProduct:IProduct) {
    console.log('SaveNewProduct');
    console.log(newProduct);

  }

  saveNewShipment(shipment:IShipment) {
    console.log('SaveNewShipment');
    console.log(shipment);
  }

}
</script>

<style lang="scss" scoped>
</style>
