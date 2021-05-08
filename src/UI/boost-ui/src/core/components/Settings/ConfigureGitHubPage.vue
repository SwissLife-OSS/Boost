<template>
  <div class="ma-4">
    <v-card>
      <v-toolbar color="grey lighten-2" elevation="0" height="44">
        <v-avatar size="40">
          <img alt="AzureDevOps" src="@/assets/cs/GitHub.png" />
        </v-avatar>
        <h4 class="ml-4">Configure GitHub {{ config.name }}</h4>
        <v-spacer> </v-spacer>
        <v-icon color="grey darken-3" @click="$router.back()">mdi-close</v-icon>
      </v-toolbar>
      <v-card-text>
        <v-form ref="form" v-model="valid" lazy-validation>
          <v-row dense>
            <v-col md="8"
              ><v-text-field
                v-model="config.name"
                :rules="config.nameRules"
                label="Name"
              ></v-text-field
            ></v-col>
            <v-col md="4"
              ><v-select
                :items="configModes"
                v-model="config.mode"
                label="Mode"
              ></v-select
            ></v-col>
          </v-row>
          <v-row dense v-if="config.mode === 'OAuth'">
            <v-col md="4"
              ><v-text-field
                v-model="config.oauth.clientId"
                :rules="config.clientIdRules"
                label="ClientId"
              ></v-text-field
            ></v-col>
            <v-col md="4"
              ><v-text-field
                label="Secret"
                v-model="config.oauth.secret"
                :rules="config.secretRules"
              ></v-text-field
            ></v-col>
            <v-col md="4">
              <p>
                <a target="_blank" href="https://github.com/settings/developers"
                  >Configure GitHub App</a
                >
              </p>
            </v-col>
          </v-row>
          <v-row dense v-if="config.mode === 'PersonalAccessToken'">
            <v-col md="12"
              ><v-text-field
                v-model="config.personalAccessToken"
                label="Personal Access Token"
                :rules="config.tokenRules"
              ></v-text-field
            ></v-col>
          </v-row>
          <v-row dense>
            <v-col md="6"
              ><v-select
                v-model="config.defaultWorkRoot"
                :items="workRoots"
                label="Default work root"
                item-text="name"
                item-value="name"
              ></v-select
            ></v-col>
            <v-col md="6"
              ><v-text-field
                v-model="config.owner"
                :rules="config.ownerRules"
                label="Owner"
              ></v-text-field
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
import {
  getConnectedService,
  mapService,
  getServicePropertyValue,
} from "../../settingsService";
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
        mode: "PersonalAccessToken",
        defaultWorkRoot: null,
        owner: null,
        oauth: {
          clientId: null,
          secret: null,
        },
        personalAccessToken: null,
        nameRules: new FormRuleBuilder("Name", this).addRequired(2).build(),
        ownerRules: new FormRuleBuilder("Owner", this).addRequired(3).build(),
        tokenRules: new FormRuleBuilder("Token", this)
          .addRequired(10, (form) => form.config.mode === "PersonalAccessToken")
          .build(),
        clientIdRules: new FormRuleBuilder("ClientId", this)
          .addRequired(5, (form) => form.config.mode === "OAuth")
          .build(),
        secretRules: new FormRuleBuilder("Secret", this)
          .addRequired(5, (form) => form.config.mode === "OAuth")
          .build(),
      },
      configModes: ["PersonalAccessToken", "OAuth"],
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
        type: "GitHub",
        properties: [],
        defaultWorkRoot: this.config.defaultWorkRoot,
      };

      input.properties.push({
        name: "Mode",
        value: this.config.mode,
      });
      input.properties.push({
        name: "Owner",
        value: this.config.owner,
      });
      if (this.config.mode === "PersonalAccessToken") {
        input.properties.push({
          name: "PersonalAccessToken",
          value: this.config.personalAccessToken,
        });
      } else {
        input.properties.push({
          name: "OAuth.ClientId",
          value: this.config.oauth.clientId,
        });
        input.properties.push({
          name: "OAuth.Secret",
          value: this.config.oauth.secret,
        });
      }

      const service = this.saveConnectedService(input);
      this.config.id = service.id;
    },
    async loadService() {
      const result = await getConnectedService(this.id);
      const { connectedService } = result.data;
      mapService(this.config, connectedService);

      if (this.config.mode === "OAuth") {
        this.config.oauth = {};
        this.config.oauth.clientId = getServicePropertyValue(
          connectedService,
          "OAuth.ClientId"
        );
        this.config.oauth.secret = getServicePropertyValue(
          connectedService,
          "OAuth.Secret"
        );
      }
    },
  },
};
</script>
<style>
</style>