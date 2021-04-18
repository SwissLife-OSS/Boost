<template>
  <div>
    <v-alert
      class="mt-4"
      outlined
      type="info"
      v-if="pipelines.length === 0 && !loading"
    >
      <p>Not pipelines found for this repository.</p>
      <a
        href="https://github.com/SwissLife-OSS/Boost#readme"
        style="color: blue"
        class="text-decoration-none"
        >Learn how the link existing pipelines</a
      >
    </v-alert>

    <v-list two-line v-else>
      <v-list-item
        v-for="pipeline in pipelines"
        :key="pipeline.id"
        @click="openExternal(pipeline.webUrl)"
      >
        <v-list-item-avatar :title="pipeline.result" size="42">
          <v-icon v-if="pipeline.result === 'Succeeded'" size="40" color="green"
            >mdi-check-circle</v-icon
          >
          <v-icon v-if="pipeline.result === 'Failed'" size="40" color="red"
            >mdi-alert-circle</v-icon
          >
        </v-list-item-avatar>
        <v-list-item-content>
          <v-list-item-title
            ><strong>{{ pipeline.name }}</strong></v-list-item-title
          >
          <div>
            <v-row>
              <v-col md="3">{{ pipeline.startedAt | dateformat }}</v-col>
              <v-col md="4"> {{ pipeline.lastRunName }}</v-col>
              <v-col md="4"> {{ pipeline.requestedFor }}</v-col>
            </v-row>
          </div>
          <div v-if="pipeline.relases">
            <v-row
              v-for="release in pipeline.relases"
              :key="release.id"
              @click.stop="openExternal(release.webUrl)"
            >
              <v-col md="1">
                <v-icon color="blue">mdi-rocket-launch-outline</v-icon></v-col
              >
              <v-col md="3">{{ release.createdOn | dateformat }}</v-col>
              <v-col md="4"> {{ release.name }}</v-col>
              <v-col md="4"> {{ release.createdFor }}</v-col>
            </v-row>
          </div>
        </v-list-item-content>
        <v-list-item-action> </v-list-item-action>
      </v-list-item>
    </v-list>
  </div>
</template>

<script>
import { getPipelinesByRepo } from "../../pipelinesService";

export default {
  props: ["serviceId", "repositoryId"],

  watch: {
    repositoryId: {
      immediate: true,
      handler: function () {
        this.loadPipelines();
      },
    },
  },
  data() {
    return {
      loading: true,
      pipelines: [],
    };
  },

  methods: {
    async loadPipelines() {
      this.loading = true;
      const result = await getPipelinesByRepo({
        serviceId: this.serviceId,
        repositoryId: this.repositoryId,
      });

      this.pipelines = result.data.pipelines.map((x) => {
        if (x.runs && x.runs.length > 0) {
          const lastRun = x.runs[0];

          x.result = lastRun.result;
          x.lastRunName = lastRun.name;
          x.lastRunLink = lastRun.webLink;
          x.requestedFor = lastRun.requestedFor;
          x.startedAt = lastRun.startedAt;
        } else {
          x.result = "NoRuns";
        }

        return x;
      });

      this.loading = false;
    },
    openExternal(url) {
      window.open(url);
    },
  },
};
</script>

<style>
</style>