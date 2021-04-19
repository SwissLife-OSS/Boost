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
      <v-btn text @click="onSave">Save request</v-btn>

      <v-spacer></v-spacer>
      <v-btn color="primary" @click="onRequest">Request token</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import { requestToken, saveRequest } from "../../tokenService";
export default {
  data() {
    return {
      request: {
        authority: "https://demo.identityserver.io",
        clientId: "m2m",
        secret: "secret",
        scopes: [],
        grantType: "client_credentials",
      },
      grantTypes: ["client_credentials"],
    };
  },
  methods: {
    async onRequest() {
      const result = await requestToken(this.request);

      this.$emit("completed", result.data.requestToken.result);
    },
    async onSave() {
      let input = Object.assign({}, this.request);
      input.name = "Test 1";
      input.type = "TOKEN";

      const result = await saveRequest(input);

      console.log(result);
    },
  },
};
</script>

<style>
</style>