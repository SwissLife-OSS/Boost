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
        <v-col md="3"
          ><v-switch v-model="withMismatchOnly" label="Mismatch"></v-switch>
        </v-col>
        <v-col md="1">
          <v-icon
            class="mt-4"
            @click="onRefresh"
            color="grey darken-2"
            size="30"
            >mdi-refresh</v-icon
          >
        </v-col>
      </v-row>
      <v-alert
        v-if="directories.length === 0 && !loading"
        type="success"
        outlined
      >
        Hooray! no mismatches found.
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
          <v-switch
            v-if="isDiff"
            class="mt-5"
            v-model="inline"
            label="Inline diff"
          ></v-switch>
          <v-icon class="ml-4" @click="content = null">mdi-close</v-icon>
        </v-toolbar>
        <div v-if="isDiff">
          <MonacoEditor
            ref="editor"
            class="editor"
            :key="inline"
            :diffEditor="true"
            :options="diffOptions"
            :value="content.mismatch"
            :original="content.snapshot"
            language="json"
            :style="{ height: $vuetify.breakpoint.height - 120 + 'px' }"
          />
        </div>
        <div v-else>
          <MonacoEditor
            class="editor"
            :value="content.snapshot"
            language="json"
            :style="{ height: $vuetify.breakpoint.height - 120 + 'px' }"
          />
        </div>
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
import MonacoEditor from "../../../core/components/Common/MonacoEditor";
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
      diffOptions: {
        renderSideBySide: true,
      },
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
    inline: {
      get: function () {
        return !this.diffOptions.renderSideBySide;
      },
      set(value) {
        this.diffOptions.renderSideBySide = !value;
      },
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
      if (nodes && nodes.length > 0) {
        this.getContent(nodes[0]);
      }
    },
    onRefresh: function () {
      this.getDirectories();
    },
    setInline: function (value) {
      this.diffOptions.renderSideBySide = !value;

      this.$refs.editor.reset();

      console.log(value);
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
      this.content = null;
      const result = await getContent({
        fileName: node.fileName,
        missmatchFileName: node.missmatchFileName,
      });

      this.content = result.data.snapshooterSnapshot;
    },
    async approveAll() {
      await approveAll();

      this.content = null;

      this.getDirectories();
    },
    async approve() {
      await approve({
        fileName: this.selectedNode.fileName,
        missmatchFileName: this.selectedNode.missmatchFileName,
      });
      this.content = "";
      this.getDirectories();
    },
  },
};
</script>

<style>
</style>