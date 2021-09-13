<template>
  <div class="side-menu-container">
    <router-link to="/">
      <img
        id="imgLogo"
        alt="Solar Coffee Logo"
        src="../assets/images/SolarCoffeeLogo.png"
      />
    </router-link>
    <h1>Management Portal</h1>
    <solar-button
      id="menuInventory"
      isFullwidth
      @button:click="goToRoute('/inventory')"
    >
      Inventory
    </solar-button>
    <solar-button
      id="menuCustomer"
      isFullwidth
      @button:click="goToRoute('/customers')"
    >
      Manage Customers
    </solar-button>
    <solar-button
      id="menuInvoice"
      isFullwidth
      @button:click="goToRoute('/invoice/new')"
    >
      New Invoice
    </solar-button>
    <solar-button
      id="menuOrders"
      isFullwidth
      @button:click="goToRoute('/orders')"
    >
      Orders
    </solar-button>
  </div>
</template>

<script lang="ts">
import SolarButton from "./SolarButton.vue";
import { Component, Vue } from "vue-property-decorator";

@Component({
  name: "SideMenu",
  components: { SolarButton }
})
export default class SideMenu extends Vue {
  goToRoute(route: string) {
    this.$router.push(route);
  }
}
</script>

<style lang="scss" scoped>
@import "../scss/global.scss";

.side-menu-container {
  background-color: #fcfcfc;
  height: 100vh;
  width: $menu-width;
  display: flex;
  flex-direction: column;
  padding: 0.8rem;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.12), 0 1px 1px rgba(0, 0, 0, 0.24);
  box-sizing: border-box;
}

#imgLogo {
  width: 100%;
}

h1 {
  font-size: 1.2rem;
  margin: 1rem 0;
}
</style>
