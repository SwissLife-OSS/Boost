<template>
  <v-row>
    <v-col md="4">
      <v-card elevation="1">
        <v-toolbar light color="grey lighten-2" elevation="0" height="42">
          <v-toolbar-title>Create file from Base64</v-toolbar-title>
          <v-spacer></v-spacer>
        </v-toolbar>
        <v-card-text>
          <v-row>
            <v-col md="12">
              <v-text-field
                label="File type"
                v-model="fileType"
                placeholder="txt"
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row dense>
            <v-col md="12"
              ><v-textarea
                label="Value"
                outlined
                v-model="value"
                rows="5"
              ></v-textarea
            ></v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" @click="onClickView">View file</v-btn>
        </v-card-actions>
      </v-card>
    </v-col>
    <v-col md="8">
      <v-toolbar v-if="file" elevation="0">
        <a href="#" @click="onDownload">{{ file.path }}</a>
        <v-spacer></v-spacer>
        <v-icon @click="file = null">mdi-close</v-icon>
      </v-toolbar>
      <file-editor
        v-if="file"
        :file="file"
        :height="$vuetify.breakpoint.height - 180"
      ></file-editor>
    </v-col>
  </v-row>
</template>

<script>
import { createFileFromBase64 } from "../../workspaceService";
import FileEditor from "../Workspace/FileEditor.vue";

export default {
  components: { FileEditor },
  data() {
    return {
      file: null,
      fileType: null,
      value: null,
    };
  },
  methods: {
    async onClickView() {
      const result = await createFileFromBase64({
        fileType: this.fileType,
        value: this.value,
      });

      this.file = result.data.createFileFromBase64.file;
    },
    onDownload: function () {
      window.location.href = "/api/file/content/download/" + this.file.meta.id;
    },
  },
};
</script>

<style>
</style>