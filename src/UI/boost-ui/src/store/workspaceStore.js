import { getWorkspace, openInCode, executeCommand } from "../core/workspaceService";
import { excuteGraphQL } from "../store/graphqlClient";

const workspaceStore = {
    namespaced: true,
    state: () => ({
        workspace: null,
        console: {
            messages: []
        }
    }),
    mutations: {
        WORKSPACE_LOADED(state, workspace) {
            state.workspace = workspace;
        },
        CONSOLE_MESSAGE_ADDED(state, message) {
            state.console.messages.push(message);
        },
        CONSOLE_CLEARED(state) {
            state.console.messages = [];
        },
        COMMAND_EXECUTED(state, result) {
            console.log(state, result);
        }
    },
    actions: {
        async getWorkspace({ commit, dispatch }) {
            const result = await excuteGraphQL(() => getWorkspace(), dispatch);
            if (result.success) {
                commit("WORKSPACE_LOADED", result.data.workspace);
                return result.data.workspace;
            }

            return null;
        },
        async openInCode({ dispatch }, input) {
            const result = await excuteGraphQL(() => openInCode(input), dispatch);
            if (result.success) {
                return result.data;
            }

            return null;
        },
        async executeCommand({ commit }, command) {

            const result = await excuteGraphQL(() => executeCommand(command));

            if (result.success) {
                commit("COMMAND_EXECUTED", result.data)
                return result.data;
            }
            return null;

        },
        async addConsoleMessage({ commit }, message) {
            commit("CONSOLE_MESSAGE_ADDED", message);
        },
        clearConsoleMessages: function ({ commit }) {
            commit("CONSOLE_CLEARED");
        }
    },
    getters: {

    }
};

export default workspaceStore;
