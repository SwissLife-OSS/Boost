<template>
  <v-list
    two-line
    dense
    :style="{ 'max-height': height + 'px' }"
    class="overflow-y-auto mt-0"
  >
    <v-list-item-group color="primary" select-object>
      <v-list-item
        v-for="(action, i) in actions"
        :key="i"
        selectable
        @click="onClickAction(action)"
      >
        <v-list-item-avatar size="24">
          <v-img :src="actionImage(action)"></v-img>
        </v-list-item-avatar>

        <v-list-item-content>
          <v-list-item-title v-text="action.title"></v-list-item-title>
          <v-list-item-subtitle
            v-text="action.description"
          ></v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>
    </v-list-item-group>
  </v-list>
</template>

<script>
import workspaceMixins from "../workspaceMixins";

export default {
  mixins: [workspaceMixins],
  props: {
    actions: {
      type: Array,
      default: () => [],
    },
    height: {
      type: Number,
      default: 200,
    },
  },
  methods: {
    onClickAction(action) {
      switch (action.type) {
        case "VS_SOLUTION":
          this.openFile(action.value);
          break;
        case "CODE_DIRECTORY":
          this.openDirectoryInCode(action.value);
          break;
        case "EXPLORER_DIRECTORY":
          this.openInExplorer(action.value);
          break;
        case "TERMINAL_DIRECTORY":
          this.openInTerminal(action.value);
          break;
        case "SUPERBOOST":
          this.runSuperBoost({ name: action.title, directory: action.value });
          break;
      }
    },
    actionImage(action) {
      switch (action.type) {
        case "VS_SOLUTION":
          return require("../../../assets/vs_icon.png");
        case "CODE_DIRECTORY":
          if (action.tags.includes("js")) {
            return require("../../../assets/js_icon.png");
          }
          return require("../../../assets/vscode_icon.png");
        case "EXPLORER_DIRECTORY":
          return require("../../../assets/directory_icon.png");
        case "TERMINAL_DIRECTORY":
          return require("../../../assets/terminal_icon.png");
        case "SUPERBOOST":
          return require("../../../assets/superboost.png");
      }
    },
  },
};
</script>

<style>
</style>