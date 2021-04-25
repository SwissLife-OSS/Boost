<template>
  <v-card elevation="1" :loading="loading">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title> Stored Tokens</v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text>
      <v-row dense>
        <v-col>
          <v-list
            two-line
            dense
            :style="{
              'max-height': $vuetify.breakpoint.height - 180 + 'px',
              height: $vuetify.breakpoint.height - 180 + 'px',
            }"
            class="overflow-y-auto mt-0"
          >
            <v-list-item-group color="primary" select-object>
              <v-list-item
                v-for="token in tokens"
                :key="token.id"
                selectable
                @click="onSelectToken(token)"
              >
                <v-list-item-avatar size="40" color="purple darken-3">
                  <v-icon dark>mdi-key</v-icon>
                </v-list-item-avatar>
                <v-list-item-content>
                  <v-row dense>
                    <v-col md="8"
                      ><strong>{{ token.name }}</strong></v-col
                    >
                    <v-col md="4"
                      ><small>{{ token.createdAt | dateformat }}</small></v-col
                    >
                  </v-row>

                  <v-row dense>
                    <v-col md="3"
                      >Access
                      <small>({{ token.accessTokenExpiresIn }}s)</small>
                      <v-icon
                        v-if="token.hasAccessToken"
                        :color="getTokenColor(token)"
                        >mdi-check</v-icon
                      >
                      <v-icon v-else color="grey">mdi-minus</v-icon>
                    </v-col>

                    <v-col md="3" @click.stop="onClickIdToken(token)"
                      >ID
                      <v-icon v-if="token.hasIdToken" color="blue"
                        >mdi-check</v-icon
                      >
                      <v-icon v-else color="grey">mdi-minus</v-icon></v-col
                    >
                    <v-col md="3"
                      >Refresh
                      <v-icon v-if="token.hasRefreshToken" color="blue"
                        >mdi-check</v-icon
                      >
                      <v-icon v-else color="grey">mdi-minus</v-icon></v-col
                    >
                  </v-row>
                </v-list-item-content>
                <v-list-item-action>
                  <v-btn icon>
                    <v-icon color="grey lighen-1" @click.stop="onDelete(token)"
                      >mdi-delete-variant</v-icon
                    >
                  </v-btn>
                  <v-btn icon v-if="token.hasRefreshToken">
                    <v-icon color="grey lighen-1" @click.stop="onRefresh(token)"
                      >mdi-refresh</v-icon
                    >
                  </v-btn>
                </v-list-item-action>
              </v-list-item>
            </v-list-item-group>
          </v-list>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script>
import {
  deleteToken,
  getStoredToken,
  listStoredTokens,
  refreshToken,
} from "../../tokenService";
export default {
  created() {
    this.loadTokens();
  },
  data() {
    return {
      loading: false,
      tokens: [],
    };
  },
  methods: {
    getTokenColor: function (token) {
      if (token.accessTokenExpiresIn > 0) {
        return "green";
      }
      return "red";
    },
    async loadTokens() {
      this.loading = true;
      const result = await listStoredTokens();

      this.tokens = result.data.storedTokens;
      this.loading = false;
    },
    async getToken(id, type, refresh) {
      const result = await getStoredToken({
        id,
        type,
        autoRefresh: refresh,
      });

      return result.data.storedToken;
    },
    async refreshToken(token) {
      this.loading = true;
      const result = await refreshToken(token.id);
      var index = this.tokens.findIndex((x) => x.id == token.id);
      this.tokens[index] = result.data.refreshStoredToken.header;
      this.loading = false;
    },
    async delete(token) {
      this.loading = true;
      await deleteToken(token.id);
      var index = this.tokens.findIndex((x) => x.id == token.id);

      this.tokens.splice(index, 1);

      this.loading = false;
    },
    onDelete: function (token) {
      this.delete(token);
    },
    onRefresh: function (token) {
      this.refreshToken(token);
    },
    onClickIdToken: function (token) {
      this.selectToken(token, "ID");
    },
    onSelectToken: function (token) {
      this.selectToken(token, "ACCESS");
    },
    async selectToken(token, type) {
      const tokenValue = await this.getToken(token.id, type, false);

      if (tokenValue) {
        this.$emit("select", tokenValue);
      }
    },
  },
};
</script>

<style>
</style>