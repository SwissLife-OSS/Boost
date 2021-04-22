<template>
  <v-card elevation="1" :loading="loading">
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
              <v-combobox
                label="Authority"
                v-model="request.authority"
                :items="identityServers"
                clearable
              ></v-combobox>
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
            <v-col md="11">
              <v-select
                label="Grant Type"
                v-model="request.grantType"
                item-text="name"
                item-value="name"
                :items="grantTypes"
              ></v-select>
            </v-col>
            <v-col md="1">
              <v-icon
                class="mt-4"
                @click="$router.push({ name: 'Settings.Security' })"
                title="Configure custom grants"
                >mdi-cog</v-icon
              >
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

          <v-row dense v-for="field in parameters" :key="field.name">
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
        :parameters="parameters"
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
      tab: null,
      loading: false,
    };
  },
  computed: {
    identityServers: function () {
      return this.$store.state.app.userSettings.tokenGenerator.identityServers;
    },
    grantTypes: function () {
      const grantTypes = [
        {
          name: "client_credentials",
          parameters: [],
        },
      ];

      return grantTypes.concat(
        this.$store.state.app.userSettings.tokenGenerator.customGrants
      );
    },
    parameters: function () {
      var gt = this.grantTypes.find((x) => x.name === this.request.grantType);
      if (gt.parameters) {
        return gt.parameters;
      } else {
        return [];
      }
    },
  },
  methods: {
    async onRequest() {
      this.$emit("request-start");

      var input = {
        authority: this.request.authority,
        clientId: this.request.clientId,
        secret: this.request.secret,
        scopes: this.request.scopes,
        grantType: this.request.grantType,
        parameters: this.parameters.map((x) => {
          return {
            name: x.name,
            value: x.value,
          };
        }),
      };

      this.loading = true;
      this.$emit("request-start");

      const result = await requestToken(input);
      this.loading = false;

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
      this.request.grantType = request.data.grantType;

      for (let i = 0; i < this.parameters.length; i++) {
        const param = this.parameters[i];
        let value = request.data.parameters.find((x) => x.name === param.name);

        if (value) {
          param.value = value.value;
        }
      }
    },
    onRequestSaved: function () {
      if (this.$refs.requestList) {
        this.$refs.requestList.search();
      }
    },
  },
};
</script>

<style>
</style>