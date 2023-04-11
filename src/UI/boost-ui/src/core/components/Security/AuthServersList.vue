<template>
  <div>
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title>Servers</v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-list
      two-line
      dense
      :style="{ 'max-height': $vuetify.breakpoint.height - 180 + 'px' }"
      class="overflow-y-auto mt-0"
    >
      <v-list-item-group color="primary" select-object>
        <v-list-item
          v-for="server in servers"
          :key="server.id"
          selectable
          @click="onSelectServer(server)"
        >
          <v-list-item-avatar size="40" color="grey darken-2">
            <v-icon dark>mdi-server-security</v-icon>
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title v-text="server.url"></v-list-item-title>
            <v-list-item-subtitle v-text="server.title"></v-list-item-subtitle>
          </v-list-item-content>
          <v-list-item-action>
            <v-btn icon>
              <v-icon color="red darken-1" @click.stop="onStop(server)"
                >mdi-stop</v-icon
              >
            </v-btn>
          </v-list-item-action>
        </v-list-item>
      </v-list-item-group>
    </v-list>

    <v-alert type="info" v-if="lastCreated" outlined>
      New webserver started on <strong>{{ lastCreated.url }}</strong
      >.<br />
      Click on server to open in new tab.
    </v-alert>
    <p></p>
  </div>
</template>

<script>
export default {
  props: {
    servers: {
      type: Array,
      default: () => [],
    },
  },
  watch: {
    servers: function (newValue) {
      if (newValue.length > 0) {
        this.lastCreated = newValue[newValue.length - 1];

        window.setTimeout(() => {
          this.lastCreated = null;
        }, 8000);
      }
    },
  },
  data() {
    return {
      lastCreated: null,
    };
  },
  methods: {
    onSelectServer: function (server) {
      window.open(server.url);
    },
    onStop: function (server) {
      this.$emit("stop", server);
    },
  },
};
</script>

<style>
</style>
