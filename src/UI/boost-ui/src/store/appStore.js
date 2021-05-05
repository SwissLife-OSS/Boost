
import { getInitialData } from "../core/appService";
import { saveConnectedService, saveTokenRequestor, saveWorkRoots, getConnectedServices } from "../core/settingsService";
import { excuteGraphQL } from "./graphqlClient";

const appStore = {
    namespaced: true,
    state: () => ({
        statusMessage: null,
        console: [],
        userSettings: null,
        app: null,
        connectedServices: []
    }),
    mutations: {
        MESSAGE_ADDED(state, message) {
            state.statusMessage = message;

            window.setTimeout(() => {
                state.statusMessage = null;
            }, 5000)
        },
        CONSOLE_MESSAGE(state, message) {
            state.console.push(message);
        },
        INIT_DATA_LOADED(state, data) {
            state.userSettings = data.userSettings;
            state.app = data.appliation
        },
        WORKROOTS_SAVED(state, workRoots) {
            state.userSettings.workRoots = workRoots;
        },
        TOKENREQUESTOR_SAVED(state, settings) {
            state.userSettings.tokenGenerator = settings;
        },
        CONNECTED_SERVICE_SAVED(state, service) {
            console.log(state, service);
        },
        CONNECTED_SERVICES_LOADED(state, services) {
            state.connectedServices = services;
        }
    },
    actions: {
        addMessage: function ({ commit }, message) {
            commit("MESSAGE_ADDED", message)
        },
        async loadInitialData({ commit, dispatch }) {
            const result = await excuteGraphQL(() => getInitialData(), dispatch);
            if (result.success) {
                commit("INIT_DATA_LOADED", result.data);
            }
        },
        async saveWorkRoots({ commit, dispatch }, data) {
            const result = await excuteGraphQL(() => saveWorkRoots(data), dispatch);
            if (result.success) {
                commit("WORKROOTS_SAVED", data.workRoots);
                dispatch(
                    "app/addMessage",
                    { text: "Saved", type: "SUCCESS" },
                    { root: true }
                );
            }
        },
        async saveTokenRequestorSettings({ commit, dispatch }, data) {
            const result = await excuteGraphQL(() => saveTokenRequestor(data), dispatch);
            if (result.success) {
                commit("TOKENREQUESTOR_SAVED", data.settings);
                dispatch(
                    "app/addMessage",
                    { text: "Saved", type: "SUCCESS" },
                    { root: true }
                );
            }
        },
        async loadConnectedServices({ commit, dispatch }) {
            const result = await excuteGraphQL(() => getConnectedServices(), dispatch);
            if (result.success) {
                const { connectedServices } = result.data;
                commit("CONNECTED_SERVICES_LOADED", connectedServices);

                return connectedServices;
            }

            return null;
        },
        async saveConnectedService({ commit, dispatch }, service) {
            const result = await excuteGraphQL(() => saveConnectedService(service), dispatch);
            if (result.success) {
                const { connectedService } = result.data.saveConnectedService
                commit("CONNECTED_SERVICE_SAVED", connectedService);
                dispatch(
                    "app/addMessage",
                    { text: "Saved", type: "SUCCESS" },
                    { root: true }
                );

                return connectedService;
            }
            return null;
        },
    },
    getters: {

    }
};

export default appStore;
