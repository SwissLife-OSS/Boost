<template>
  <v-row>
    <v-col md="6">
      <token-request-card
        @request-start="onStart"
        @completed="onTokenRequested"
      ></token-request-card>
    </v-col>
    <v-col md="6"
      ><token-details-card
        v-if="result && result.accessToken"
        :token="result.accessToken"
        :height="500"
        title="Access Token"
      ></token-details-card>
      <v-alert
        v-if="result && result.isSuccess === false"
        type="error"
        outlined
      >
        Error requesting token!<br /><br /><strong>{{
          result.errorMessage
        }}</strong>
      </v-alert>
    </v-col>
  </v-row>
</template>

<script>
import TokenDetailsCard from "./TokenDetailsCard.vue";
import TokenRequestCard from "./TokenRequestCard.vue";

export default {
  components: { TokenRequestCard, TokenDetailsCard },
  data() {
    return {
      result: null,
    };
  },
  methods: {
    onTokenRequested: function (result) {
      this.result = result;
    },
    onStart: function () {
      this.result = null;
    },
  },
};
</script>

<style>
</style>