import { getRemoteRepo, searchRepos, searchLocalRepos, getLocalRepo } from "./gitService";
import { excuteGraphQL } from "../store/graphqlClient";

const workspaceStore = {
    namespaced: true,
    state: () => ({
        list: {
            loading: false,
            items: []
        },
        local: {
            loading: false,
            items: []
        },
        details: {
            loading: false,
            data: null,
        },
        localDetails: {
            loading: false,
            data: null,
        }
    }),
    mutations: {
        REPOS_LOADED(state, repos) {
            state.list.items = repos;
            state.list.loading = false;
        },
        REPOS_LOCAL_LOADED(state, repos) {
            state.local.items = repos;
            state.local.loading = false;
        },
        REPOS_LIST_LOADING(state, loading) {
            state.list.loading = loading;
        },
        REPOS_LOCAL_LIST_LOADING(state, loading) {
            state.local.loading = loading;
        },
        REPO_DETAILS_LOADED(state, repo) {
            state.details.data = repo;
            state.details.loading = false;
        },
        REPO_DETAILS_LOADING(state, loading) {
            state.details.loading = loading;
        },
        REPO_LOCAL_DETAILS_LOADING(state, loading) {
            state.localDetails.loading = loading;
        },
        REPO_LOCAL_DETAILS_LOADED(state, repo) {
            state.localDetails.data = repo;
            state.localDetails.loading = false;
        },
    },
    actions: {
        async searchRepos({ commit, dispatch }, input) {
            commit("REPOS_LIST_LOADING", true);

            const result = await excuteGraphQL(() => searchRepos(input), dispatch);
            if (result.success) {
                commit("REPOS_LOADED", result.data.searchRepositories);
            }

            return null;
        },
        async searchLocalRepos({ commit, dispatch }, input) {
            commit("REPOS_LOCAL_LIST_LOADING", true);

            const result = await excuteGraphQL(() => searchLocalRepos(input), dispatch);
            if (result.success) {
                commit("REPOS_LOCAL_LOADED", result.data.searchLocalRepositories);
            }

            return null;
        },
        async getRemoteRepo({ commit, dispatch }, input) {
            commit("REPO_DETAILS_LOADING", true);

            const result = await excuteGraphQL(() => getRemoteRepo(input), dispatch);
            if (result.success) {
                commit("REPO_DETAILS_LOADED", result.data.remoteGitRepository);
            }

            return null;
        },
        async getLocalRepo({ commit, dispatch }, id) {
            commit("REPO_LOCAL_DETAILS_LOADING", true);

            const result = await excuteGraphQL(() => getLocalRepo(id), dispatch);
            if (result.success) {

                commit("REPO_LOCAL_DETAILS_LOADED", result.data.localGitRepository);

                return result.data.localGitRepository;
            }

            return null;
        },
    },
    getters: {

    }
};

export default workspaceStore;
