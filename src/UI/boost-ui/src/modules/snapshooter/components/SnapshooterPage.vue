<template>
  <v-row class="mx-4">
    <v-col md="4">
      <v-row>
        <v-col md="8">
          <v-text-field
            v-model="searchText"
            label="Search"
            prepend-inner-icon="mdi-magnify"
            flat
            hide-details
            clearable
          ></v-text-field
        ></v-col>
        <v-col md="4"
          ><v-switch v-model="withMismatchOnly" label="Missmatch"></v-switch>
        </v-col>
      </v-row>
      <v-alert
        v-if="directories.length === 0 && !loading"
        type="success"
        outlined
      >
        Hooray! no missmatches found.
      </v-alert>
      <v-progress-linear
        color="primary"
        top
        indeterminate
        v-if="loading"
      ></v-progress-linear>
      <v-treeview
        :style="{
          height: $vuetify.breakpoint.height - 100 + 'px',
        }"
        class="overflow-y-auto mt-0"
        :return-object="true"
        :open-on-click="true"
        :items="directories"
        :search="searchText"
        activatable
        item-key="fullName"
        ref="tree"
        :multiple-active="false"
        @update:active="onSelect"
        dense
        transition
      >
        <template v-slot:prepend="{ open, item }">
          <v-icon size="20" color="yellow darken-2" v-if="item.root">
            {{ open ? "mdi-folder-open" : "mdi-folder" }}
          </v-icon>
          <v-icon size="22" color="red" v-if="!item.root && item.hasMismatch"
            >mdi-alert-circle</v-icon
          >
          <v-icon size="22" v-if="!item.root && !item.hasMismatch"
            >mdi-code-json</v-icon
          >
        </template>
      </v-treeview></v-col
    >
    <v-col md="8">
      <div v-if="content">
        <v-toolbar elevation="0">
          <v-toolbar-title>{{ content.name }}</v-toolbar-title>
          <v-btn @click="approve" v-if="isDiff" class="mx-4" color="green" dark>
            <v-icon class="mr-2">mdi-check-circle</v-icon>
            Accept</v-btn
          >
          <v-spacer></v-spacer>
          <v-icon @click="content = null">mdi-close</v-icon>
        </v-toolbar>
        <MonacoEditor
          class="editor"
          :diffEditor="isDiff"
          :value="content.snapshot"
          :original="content.mismatch"
          language="json"
          :style="{ height: $vuetify.breakpoint.height - 120 + 'px' }"
        />
      </div>
      <v-toolbar v-if="content == null && missmatchCount > 0" elevation="0">
        <v-btn class="mx-4" color="green" @click="approveAll" dark>
          <v-icon class="mr-2">mdi-check-circle</v-icon>
          Accept all missmatches ({{ missmatchCount }})</v-btn
        >
      </v-toolbar>
    </v-col>
  </v-row>
</template>

<script>
import MonacoEditor from "vue-monaco";
import {
  getsnapshooterDirectories,
  getContent,
  approve,
  approveAll,
} from "../snapshooterService";

export default {
  components: { MonacoEditor },
  created() {
    this.getDirectories();
  },
  data() {
    return {
      loading: false,
      active: [],
      searchText: "",
      withMismatchOnly: true,
      directories: [],
      content: null,
      selectedNode: null,
    };
  },
  watch: {
    withMismatchOnly: function () {
      this.getDirectories();
    },
  },
  computed: {
    isDiff: function () {
      if (this.content && this.content.mismatch) {
        return true;
      }
      return false;
    },
    missmatchCount: function () {
      let count = 0;
      for (let i = 0; i < this.directories.length; i++) {
        const dir = this.directories[i];
        if (dir.snapshots) {
          count += dir.snapshots.filter((x) => x.hasMismatch).length;
        }
      }

      return count;
    },
  },
  methods: {
    onSelect: function (nodes) {
      console.log(nodes);
      if (nodes && nodes.length > 0) {
        this.getContent(nodes[0]);
      }
    },
    async getDirectories() {
      this.loading = true;
      const result = await getsnapshooterDirectories(this.withMismatchOnly);

      this.directories = await result.data.snapshooterDirectories.map((x) => {
        x.children = x.snapshots.map((c) => {
          c.fullName = c.fileName;

          return c;
        });
        x.root = true;

        return x;
      });
      this.$nextTick(() => {
        window.setTimeout(() => {
          this.$refs.tree.updateAll(true);
        }, 150);
      });
      this.loading = false;
    },
    async getContent(node) {
      this.selectedNode = node;
      const result = await getContent({
        fileName: node.fileName,
        missmatchFileName: node.missmatchFileName,
      });

      this.content = result.data.snapshooterSnapshot;
    },
    async approveAll() {
      const result = await approveAll();

      console.log(result);
      this.content = null;

      this.getDirectories();
    },
    async approve() {
      await approve({
        fileName: this.selectedNode.fileName,
        missmatchFileName: this.selectedNode.missmatchFileName,
      });
      this.content = null;
      this.getDirectories();
    },
  },
};
</script>

<style>
</style>