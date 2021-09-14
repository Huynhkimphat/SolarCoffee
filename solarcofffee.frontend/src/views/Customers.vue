<template>
  <div>
    <h1 id="customersTitle">Manage Customers</h1>
    <hr />
    <div class="customersAction">
      <solar-button @button:click="showNewCustomerModal">
        Add Customer
      </solar-button>
    </div>
    <table id="customers" class="table">
      <tr>
        <th>Customer</th>
        <th>Address</th>
        <th>State</th>
        <th>Since</th>
        <th>Delete</th>
      </tr>
      <tr v-for="customer in customers" :key="customer.id">
        <td>
          {{ customer.firstName + " " + customer.lastName }}
        </td>
        <td>
          {{ customer.primaryAddress.addressLine1 + " " + customer.primaryAddress.addressLine2 + " " + customer.primaryAddress.addressLine3
          }}
        </td>
        <td>
          {{ customer.primaryAddress.state }}
        </td>
        <td>
          {{ customer.createOn | humanizeDate }}
        </td>
        <td>
          <div
            class="lni-cross-circle customer-delete"
            @click="deleteCustomer(customer.id)"
          ></div>
        </td>
      </tr>
    </table>
    <new-customer-modal
      v-if="isCustomerModalVisible"
      @close="closeModal"
      @save:customer="saveNewCustomer"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SolarButton from '@/components/SolarButton';
import { ICustomer } from "@/types/Customer";
import CustomerService from "@/services/customer-service";
import NewCustomerModal from "@/components/modals/NewCustomerModal.vue";

const customerService = new CustomerService();

@Component({
  name: "Customers",
  components: { SolarButton, NewCustomerModal }
})
export default class Customers extends Vue {
  customers: ICustomer[] = [];
  isCustomerModalVisible = false;

  showNewCustomerModal() {
    this.isCustomerModalVisible = true;
  }

  closeModal() {
    this.isCustomerModalVisible = false;
  }

  async initialize() {
    this.customers = await customerService.getCustomer();
  }

  async saveNewCustomer(newCustomer: ICustomer) {
    await customerService.addCustomer(newCustomer);
    this.isCustomerModalVisible = false;
    await this.initialize();
  }

  async created() {
    await this.initialize();
  }

  async deleteCustomer(customerId: number) {
    await customerService.deleteCustomer(customerId);
    await this.initialize();
  }
}
</script>

<style lang="scss" scoped>
@import "@/scss/global.scss";

.customer-actions {
  display: flex;
  margin-bottom: 0.8rem;
  div {
    margin-right: 0.8rem;
  }
}

.customer-delete {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $solar-red;
}
</style>
