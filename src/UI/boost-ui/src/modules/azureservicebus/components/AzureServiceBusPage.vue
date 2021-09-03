<template>
  <v-row class="ma-4">
    <v-col md="6"><connections-list-card></connections-list-card></v-col>
  </v-row>
</template>

<script>
import ConnectionsListCard from "./ConnectionsListCard.vue";
export default {
  components: { ConnectionsListCard }
};
</script>

<style></style>
