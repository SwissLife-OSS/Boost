<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title>Token Request</v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text>
      <v-row dense>
        <v-col md="12">
          <v-text-field
            label="Authority"
            v-model="request.authority"
          ></v-text-field>
        </v-col>
      </v-row>
      <v-row dense>
        <v-col md="6">
          <v-text-field
            label="ClientId"
            v-model="request.clientId"
          ></v-text-field>
        </v-col>
        <v-col md="6">
          <v-text-field label="Secret" v-model="request.secret"></v-text-field>
        </v-col>
      </v-row>
      <v-row dense> </v-row>
      <v-row dense>
        <v-col md="12">
          <v-select
            label="Grant Type"
            v-model="request.grantType"
            :items="grantTypes"
          ></v-select>
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
      <v-row dense> </v-row>
    </v-card-text>
    <v-card-actions>
      <save-identity-request-menu
        :data="request"
        :request="save"
      ></save-identity-request-menu>
      <v-spacer></v-spacer>
      <v-btn color="primary" @click="onRequest">Request token</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import { requestToken } from "../../tokenService";
import SaveIdentityRequestMenu from "./SaveIdentityRequestMenu.vue";
export default {
  components: { SaveIdentityRequestMenu },
  data() {
    return {
      request: {
        authority: "https://demo.identityserver.io",
        clientId: "m2m",
        secret: "secret",
        scopes: [],
        grantType: "client_credentials",
      },
      save: {
        id: null,
        name: "",
        tags: [],
        type: "TOKEN",
      },
      grantTypes: ["client_credentials"],
    };
  },
  methods: {
    async onRequest() {
      const result = await requestToken(this.request);

      this.$emit("completed", result.data.requestToken.result);
    },
  },
};
</script>

<style>
</style>