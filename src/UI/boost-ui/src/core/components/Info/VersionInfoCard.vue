<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title>Boost Version</v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text v-if="version">
      <v-row>
        <v-col md="4">PackageId</v-col>
        <v-col
          ><strong>{{ version.packageId }}</strong></v-col
        >
      </v-row>
      <v-row>
        <v-col md="4">Installed</v-col>
        <v-col
          ><strong>{{ version.installed }}</strong></v-col
        >
      </v-row>
      <v-row>
        <v-col md="4">Latest </v-col>
        <v-col
          ><strong
            >{{ version.latest.version }} ({{
              version.latest.published | dateformat
            }})</strong
          ></v-col
        >
      </v-row>
      <v-row v-if="version.preRelease">
        <v-col md="4">Latest Pre-Release </v-col>
        <v-col
          ><strong
            >{{ version.preRelease.version }} ({{
              version.preRelease.published | dateformat
            }})</strong
          ></v-col
        >
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script>
import { mapState } from "vuex";
export default {
  computed: {
    ...mapState("app", ["version"]),
  },
};
</script>

<style>
</style>