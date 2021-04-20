<template>
  <loader-center v-if="loading"></loader-center>
  <div v-else>
    <div v-if="session.isAuthenticated" class="ma-2">
      <v-row>
        <v-col md="6"
          ><token-card
            :token="session.accessToken"
            title="Access Token"
            :height="300"
          ></token-card
        ></v-col>
        <v-col md="6"
          ><token-card
            :token="session.idToken"
            title="ID Token"
            :height="300"
          ></token-card>
        </v-col>
      </v-row>
      <v-row>
        <v-col md="6"
          ><user-info-card :userInfo="session.userInfo"></user-info-card>
        </v-col>
        <v-col md="6"
          ><refresh-token-card
            v-if="session.refreshToken"
            :token="session.refreshToken"
          ></refresh-token-card
        ></v-col>
      </v-row>
    </div>
    <v-container v-else>
      <v-row class="mt-8">
        <v-col md="2"></v-col>
        <v-col md="8">
          <v-alert type="error" outlined>
            User is not authenticated.
          </v-alert></v-col
        >
        <v-col md="2"></v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script>
import { getAuthSession } from "../authService";
import LoaderCenter from "./Common/LoaderCenter.vue";
import RefreshTokenCard from "./RefreshTokenCard.vue";
import TokenCard from "./TokenCard.vue";
import UserInfoCard from "./UserInfoCard.vue";

export default {
  components: { TokenCard, UserInfoCard, RefreshTokenCard, LoaderCenter },
  created() {
    this.getSession();
  },
  data() {
    return {
      loading: false,
      session: {},
    };
  },
  methods: {
    async getSession() {
      this.loading = true;
      const result = await getAuthSession();
      this.session = result.data.authenticationSession;
      this.loading = false;
    },
  },
};
</script>

<style>
</style>