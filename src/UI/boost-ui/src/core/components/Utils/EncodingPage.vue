<template>
  <v-row class="ma-4">
    <v-col md="6">
      <encoding-card></encoding-card>
    </v-col>
    <v-col md="6">
      <guid-card></guid-card>
    </v-col>
  </v-row>
</template>

<script>
import EncodingCard from "./EncodingCard.vue";
import GuidCard from "./GuidCard.vue";

export default {
  components: { EncodingCard, GuidCard },

  data() {
    return {};
  },
};
</script>
