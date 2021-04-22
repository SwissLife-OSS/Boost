<template>
  <v-row class="ma-4">
    <v-col md="6"><work-roots-card></work-roots-card></v-col>
    <v-col md="6"><connected-services-card></connected-services-card></v-col>
  </v-row>
</template>

<script>
import ConnectedServicesCard from "./ConnectedServicesCard.vue";
import WorkRootsCard from "./WorkRootsCard.vue";
export default {
  components: { WorkRootsCard, ConnectedServicesCard },
};
</script>

<style>
</style>