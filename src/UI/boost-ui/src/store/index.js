
import Vue from 'vue'
import Vuex from 'vuex'
import appStore from './appStore'
import workspaceStore from './workspaceStore'
import gitStore from "../core/gitStore";

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
  },
  mutations: {
  },
  actions: {
  },
  modules: {
    app: appStore,
    workspace: workspaceStore,
    git: gitStore
  }
})
