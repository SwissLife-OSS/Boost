<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title>Authorize</v-toolbar-title>
      <v-spacer></v-spacer>
      <strong>{{ save.name }}</strong>
      <template v-slot:extension>
        <v-tabs background-color="grey lighten-3" height="44" v-model="tab">
          <v-tab>Request</v-tab>
          <v-tab>Library</v-tab>
        </v-tabs>
      </template>
    </v-toolbar>
    <v-card-text style="min-height: 400px">
      <v-tabs-items v-model="tab">
        <v-tab-item>
          <v-row dense>
            <v-col md="12">
              <v-combobox
                label="Authority"
                v-model="request.authority"
                :items="identityServers"
                clearable
              ></v-combobox>
            </v-col>
          </v-row>
          <v-row dense>
            <v-col md="12">
              <v-text-field
                label="ClientId"
                v-model="request.clientId"
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row dense>
            <v-col md="12">
              <v-text-field
                label="Secret (optional)"
                v-model="request.secret"
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row dense>
            <v-col md="12">
              <v-combobox
                label="Scopes"
                v-model="request.scopes"
                chips
                multiple
                clearable
                deletable-chips
                :items="request.scopes"
              ></v-combobox>
            </v-col>
          </v-row>
          <v-row dense>
            <v-col md="6">
              <v-combobox
                label="Response Type"
                v-model="request.responseTypes"
                chips
                multiple
                clearable
                deletable-chips
                :items="responseTypes"
              ></v-combobox>
            </v-col>
            <v-col md="2">
              <v-switch label="Pkce" v-model="request.usePkce"></v-switch>
            </v-col>
            <v-col md="2">
              <v-switch
                label="Save Tokens"
                v-model="request.saveTokens"
              ></v-switch>
            </v-col>
            <v-col md="2">
              <v-text-field
                type="number"
                label="Port (optional)"
                v-model.number="request.port"
              ></v-text-field>
            </v-col>
          </v-row>
        </v-tab-item>
        <v-tab-item>
          <saved-requests-list
            :type="save.type"
            @select="onSelectRequest"
            ref="requestList"
          ></saved-requests-list>
        </v-tab-item>
      </v-tabs-items>
    </v-card-text>
    <v-card-actions v-if="tab == 0">
      <save-identity-request-menu
        :data="request"
        :request="save"
        @saved="onRequestSaved"
      ></save-identity-request-menu>
      <v-spacer></v-spacer>
      <v-btn color="primary" @click="onClickAuthorize">Authorize</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import SaveIdentityRequestMenu from "./SaveIdentityRequestMenu.vue";
import SavedRequestsList from "./SavedRequestsList.vue";

import { startAuthorize } from "../../tokenService";
export default {
  components: { SaveIdentityRequestMenu, SavedRequestsList },
  watch: {
    save: {
      handler: function () {
        this.request.requestId = this.save.id;
      },
      deep: true,
    },
  },
  data() {
    return {
      request: {
        authority: null,
        clientId: null,
        responseTypes: ["code"],
        secret: null,
        usePkce: true,
        scopes: ["openid", "profile"],
        port: null,
        saveTokens: false,
        requestId: null,
      },
      save: {
        id: null,
        name: "",
        tags: [],
        type: "AUTHORIZE",
      },
      tab: null,
    };
  },
  computed: {
    identityServers: function () {
      return this.$store.state.app.userSettings.tokenGenerator.identityServers;
    },
    responseTypes: function () {
      return ["code", "id_token", "token"];
    },
  },
  methods: {
    async onClickAuthorize() {
      const result = await startAuthorize(this.request);
      this.$emit("started", result.data.startAuthorizationRequest.server);
      this.request.port = null;
    },
    onSelectRequest: function (request) {
      this.tab = 0;
      this.save.id = request.id;
      this.save.name = request.name;
      this.save.tags = request.tags;
      this.request.authority = request.data.authority;
      this.request.clientId = request.data.clientId;
      this.request.responseTypes = request.data.responseTypes;
      this.request.secret = request.data.secret;
      this.request.scopes = request.data.scopes;
      this.request.port = request.data.port;
      this.request.usePkce = request.data.usePkce;
      this.request.saveTokens = request.data.saveTokens;
    },
    onRequestSaved: function () {
      if (this.$refs.requestList) {
        this.$refs.requestList.search();
      }
    },
  },
};
</script>

<style></style>
