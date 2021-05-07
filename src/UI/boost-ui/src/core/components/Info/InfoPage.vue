<template>
  <v-row class="ma-2">
    <v-col md="6"> <version-info-card></version-info-card></v-col>
  </v-row>
</template>

<script>
import VersionInfoCard from "./VersionInfoCard.vue";
export default {
  components: { VersionInfoCard },
};
</script>

<style>
</style>