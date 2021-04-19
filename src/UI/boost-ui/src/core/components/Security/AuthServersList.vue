<template>
  <div>
    <h4>Running auth servers</h4>
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
          <v-list-item-avatar size="40" color="green darken-2">
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
  data() {
    return {};
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