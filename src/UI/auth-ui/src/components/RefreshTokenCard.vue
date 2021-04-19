<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title> Refresh Token </v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text>
      <v-textarea outlined :value="token" rows="2"></v-textarea>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  props: ["token"],
};
</script>

<style>
</style>