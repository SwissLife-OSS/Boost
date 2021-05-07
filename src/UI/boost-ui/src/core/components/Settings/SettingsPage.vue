<template>
  <sub-navigation-bar navId="Settings">
    <template>
      <v-spacer></v-spacer>
      <v-tooltip bottom v-if="userSettings">
        <template v-slot:activator="{ on, attrs }">
          <v-icon
            @click="onClickLocation"
            v-bind="attrs"
            v-on="on"
            color="white"
            >mdi-folder</v-icon
          >
        </template>
        <span>{{ userSettings.location }}</span>
      </v-tooltip>
    </template>
  </sub-navigation-bar>
</template>

<script>
import { mapState } from "vuex";
import SubNavigationBar from "../Common/SubNavigationBar.vue";
import workspaceMixins from "../workspaceMixins";
export default {
  components: { SubNavigationBar },
  mixins: [workspaceMixins],
  computed: {
    ...mapState("app", ["userSettings"]),
  },
  methods: {
    onClickLocation: function () {
      this.openDirectoryInCode(this.userSettings.location);
    },
  },
};
</script>
