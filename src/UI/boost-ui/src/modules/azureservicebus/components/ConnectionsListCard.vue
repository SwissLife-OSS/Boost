<template>
  <v-card elevation="1">
    <v-toolbar color="grey lighten-2" elevation="0" height="36">
      <v-toolbar-title> Azure Service Bus Connections list </v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text>
      <v-list>
        <v-list-item v-for="(connection, i) in connections" :key="i">
          <v-list-item-content>
            <v-list-item-title @click="onClickConnection(connection)">
              <v-btn>
                {{ connection.name }}
              </v-btn>
            </v-list-item-title>
          </v-list-item-content>
          <v-list-item-action>
            <v-btn icon>
              <v-icon class="mt-5" @click="onClickDeleteConnection(connection)"
                >mdi-delete-outline</v-icon
              >
            </v-btn>
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-toolbar elevation="0">
        <v-spacer></v-spacer>
        <v-btn @click="onClickAddConnection()"
          ><v-icon left>mdi-plus</v-icon>Add connection</v-btn
        >
      </v-toolbar>
    </v-card-text>
  </v-card>
</template>

<script>
import { getSettings, deleteConnection } from "../azureServiceBusService";
export default {
  created() {
    this.loadSettings();
  },
  data() {
    return {
      connections: [],
      showAdd: false
    };
  },
  methods: {
    async loadSettings() {
      const result = await getSettings();

      this.connections = result.data.azureServiceBusSettings.connections;
    },
    onClickConnection: function(connection) {
      this.$router.push({
        name: `AzureServiceBus.View`,
        params: {
          connectionName: connection.name
        }
      });
    },
    onClickAddConnection: function() {
      this.$router.push({
        name: `AzureServiceBus.Add`
      });
    },
    onClickDeleteConnection: function(connection) {
      deleteConnection(connection.name);
      this.loadSettings();
    }
  }
};
</script>

<style></style>
