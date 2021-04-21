<template>
  <v-row class="ma-4">
    <v-col md="6">
      <create-hash-card></create-hash-card>
    </v-col>
    <v-col md="6"> </v-col>
  </v-row>
</template>

<script>
import CreateHashCard from "./CreateHashCard.vue";
export default {
  components: { CreateHashCard },
};
</script>

<style>
</style>
CreateHashCard