<template>
  <v-row class="ma-4">
    <v-col md="6"
      ><token-requestor-settings-card></token-requestor-settings-card
    ></v-col>
    <v-col md="6"></v-col>
  </v-row>
</template>

<script>
import TokenRequestorSettingsCard from "./TokenRequestorSettingsCard.vue";
export default {
  components: { TokenRequestorSettingsCard },
};
</script>

<style>
</style>