import { make } from "vuex-pathify";
import { IInventoryTimeline } from "@/types/InventoryGraph";
import InvoiceService from "@/services/invoice-service";

class GlobalStore {
  snapshotTimeline: IInventoryTimeline = {
    productInventorySnapshots: [],
    timelines: [],
  };
  isTimelineBuilt = false;
}
const state = new GlobalStore();

const mutations = make.mutations(state);

const actions = {
  async assignSnapshots({ commit }) {
    const inventoryService = new InvoiceService();
    const res = await inventoryService.getSnapshotHistory();

    console.log(':: assignSnapshots :: ',res)

    const timeline: IInventoryTimeline = {
      productInventorySnapshots: res.productInventorySnapshots,
      timelines: res.timelines,
    };
    console.log(':: assignSnapshots :: timeline :: ',timeline.timelines)

    commit("SET_SNAPSHOT_TIMELINE", timeline);
    commit("SET_IS_TIMELINE_BUILT", true);
  },
};

const getters = {};

export default {
  state,
  mutations,
  actions,
  getters,
};
