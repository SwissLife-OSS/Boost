<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title> UserInfo </v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text>
      <div class="mt-2">
        <v-alert type="error" outlined v-if="userInfo.error">
          {{ userInfo.error }}
        </v-alert>
        <div v-else>
          <v-row dense v-for="(claim, i) in userInfo.claims" :key="i">
            <v-col md="4">{{ claim.type }}</v-col>
            <v-col md="8">
              <strong>{{ claim.value }}</strong></v-col
            >
          </v-row>
        </div>
      </div>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  props: ["userInfo"],
};
</script>

<style>
</style>