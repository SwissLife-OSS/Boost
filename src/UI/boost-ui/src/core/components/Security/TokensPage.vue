<template>
  <v-row>
    <v-col md="6"
      ><token-analyzer-card ref="analyzer"></token-analyzer-card>
    </v-col>
    <v-col md="6"
      ><stored-tokens-card @select="onTokenSelected"></stored-tokens-card>
    </v-col>
  </v-row>
</template>

<script>
import StoredTokensCard from "./StoredTokensCard.vue";
import TokenAnalyzerCard from "./TokenAnalyzerCard.vue";
export default {
  components: { TokenAnalyzerCard, StoredTokensCard },

  methods: {
    onTokenSelected: function (token) {
      this.$refs.analyzer.analyze(token);
    },
  },
};
</script>

<style>
</style>