<template>
  <v-row>
    <v-col md="12">
      <v-card :loading="loading">
        <v-toolbar height="44" color="grey lighten-2" elevation="0">
          <v-img
            v-if="repo && repo.remote"
            max-height="26"
            max-width="26"
            @click="openExternal(repo.remote.webUrl)"
            :src="serviceImage(repo.remote)"
          ></v-img>
          <v-toolbar-title class="ml-4" v-if="repo">{{
            repo.name
          }}</v-toolbar-title>
          <v-spacer></v-spacer>

          <template v-slot:extension>
            <v-tabs background-color="grey lighten-3" height="44" v-model="tab">
              <v-tab>Overview</v-tab>
              <v-tab>Quick Actions</v-tab>
              <v-tab>Commits</v-tab>
              <v-tab>ReadMe</v-tab>
              <v-tab>Files</v-tab>
            </v-tabs>
          </template>
        </v-toolbar>
        <v-card-text
          :style="{
            'max-height': cardHeight + 'px',
            height: cardHeight + 'px',
          }"
          class="overflow-y-auto mt-0"
        >
          <v-tabs-items v-model="tab">
            <v-tab-item>
              <v-card elevation="1">
                <v-card-text v-if="repo">
                  <v-row>
                    <v-col md="3"><label>Branch</label></v-col>
                    <v-col md="9">
                      <v-select
                        outlined
                        dense
                        :items="repo.branches"
                        item-text="name"
                        item-value="name"
                        v-model="branch"
                      >
                      </v-select
                    ></v-col>
                  </v-row>
                  <v-row>
                    <v-col md="3">Working Directory</v-col>
                    <v-col md="9"
                      ><strong>{{ repo.workingDirectory }}</strong></v-col
                    >
                  </v-row>
                  <v-row v-if="repo.remote">
                    <v-col md="3">Remote</v-col>
                    <v-col md="9"
                      ><a href="#" @click="onClickRemote(repo.remote)"
                        ><strong>{{ repo.remote.fullName }}</strong></a
                      >
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col md="3">Head</v-col>
                    <v-col md="9"
                      ><strong>{{
                        `+${repo.head.aheadBy} -${repo.head.behindBy}
                      | ${repo.head.message}`
                      }}</strong></v-col
                    >
                  </v-row>
                </v-card-text>
              </v-card>
            </v-tab-item>
            <v-tab-item>
              <quick-actions-list
                :actions="quickActions"
                :height="cardHeight - 50"
              ></quick-actions-list>
            </v-tab-item>
            <v-tab-item>
              <commit-list
                :height="cardHeight - 50"
                :commits="repo && repo.commits"
              ></commit-list>
            </v-tab-item>
            <v-tab-item>
              <div
                v-if="repo && repo.readme"
                :style="{
                  'max-height': $vuetify.breakpoint.height - 150 + 'px',
                }"
                class="overflow-y-auto mt-0"
              >
                <vue-simple-markdown
                  :heading="true"
                  :table="true"
                  :source="repo.readme"
                ></vue-simple-markdown></div
            ></v-tab-item>
            <v-tab-item>
              <file-explorer-view
                :files="files"
                :height="$vuetify.breakpoint.height - 200"
              ></file-explorer-view>
            </v-tab-item>
          </v-tabs-items>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script>
import { mapActions, mapState } from "vuex";
import { getQuickActions, getDirectoryChildren } from "../../workspaceService";
import FileExplorerView from "../Workspace/FileExplorerView.vue";
import QuickActionsList from "../Workspace/QuickActionsList.vue";
import CommitList from "./CommitList.vue";
export default {
  components: { QuickActionsList, FileExplorerView, CommitList },
  props: ["id"],
  watch: {
    id: {
      immediate: true,
      handler: function () {
        this.loadRepo();
      },
    },
    tab: function (value) {
      if (value === 2) {
        this.loadFiles();
      }
    },
  },
  data() {
    return {
      files: [],
      branch: null,
      tab: null,
      quickActions: [],
    };
  },
  computed: {
    ...mapState("git", {
      repo: (state) => state.localDetails.data,
      loading: (state) => state.localDetails.loading,
    }),
    cardHeight: function () {
      return this.$vuetify.breakpoint.height - 200;
    },
  },
  methods: {
    ...mapActions("git", ["getLocalRepo"]),
    async loadRepo() {
      const repo = await this.getLocalRepo(this.id);
      this.branch = repo.head.name;
      this.loadQuickActions();
    },
    async loadFiles() {
      const result = await getDirectoryChildren(this.repo.workingDirectory);

      this.files = result.data.directoryChildren.map((x) => {
        if (x.type === "DIRECTORY") x.children = [];
        return x;
      });
    },
    serviceImage: function (repo) {
      if (repo.service) {
        return require(`../../../assets/cs/${repo.service.type}.png`);
      }

      return require("../../../assets/git.png");
    },
    openExternal(url) {
      window.open(url);
    },
    onClickRemote: function (remote) {
      this.$router.push({
        name: "Git.Repo.Details",
        params: { id: remote.id, serviceId: remote.service.id },
      });
    },
    async loadQuickActions() {
      const result = await getQuickActions(this.repo.workingDirectory);
      this.quickActions = result.data.quickActions;
    },
  },
};
</script>

<style>
</style>