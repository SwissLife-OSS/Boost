<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title>Authorize</v-toolbar-title>
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
        <v-col md="12">
          <v-text-field
            label="ClientId"
            v-model="request.clientId"
          ></v-text-field>
        </v-col>
      </v-row>
      <v-row dense>
        <v-col md="12">
          <v-text-field label="Secret" v-model="request.secret"></v-text-field>
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
        <v-col md="4">
          <v-text-field
            type="number"
            label="Port"
            v-model.number="request.port"
          ></v-text-field>
        </v-col>
        <v-col md="4">
          <v-switch label="Pkce" v-model="request.pkce"></v-switch>
        </v-col>
      </v-row>
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn color="primary" @click="onClickAuthorize">Authorize</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import { startAuthorize } from "../../tokenService";
export default {
  data() {
    return {
      request: {
        authority: "https://demo.identityserver.io",
        clientId: "interactive.confidential",
        secret: "secret",
        pkce: true,
        scopes: ["openid", "profile"],
        port: 3010,
      },
    };
  },
  methods: {
    async onClickAuthorize() {
      const result = await startAuthorize(this.request);

      console.log(result);

      this.$emit("started", result.data.startAuthorizationRequest.server);
    },
  },
};
</script>

<style>
</style>