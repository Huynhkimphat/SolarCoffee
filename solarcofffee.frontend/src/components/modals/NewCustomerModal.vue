<template>
  <solar-modal>
    <template v-slot:header>
      Add New Customer
    </template>
    <template v-slot:body>
      <ul class="newCustomer">
        <li>
          <label for="firstName">First Name: </label>
          <input id="firstName" v-model="newCustomer.firstName" type="text" />
        </li>
        <li>
          <label for="lastName">Last Name: </label>
          <input id="lastName" v-model="newCustomer.lastName" type="text" />
        </li>
        <li>
          <label for="addressLine1">Address Line 1: </label>
          <input id="addressLine1" v-model="newCustomer.primaryAddress.addressLine1" type="text" />
        </li>
        <li>
          <label for="addressLine2">Address Line 2: </label>
          <input id="addressLine2" v-model="newCustomer.primaryAddress.addressLine2" type="text" />
        </li>
        <li>
          <label for="addressLine3">Address Line 3: </label>
          <input id="addressLine3" v-model="newCustomer.primaryAddress.addressLine3" type="text" />
        </li>
        <li>
          <label for="city">City: </label>
          <input id="city" v-model="newCustomer.primaryAddress.city" type="text" />
        </li>
        <li>
          <label for="state">State: </label>
          <input id="state" v-model="newCustomer.primaryAddress.state" type="text" />
        </li>
        <li>
          <label for="postalCode">Postal Code: </label>
          <input id="postalCode" v-model="newCustomer.primaryAddress.postalCode" type="text" />
        </li>
        <li>
          <label for="country">Country: </label>
          <input id="country" v-model="newCustomer.primaryAddress.country" type="text" />
        </li>
      </ul>
    </template>
    <template v-slot:footer>
      <solar-button
        aria-label="save new customer"
        type="button"
        @button:click="save"
      >
        Save Customer
      </solar-button>
      <solar-button
        aria-label="close modal"
        type="button"
        @button:click="close"
      >
        Close
      </solar-button>
    </template>
  </solar-modal>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";
import SolarModal from "@/components/modals/SolarModal.vue";
import { ICustomer } from "@/types/Customer";

@Component({
  name: "NewCustomerModal",
  components: { SolarButton, SolarModal }
})
export default class NewCustomerModal extends Vue {
  newCustomer: ICustomer = {
    id: 0,
    createOn: new Date(),
    updateOn: new Date(),
    firstName:"",
    lastName:"",
    primaryAddress:{
      id:0,
      createOn: new Date(),
      updateOn: new Date(),
      addressLine1: "",
      addressLine2: "",
      addressLine3: "",
      city: "",
      state: "",
      postalCode: "",
      country: "",
    }
  };

  close() {
    this.$emit("close");
  }

  save() {
    this.$emit("save:customer", this.newCustomer);
  }
}
</script>

<style lang="scss" scoped>
.newCustomer {
  display: flex;
  flex-wrap: wrap;
  list-style: none;
  padding: 0;
  margin: 0;
  input {
    width: 80%;
    height: 1.8rem;
    margin: 0.8rem 2rem 0.8rem 0.8rem;
    font-size: 1.1rem;
    line-height: 1.3rem;
    padding: 0.2rem;
    color: #555;
  }
  label {
    font-weight: bold;
    margin: 0.8rem;
    display: block;
  }
}
</style>
