<template>
  <v-row dense>
    <v-col md="3">
      <v-row>
        <v-col>
          <file-explorer-view
            @select="onSelectItem"
            :height="explorerHeight"
            :files="files"
          ></file-explorer-view
        ></v-col>
      </v-row>
      <v-row v-if="showQuickActions">
        <v-col>
          <v-toolbar color="grey lighten-4" height="32" elevation="0">
            <h4>Quick actions</h4>
            <v-spacer></v-spacer>
            <v-icon @click="showQuickActions = false">mdi-close</v-icon>
          </v-toolbar>
          <quick-actions-list
            :actions="quickActions"
            :height="200"
          ></quick-actions-list>
        </v-col>
      </v-row>
    </v-col>
    <v-col md="9" v-if="editor.visible === false">
      <div>
        <v-row class="mx-2 mt-1">
          <v-col md="3">
            <v-select
              label="Shell"
              v-model="console.shell"
              :items="console.shells"
            ></v-select>
          </v-col>
          <v-col md="9"
            ><v-text-field
              label="Command"
              v-model="console.command"
              v-on:keyup.enter="onExec"
              prepend-icon="mdi-powershell"
              append-outer-icon="mdi-delete-variant"
              @click:append-outer="onClickClear"
            ></v-text-field
          ></v-col>
        </v-row>
        <console :height="$vuetify.breakpoint.height - 150"></console></div
    ></v-col>
    <v-col md="9" v-if="editor.visible">
      <v-toolbar elevation="0">
        <v-toolbar-title>{{ editor.file }}</v-toolbar-title>

        <v-btn
          class="mx-4"
          small
          color="purple"
          dark
          v-for="action in editor.meta.actions"
          :key="action"
          @click="onClickAction(action)"
          >{{ action }}</v-btn
        >

        <v-spacer></v-spacer>
        <div style="width: 150px" class="mt-6 ml-4">
          <v-select
            dense
            v-model="converter"
            @change="onChangeConverter"
            :items="converters"
          >
          </v-select>
        </div>
        <v-icon @click="onCloseEditor">mdi-close</v-icon>
      </v-toolbar>
      <file-editor
        :file="editor"
        :height="$vuetify.breakpoint.height - 120"
      ></file-editor>
    </v-col>
  </v-row>
</template>

<script>
import {
  getFile,
  executeFileAction,
  getWorkspace,
} from "../../workspaceService";
import Console from "../Common/Console";
import FileExplorerView from "./FileExplorerView.vue";

import { mapActions } from "vuex";
import QuickActionsList from "./QuickActionsList.vue";

import workspaceMixins from "../workspaceMixins";
import FileEditor from "./FileEditor.vue";

export default {
  mixins: [workspaceMixins],
  components: {
    FileExplorerView,
    Console,
    QuickActionsList,
    FileEditor,
  },
  created() {
    this.loadWorkspace();
    this.getQuickActions();
  },
  data: () => ({
    quickActions: [],
    showQuickActions: false,
    files: [],
    console: {
      command: "",
      shell: "cmd",
      shells: ["cmd", "powershell", "pwsh"],
    },
    converter: "ORIGINAL",
    editor: {
      visible: false,
      content: "",
      meta: null,
      file: null,
      fullName: null,
      editor: null,
      contentType: null,
    },
  }),
  computed: {
    explorerHeight: function () {
      var space = 70;
      if (this.showQuickActions) {
        space = 315;
      }

      return this.$vuetify.breakpoint.height - space;
    },
    converters: function () {
      return [
        "ORIGINAL",
        "BASE64",
        "BASE64-ILB",
        ...this.editor.meta.converters,
      ];
    },
  },
  methods: {
    ...mapActions("workspace", ["executeCommand", "clearConsoleMessages"]),
    async loadWorkspace() {
      const result = await getWorkspace();

      this.files = result.data.workspace.files.map((x) => {
        if (x.type === "DIRECTORY") x.children = [];
        return x;
      });
    },
    async getQuickActions() {
      this.quickActions = await this.loadQuickActions();

      if (this.quickActions.length > 0) {
        this.showQuickActions = true;
      }
    },
    onExec: function () {
      this.executeCommand({
        command: this.console.command,
        shell: this.console.shell,
      });
    },
    onClickClear: function () {
      this.clearConsoleMessages();
    },
    onSelectItem: function (item) {
      if (item.type === "FILE") {
        this.loadFile(item.path);
      }
    },
    onCloseEditor: function () {
      this.editor.content = null;
      this.editor.visible = false;
    },
    async loadFile(fileName) {
      const input = { fileName };
      if (this.converter && this.converter != "ORIGINAL") {
        input.converter = this.converter;
      }

      const result = await getFile(input);
      const { file } = result.data;

      this.editor.content = file.content;
      this.editor.meta = file.meta;
      this.editor.file = file.name;
      this.editor.fullName = file.path;
      this.editor.visible = true;
      this.editor.contentType = file.contentType;
      this.editor.editor = file.editor;
    },
    onChangeConverter: function () {
      this.loadFile(this.editor.fullName);
    },
    async onClickAction(action) {
      this.editor.visible = false;

      await executeFileAction({
        file: this.editor.fullName,
        action,
      });
    },
  },
};
</script>

