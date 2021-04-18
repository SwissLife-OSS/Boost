<template>
  <div class="ma-4">
    <v-card>
      <v-toolbar color="grey lighten-2" elevation="0" height="44">
        <v-avatar size="40">
          <img alt="AzureDevOps" src="@/assets/cs/AzureDevOps.png" />
        </v-avatar>
        <h4 class="ml-4">Configure {{ config.name }} {{ config.id }}</h4>
        <v-spacer> </v-spacer>
        <v-icon color="grey darken-3" @click="$router.back()">mdi-close</v-icon>
      </v-toolbar>

      <v-card-text>
        <v-row dense>
          <v-col md="12"
            ><v-text-field v-model="config.name" label="Name"></v-text-field
          ></v-col>
        </v-row>
        <v-row dense>
          <v-col md="6"
            ><v-text-field
              v-model="config.account"
              label="Account"
            ></v-text-field
          ></v-col>
          <v-col md="6"
            ><v-text-field
              label="Default Project"
              v-model="config.defaultProject"
            ></v-text-field
          ></v-col>
        </v-row>
        <v-row dense>
          <v-col md="12"
            ><v-text-field
              v-model="config.personalAccessToken"
              label="Personal Access Token"
            ></v-text-field
          ></v-col>
        </v-row>
        <v-row dense>
          <v-col md="12"
            ><v-select
              v-model="config.defaultWorkRoot"
              :items="workRoots"
              label="Default work root"
              item-text="name"
              item-value="name"
            ></v-select
          ></v-col>
        </v-row>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="primary" @click="save">Save</v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script>
import { getConnectedService, mapService } from "../../settingsService";

import configureServiceMixin from "./configureServiceMixin";
export default {
  mixins: [configureServiceMixin],
  data() {
    return {
      config: {
        id: null,
        name: "",
        account: "",
        defaultProject: null,
        personalAccessToken: null,
        defaultWorkRoot: null,
      },
    };
  },
  methods: {
    async save() {
      const input = {
        id: this.config.id,
        name: this.config.name,
        type: "AzureDevOps",
        defaultWorkRoot: this.config.defaultWorkRoot,
        properties: [],
      };

      input.properties.push({
        name: "Account",
        value: this.config.account,
      });
      input.properties.push({
        name: "DefaultProject",
        value: this.config.defaultProject,
      });
      input.properties.push({
        name: "PersonalAccessToken",
        value: this.config.personalAccessToken,
      });

      const service = this.saveConnectedService(input);
      this.config.id = service.id;
    },
    async loadService() {
      const result = await getConnectedService(this.id);
      const { connectedService } = result.data;

      mapService(this.config, connectedService);
    },
  },
};
</script>

<style>
</style>