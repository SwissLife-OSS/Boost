<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title> {{ title }} </v-toolbar-title>
      <v-spacer></v-spacer>
      <template v-slot:extension>
        <v-tabs background-color="grey lighten-3" height="44" v-model="tab">
          <v-tab>Token</v-tab>
          <v-tab>Protocol</v-tab>
          <v-tab>Data</v-tab>
        </v-tabs>
      </template>
    </v-toolbar>
    <v-card-text
      :style="{
        'max-height': height + 'px',
        height: height + 'px',
      }"
      class="overflow-y-auto mt-0"
    >
      <v-row dense>
        <v-col md="12">
          <v-tabs-items v-model="tab">
            <v-tab-item>
              <v-textarea outlined :value="token.token" rows="10"></v-textarea>
            </v-tab-item>
            <v-tab-item>
              <div>
                <v-alert v-if="token.expiresIn < 0" type="warning" outlined>
                  This token has expired! ({{ Math.abs(token.expiresIn) }}
                  minutes ago).
                </v-alert>
                <v-row dense>
                  <v-col md="4">Valid From</v-col>
                  <v-col md="8">
                    <strong>{{ token.validFrom | dateformat }}</strong></v-col
                  >
                </v-row>
                <v-row dense>
                  <v-col md="4">Valid Until</v-col>
                  <v-col md="8">
                    <strong>{{ token.validTo | dateformat }}</strong></v-col
                  >
                </v-row>
                <v-row dense v-for="(claim, i) in protocolClaims" :key="i">
                  <v-col md="4">{{ claim.type }}</v-col>
                  <v-col md="8">
                    <strong>{{ claim.value }}</strong></v-col
                  >
                </v-row>
              </div>
            </v-tab-item>
            <v-tab-item>
              <v-row dense v-for="(claim, i) in payloadClaims" :key="i">
                <v-col md="4">{{ claim.type }}</v-col>
                <v-col md="8">
                  <strong>{{ claim.value }}</strong></v-col
                >
              </v-row>
            </v-tab-item>
          </v-tabs-items>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  props: ["token", "title", "height"],
  data() {
    return {
      tab: null,
    };
  },
  computed: {
    protocolClaims: function () {
      if (this.token) {
        return this.token.claims.filter((x) => x.category === "PROTOCOL");
      }

      return [];
    },
    payloadClaims: function () {
      if (this.token) {
        return this.token.claims.filter((x) => x.category === "PAYLOAD");
      }

      return [];
    },
  },
};
</script>

<style>
</style>