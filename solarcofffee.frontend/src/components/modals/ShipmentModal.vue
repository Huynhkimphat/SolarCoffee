<template>
  <solar-modal>
    <template v-slot:header> Receive Shipment</template>
    <template v-slot:body>
      <label for="product">Product Received: </label>
      <select id="product" v-model="selectedProduct" class="shipmentItems">
        <option disabled value="">Please Select One</option>
        <option v-for="item in inventory" :key="item.product.id" :value="item">
          {{ item.product.name }}
        </option>
      </select>

      <label for="qtyReceived">Quantity Received: </label>
      <input id="qtyReceived" v-model="qtyReceived" type="number" />
    </template>
    <template v-slot:footer>
      <solar-button
        aria-label="Save new shipment"
        type="button"
        @button:click="save"
      >
        Save Received Shipment
      </solar-button>
      <solar-button
        aria-label="Close Modal"
        type="button"
        @button:click="close"
      >
        Close
      </solar-button>
    </template>
  </solar-modal>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";
import SolarModal from "@/components/modals/SolarModal.vue";
import { IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";

@Component({
  name: "ShipmentModal",
  components: { SolarButton, SolarModal },
})
export default class ShipmentModal extends Vue {
  @Prop({ required: true, type: Array as () => IProductInventory[] })
  inventory!: IProductInventory[];

  selectedProduct: IProductInventory = {
    id: 0,
    product: {
      id: 0,
      createOn: new Date(),
      updateOn: new Date(),
      name: "",
      description: "",
      isTaxable: false,
      price: 0,
      isArchived: false,
    },
    quantityOnHand: 0,
    idealQuantity: 0,
  };

  qtyReceived = 0;

  close() {
    this.$emit("close");
  }

  save() {
    let shipment: IShipment = {
      productId: this.selectedProduct.product.id,
      adjustment: this.qtyReceived,
    };
    this.$emit("save:shipment", shipment);
  }
}
</script>

<style scoped></style>
