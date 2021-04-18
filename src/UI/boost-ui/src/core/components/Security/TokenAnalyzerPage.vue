<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title> Analyze JWT Token </v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text
      :style="{
        'max-height': cardHeight + 'px',
        height: cardHeight + 'px',
      }"
      class="overflow-y-auto mt-0"
    >
      <v-row dense>
        <v-col md="12">
          <v-tabs v-model="tab">
            <v-tab>Raw</v-tab>
            <v-tab v-if="analyzed">Protocol</v-tab>
            <v-tab v-if="analyzed">Data</v-tab>
            <v-tab v-if="analyzed">Actions</v-tab>

            <v-tab-item class="mt-4">
              <v-textarea
                outlined
                label="Token"
                v-model="token"
                rows="7"
              ></v-textarea>
              <v-alert type="error" outlined v-if="errorMessage">
                {{ errorMessage }}
              </v-alert>
            </v-tab-item>
            <v-tab-item class="mt-4">
              <div v-if="analyzed">
                <v-alert v-if="analyzed.expiresIn > 0" type="success" outlined>
                  This token is valid for the next
                  {{ analyzed.expiresIn }} minutes.
                </v-alert>
                <v-alert v-else type="warning" outlined>
                  This token has expired! ({{ Math.abs(analyzed.expiresIn) }}
                  minutes ago).
                </v-alert>
                <v-row dense>
                  <v-col md="4">Valid From</v-col>
                  <v-col md="8">
                    <strong>{{
                      analyzed.validFrom | dateformat
                    }}</strong></v-col
                  >
                </v-row>
                <v-row dense>
                  <v-col md="4">Valid Until</v-col>
                  <v-col md="8">
                    <strong>{{ analyzed.validTo | dateformat }}</strong></v-col
                  >
                </v-row>
                <v-row dense v-for="(claim, i) in protocolClaims" :key="i">
                  <v-col md="4">{{ claim.type }}</v-col>
                  <v-col md="8">
                    <strong>{{ claim.value }}</strong></v-col
                  >
                </v-row>
              </div>
            </v-tab-item>
            <v-tab-item class="mt-4">
              <v-row dense v-for="(claim, i) in payloadClaims" :key="i">
                <v-col md="4">{{ claim.type }}</v-col>
                <v-col md="8">
                  <strong>{{ claim.value }}</strong></v-col
                >
              </v-row>
            </v-tab-item>
            <v-tab-item class="mt-4">
              <v-btn
                v-if="analyzed && analyzed.subject"
                @click="requestUserInfo"
                class="ma-2"
                >Request UserInfo</v-btn
              >
              <div v-if="userInfo" class="mt-2">
                <v-alert type="error" outlined v-if="userInfo.error">
                  {{ userInfo.error }}
                </v-alert>
                <v-card elevation="0" v-else>
                  <v-row dense v-for="(claim, i) in userInfo.claims" :key="i">
                    <v-col md="4">{{ claim.type }}</v-col>
                    <v-col md="8">
                      <strong>{{ claim.value }}</strong></v-col
                    >
                  </v-row>
                </v-card>
              </div>
            </v-tab-item>
          </v-tabs>
        </v-col>
      </v-row>
      <v-row dense>
        <v-col md="12"> </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script>
import { analyzeToken, getUserInfo } from "../../tokenService";
export default {
  data() {
    return {
      tab: null,
      token: "",
      userInfo: null,
      analyzed: null,
      errorMessage: null,
    };
  },
  watch: {
    token: function () {
      this.analyzeToken();
    },
  },
  computed: {
    cardHeight: function () {
      return this.$vuetify.breakpoint.height - 150;
    },
    protocolClaims: function () {
      if (this.analyzed) {
        return this.analyzed.claims.filter((x) => x.category === "PROTOCOL");
      }

      return [];
    },
    payloadClaims: function () {
      if (this.analyzed) {
        return this.analyzed.claims.filter((x) => x.category === "PAYLOAD");
      }

      return [];
    },
  },
  methods: {
    async analyzeToken() {
      if (this.token.length < 20) {
        this.analyzed = null;
        this.errorMessage = null;
        return;
      }
      this.errorMessage = null;
      const result = await analyzeToken(this.token);
      if (result.errors) {
        this.errorMessage = result.errors[0].extensions.message;
        this.analyzed = null;
      } else {
        this.analyzed = result.data.analyzeToken;

        this.tab = 1;
      }
    },
    async requestUserInfo() {
      const result = await getUserInfo(this.token);
      this.userInfo = result.data.userInfoClaims;
    },
  },
};
</script>

<style>
</style>