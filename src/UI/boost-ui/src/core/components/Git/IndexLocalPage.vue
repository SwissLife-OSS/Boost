<template>
  <v-row>
    <v-col md="4">
      <v-card elevation="1" :loading="scanning">
        <v-toolbar light color="grey lighten-2" elevation="0" height="42">
          <v-toolbar-title>Index Workroots</v-toolbar-title>
          <v-spacer></v-spacer>
        </v-toolbar>
        <v-card-text>
          <p>
            Scan your workroots for Git repos to make them searchable in Boost.
          </p>

          <v-alert v-if="indexCount != null" type="success" outlined>
            Scan completed! <br />Index count: <strong>{{ indexCount }}</strong>
          </v-alert>
        </v-card-text>
        <v-card-actions>
          <router-link :to="{ name: 'Settings' }">Edit work roots</router-link>
          <v-spacer></v-spacer>
          <v-btn color="primary" :disabled="scanning" @click="onStart"
            >Start</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-col>
    <v-col md="8">
      <console :height="$vuetify.breakpoint.height - 130"></console>
    </v-col>
  </v-row>
</template>

<script>
import { indexWorkRoots } from "../../gitService";
import Console from "../Common/Console.vue";

export default {
  components: { Console },
  data() {
    return {
      indexCount: null,
      scanning: false,
    };
  },
  methods: {
    async onStart() {
      this.scanning = true;
      const result = await indexWorkRoots();
      this.indexCount = result.data.indexWorkRoots;
      this.scanning = false;
    },
  },
};
</script>

<style>
</style>