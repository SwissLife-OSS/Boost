<template>
  <v-card elevation="1" class="ma-1 mt-4">
    <v-toolbar elevation="0" color="grey lighten-4" height="42">
      <v-toolbar-title>Clone repository</v-toolbar-title>
    </v-toolbar>
    <v-card-text>
      <v-select
        :items="workRoots"
        label="Work Root"
        item-value="path"
        item-text="name"
        v-model="clone.directory"
      ></v-select>
      <v-alert v-if="clone.clonedDirectory" type="success" outlined>
        Repostitory was sucessfully cloned.
      </v-alert>
    </v-card-text>
    <v-card-actions>
      <router-link :to="{ name: 'Settings' }">Edit work roots</router-link>
      <v-spacer></v-spacer>
      <v-btn
        class="ma-2 white--text"
        color="primary"
        :disabled="clone.directory == null || clone.clonedDirectory != null"
        @click="onClickClone"
        >Clone<v-icon right>mdi-cloud-download</v-icon>
      </v-btn>
    </v-card-actions>
    <console v-if="clone.cloning" class="mt-4" :height="400"></console>
  </v-card>
</template>

<script>
import { mapState } from "vuex";
import { cloneRepo } from "../../gitService";
import Console from "../Common/Console.vue";
import workspaceMixins from "../workspaceMixins";
export default {
  mixins: [workspaceMixins],
  components: { Console },
  props: ["repo"],
  data() {
    return {
      clone: {
        cloning: false,
        directory: null,
        clonedDirectory: null,
      },
    };
  },
  computed: {
    ...mapState("app", {
      workRoots: (state) => state.userSettings.workRoots,
    }),
  },
  methods: {
    async onClickClone() {
      const input = {
        directory: this.clone.directory,
        url: this.repo.url,
      };
      this.clone.cloning = true;

      const result = await cloneRepo(input);
      this.clone.clonedDirectory = result.data.cloneRepository.directory;

      window.setTimeout(() => {
        this.$emit("updated", {
          directory: this.clone.clonedDirectory,
        });
      }, 2000);
    },
    onClickOpenInCode: function () {},
  },
};
</script>

<style>
</style>