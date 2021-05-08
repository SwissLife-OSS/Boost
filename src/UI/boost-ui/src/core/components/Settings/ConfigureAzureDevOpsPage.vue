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
        <v-form ref="form" v-model="valid" lazy-validation>
          <v-row dense>
            <v-col md="12"
              ><v-text-field
                v-model="config.name"
                :rules="config.nameRules"
                label="Name"
              ></v-text-field
            ></v-col>
          </v-row>
          <v-row dense>
            <v-col md="6"
              ><v-text-field
                v-model="config.account"
                :rules="config.accountRules"
                label="Account"
              ></v-text-field
            ></v-col>
            <v-col md="6"
              ><v-text-field
                label="Default Project"
                v-model="config.defaultProject"
                :rules="config.defaultProjectRules"
              ></v-text-field
            ></v-col>
          </v-row>
          <v-row dense>
            <v-col md="12"
              ><v-text-field
                v-model="config.personalAccessToken"
                label="Personal Access Token"
                :rules="config.tokenRules"
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
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="primary" :disabled="!valid" @click="save">Save</v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script>
import { getConnectedService, mapService } from "../../settingsService";
import { FormRuleBuilder } from "../Common/FormRuleBuilder";

import configureServiceMixin from "./configureServiceMixin";
export default {
  mixins: [configureServiceMixin],
  data() {
    return {
      valid: false,
      config: {
        id: null,
        name: "",
        account: "",
        defaultProject: null,
        personalAccessToken: null,
        defaultWorkRoot: null,
        nameRules: new FormRuleBuilder("Name", this).addRequired(2).build(),
        accountRules: new FormRuleBuilder("Account", this)
          .addRequired(2)
          .build(),
        tokenRules: new FormRuleBuilder("Token", this).addRequired(10).build(),
        defaultProjectRules: new FormRuleBuilder("Default project", this)
          .addRequired(2)
          .build(),
      },
    };
  },
  methods: {
    async save() {
      const isValid = this.$refs.form.validate();

      if (!isValid) {
        return;
      }

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