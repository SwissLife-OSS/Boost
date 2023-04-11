<template>
  <v-row>
    <v-col md="7"
      ><authorize-request-card
        @started="onServerStarted"
      ></authorize-request-card
    ></v-col>
    <v-col md="5"
      ><auth-servers-list :servers="servers" @stop="onStop" :loading="loading"></auth-servers-list
    ></v-col>
  </v-row>
</template>

<script>
import { stopAuthServer, getRunningAuthServers } from "../../tokenService";
import AuthorizeRequestCard from "./AuthorizeRequestCard.vue";
import AuthServersList from "./AuthServersList.vue";
export default {
  components: { AuthorizeRequestCard, AuthServersList },
  created() {
    this.loadRunningServers();
  },
  data() {
    return {
      loading: false,
      servers: [],
    };
  },
  methods: {
    onServerStarted: function (server) {
      this.servers.push(server);
    },
    async onStop(server) {
      await stopAuthServer(server.id);

      const index = this.servers.indexOf((x) => x.id == server.id);
      this.servers.splice(index, 1);
    },
    async loadRunningServers() {
      this.loading = true;
      const result = await getRunningAuthServers();

      this.servers = result.data.runningAuthServers;
      this.loading = false;
    },
  },
};
</script>

<style>
</style>
