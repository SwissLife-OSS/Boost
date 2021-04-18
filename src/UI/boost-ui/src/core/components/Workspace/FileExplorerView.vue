<template>
  <div>
    <v-treeview
      :style="{
        height: height + 'px',
      }"
      class="overflow-y-auto mt-0"
      :return-object="true"
      :active.sync="active"
      :open-on-click="true"
      :items="files"
      :search="searchText"
      activatable
      item-key="path"
      :load-children="getChildren"
      ref="tree"
      :multiple-active="false"
      :open.sync="open"
      @update:active="onSelect"
      dense
      transition
    >
      <template v-slot:prepend="{ open, item }">
        <v-icon
          size="20"
          color="yellow darken-2"
          v-if="item.type === 'DIRECTORY'"
        >
          {{ open ? "mdi-folder-open" : "mdi-folder" }}
        </v-icon>

        <v-icon size="22" v-else>mdi-file</v-icon>
      </template>
    </v-treeview>
  </div>
</template>

<script>
import { getDirectoryChildren } from "../../workspaceService";
export default {
  props: {
    files: {
      type: Array,
      default: () => [],
    },
    height: {
      type: Number,
      default: 300,
    },
  },
  created() {},
  data() {
    return {
      initiallyOpen: [],
      typeFilter: null,
      searchText: "",
      active: [],
      open: [],
      currentDirectory: null,
    };
  },
  methods: {
    async getChildren(node) {
      const result = await getDirectoryChildren(node.path);
      const children = result.data.directoryChildren.map((x) => {
        if (x.type === "DIRECTORY") x.children = [];
        return x;
      });
      node.children.push(...children);

      return children;
    },
    onSelect: function (node) {
      this.$emit("select", node[0]);
    },
  },
};
</script>

<style>
</style>