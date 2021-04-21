<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title>Token Request </v-toolbar-title>
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
              <v-text-field
                label="Secret"
                v-model="request.secret"
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row dense> </v-row>
          <v-row dense>
            <v-col md="12">
              <v-select
                label="Grant Type"
                v-model="request.grantType"
                item-text="name"
                item-value="name"
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

          <v-row dense v-for="field in extraFields" :key="field.name">
            <v-col md="12">
              <v-text-field
                :label="field.label"
                v-model="field.value"
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
      <v-btn color="primary" @click="onRequest">Request token</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import { requestToken } from "../../tokenService";
import SavedRequestsList from "./SavedRequestsList.vue";
import SaveIdentityRequestMenu from "./SaveIdentityRequestMenu.vue";
export default {
  components: { SaveIdentityRequestMenu, SavedRequestsList },
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
      grantTypes: [
        {
          name: "client_credentials",
        },
        {
          name: "personal_access_token",
          fields: [
            { name: "token", label: "Token", value: null },
            { name: "username", label: "Username", value: null },
          ],
        },
      ],
      tab: null,
    };
  },
  computed: {
    extraFields: function () {
      var gt = this.grantTypes.find((x) => x.name === this.request.grantType);
      if (gt.fields) {
        return gt.fields;
      } else {
        return [];
      }
    },
  },
  methods: {
    async onRequest() {
      var input = {
        authority: this.request.authority,
        clientId: this.request.clientId,
        secret: this.request.secret,
        scopes: this.request.scopes,
        grantType: this.request.grantType,
        parameters: this.extraFields.map((x) => {
          return {
            name: x.name,
            value: x.value,
          };
        }),
      };

      const result = await requestToken(input);

      this.$emit("completed", result.data.requestToken.result);
    },
    onSelectRequest: function (request) {
      this.tab = 0;
      this.save.id = request.id;
      this.save.name = request.name;

      this.request.authority = request.data.authority;
      this.request.clientId = request.data.clientId;
      this.request.secret = request.data.secret;
      this.request.scopes = request.data.scopes;
    },
    onRequestSaved: function () {
      this.$refs.requestList.search();
    },
  },
};
</script>

<style>
</style>