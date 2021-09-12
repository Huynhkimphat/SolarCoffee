<template>
  <solar-modal>
    <template v-slot:header>
      Add new product
    </template>
    <template v-slot:body>
      <ul class="newProduct">
        <li>
          <label for="isTaxable">Is this product taxable?</label>
          <input id="isTaxable" v-model="newProduct.isTaxable" type="checkbox" />
        </li>
        <li>
          <label for="productName">Name: </label>
          <input id="productName" v-model="newProduct.name" type="text" />
        </li>
        <li>
          <label for="productDesc">Description: </label>
          <input id="productDesc" v-model="newProduct.description" type="text" />
        </li>
        <li>
          <label for="productPrice">Price(USD): </label>
          <input id="productPrice" v-model="newProduct.price" type="number" />
        </li>
      </ul>
    </template>
    <template v-slot:footer>
      <solar-button
        aria-label="save new item"
        type="button"
        @click.native="save"
      >
        Save Product
      </solar-button>
      <solar-button
        aria-label="close modal"
        type="button"
        @click.native="close"
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
import { IProduct } from "@/types/Product";

@Component({
  name: "NewProductModal",
  components: { SolarButton, SolarModal }
})
export default class NewProductModal extends Vue {
  newProduct: IProduct = {
    createOn: new Date(),
    updateOn: new Date(),
    id: 0,
    name:"",
    description: "",
    isTaxable: false,
    price: 0,
    isArchived: false
  };

  close() {
    this.$emit("close");
  }

  save() {
    this.$emit("save:product", this.newProduct);
  }
}
</script>

<style lang="scss" scoped>
.newProduct {
  list-style: none;
  padding: 0;
  margin: 0;

  input {
    width: 100%;
    height: 1.8rem;
    margin-bottom: 1.2rem;
    font-size: 1.1rem;
    line-height: 1.3rem;
    padding: 0.2rem;
    color: #555;
  }

  label {
    font-weight: bold;
    display: block;
    margin-bottom: 0.3rem;
  }
}
</style>
