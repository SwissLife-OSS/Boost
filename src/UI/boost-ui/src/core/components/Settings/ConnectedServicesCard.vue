<template>
  <v-card elevation="1">
    <v-toolbar color="grey lighten-2" elevation="0" height="36">
      <v-toolbar-title> Connected Services </v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text>
      <v-list>
        <v-list-item
          @click="onClickService(service)"
          v-for="(service, i) in services"
          :key="i"
        >
          <v-list-item-avatar>
            <v-img :src="service.image"></v-img>
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title>{{ service.name }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
      <v-toolbar elevation="0">
        <v-spacer></v-spacer>
        <v-btn @click="showAdd = !showAdd"
          ><v-icon left>mdi-plus</v-icon>Add Service</v-btn
        >
      </v-toolbar>

      <v-list v-if="showAdd">
        <v-list-item
          @click="onClickServiceType(service)"
          v-for="(service, i) in connectedServiceTypes"
          :key="i"
        >
          <v-list-item-avatar>
            <v-img :src="service.image"></v-img>
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title>{{ service.name }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-card-text>
  </v-card>
</template>

<script>
import { getConnectedServices } from "../../settingsService";
export default {
  created() {
    this.loadConnectedServices();
  },
  data() {
    return {
      connectedServiceTypes: [],
      services: [],
      showAdd: false,
    };
  },
  methods: {
    async loadConnectedServices() {
      const result = await getConnectedServices();

      this.services = result.data.connectedServices.map((x) => {
        x.image = require(`../../../assets/cs/${x.type}.png`);

        return x;
      });

      this.connectedServiceTypes = result.data.connectedServiceTypes.map(
        (x) => {
          x.image = require(`../../../assets/cs/${x.name}.png`);
          x.route = `Settings.${x.name}`;
          return x;
        }
      );
    },
    onClickService: function (service) {
      this.$router.push({
        name: `Settings.${service.type}.Edit`,
        params: { id: service.id },
      });
    },
    onClickServiceType: function (service) {
      this.$router.push({ name: service.route });
    },
  },
};
</script>

<style>
</style>