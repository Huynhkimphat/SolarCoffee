<template>
  <div v-if="isTimelineBuilt">
    <apexchart
      type="area"
      :width="'100%'"
      height="300"
      :options="options"
      :series="series"
    ></apexchart>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IInventoryTimeline } from "@/types/InventoryGraph";
import { Get, Sync } from "vuex-pathify";

import VueApexCharts from "vue-apexcharts";
import moment from "moment";

Vue.component("apexchart", VueApexCharts);

@Component({
  name: "InventoryChart",
  components: {},
})
export default class InventoryChart extends Vue {
  @Sync("snapshotTimeline")
  snapshotTimeline?: IInventoryTimeline;

  @Get("isTimelineBuilt")
  isTimelineBuilt?: boolean;

  get options() {
    return {
      dataLabels: { enabled: false },
      fill: {
        type: "gradient",
      },
      stroke: {
        curve: "smooth",
      },
      xaxis: {
        // categories: this.snapshotTimeline.timelines.map(t=>moment(t).format('dd HHMMss')),
        categories: this.snapshotTimeline.timelines,
        type: "datetime",
      },
    };
  }
  get series() {
    return this.snapshotTimeline.productInventorySnapshots.map((snapshot) => ({
      name: snapshot.productId,
      data: snapshot.quantityOnHand,
    }));
  }

  async created() {
    await this.$store.dispatch("assignSnapshots");
    console.log(this.snapshotTimeline.timelines);
  }
}
</script>

<style scoped></style>
