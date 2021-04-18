<template>
  <v-row>
    <v-col md="12">
      <v-card :loading="loading">
        <v-toolbar height="44" color="grey lighten-2" elevation="0">
          <v-img
            max-height="26"
            max-width="26"
            v-if="repo"
            @click="openExternal(repo.webUrl)"
            :src="serviceImage(repo)"
          ></v-img>
          <v-toolbar-title class="ml-4" v-if="repo">{{
            repo.fullName
          }}</v-toolbar-title>
          <v-spacer></v-spacer>

          <template v-slot:extension>
            <v-tabs background-color="grey lighten-3" height="44" v-model="tab">
              <v-tab>Overview</v-tab>
              <v-tab>Quick Actions</v-tab>
              <v-tab>Commits</v-tab>
              <v-tab>ReadMe</v-tab>
              <v-tab>CI / CD</v-tab>
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
              <v-card v-if="repo" class="ma-1" elevation="1">
                <v-card-text>
                  <v-row v-if="repo.commits.length > 0">
                    <v-col md="3">Last push</v-col>
                    <v-col md="9"
                      ><strong
                        >{{ repo.commits[0].author }} |
                        {{ repo.commits[0].createdAt | dateformat }}</strong
                      ></v-col
                    >
                  </v-row>
                  <v-row>
                    <v-col md="3">Url</v-col>
                    <v-col md="9"
                      ><strong>{{ repo.url }}</strong></v-col
                    >
                  </v-row>
                  <v-row>
                    <v-col md="3">Connected Service</v-col>
                    <v-col md="9"
                      ><strong
                        >{{ repo.service.type }} |
                        {{ repo.service.name }}</strong
                      ></v-col
                    >
                  </v-row>
                </v-card-text>
              </v-card>

              <v-alert
                class="mt-4"
                outlined
                type="info"
                v-if="repo && repo.local"
              >
                <p>This repo is allready cloned here:</p>
                <p>
                  <strong>{{ repo.local.workingDirectory }}</strong>
                </p>
              </v-alert>
            </v-tab-item>

            <v-tab-item>
              <clone-repo-card
                :repo="repo"
                v-if="repo && repo.local == null"
                @updated="onRepoCloned"
              ></clone-repo-card>
              <quick-actions-list
                :actions="quickActions"
                :height="cardHeight - 200"
              ></quick-actions-list>
            </v-tab-item>
            <v-tab-item>
              <commit-list
                v-if="repo"
                :height="cardHeight - 50"
                :commits="repo.commits"
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
              <pipelines-list
                v-if="repo"
                :serviceId="repo.service.id"
                :repositoryId="repo.id"
              ></pipelines-list>
            </v-tab-item>
          </v-tabs-items>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script>
import { mapActions, mapState } from "vuex";
import PipelinesList from "../Pipelines/PipelinesList.vue";
import QuickActionsList from "../Workspace/QuickActionsList";
import workspaceMixins from "../workspaceMixins";
import CloneRepoCard from "./CloneRepoCard.vue";
import CommitList from "./CommitList.vue";

export default {
  mixins: [workspaceMixins],
  components: {
    QuickActionsList,
    PipelinesList,
    CommitList,
    CloneRepoCard,
  },
  props: ["id", "serviceId"],
  data() {
    return {
      tab: null,
      quickActions: [],
    };
  },
  watch: {
    id: {
      immediate: true,
      handler: function () {
        const input = {
          serviceId: this.serviceId,
          id: this.id,
        };
        this.quickActions = [];
        this.getRemoteRepo(input);
      },
    },
    repo: function () {
      this.getQuickActions();
    },
  },
  computed: {
    ...mapState("git", {
      repo: (state) => state.details.data,
      loading: (state) => state.details.loading,
    }),
    ...mapState("app", {
      workRoots: (state) => state.userSettings.workRoots,
    }),
    cardHeight: function () {
      return this.$vuetify.breakpoint.height - 200;
    },
  },
  methods: {
    ...mapActions("git", ["getRemoteRepo"]),
    async getQuickActions() {
      if (this.repo && this.repo.local) {
        this.quickActions = await this.loadQuickActions(
          this.repo.local.workingDirectory
        );
      }
    },
    onRepoCloned: function (data) {
      this.repo.local = {
        workingDirectory: data.directory,
      };

      this.getQuickActions();
    },
    openExternal(url) {
      window.open(url);
    },
    serviceImage: function (repo) {
      return require(`../../../assets/cs/${repo.service.type}.png`);
    },
  },
};
</script>

<style>
</style>