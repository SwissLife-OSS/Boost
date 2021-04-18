<template>
  <v-card elevation="1">
    <v-toolbar color="grey lighten-2" elevation="0" height="36">
      <v-toolbar-title> Work roots </v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text>
      <div class="mt-4" v-if="workroots">
        <v-row v-for="(wr, i) in workroots" :key="i">
          <v-col md="4"
            ><v-text-field label="Name" v-model="wr.name"></v-text-field
          ></v-col>
          <v-col md="6"
            ><v-text-field label="Path" v-model="wr.path"></v-text-field
          ></v-col>
          <v-col md="1"
            ><v-checkbox
              title="Default"
              @change="onDefaultChanged(wr.isDefault, i)"
              v-model="wr.isDefault"
            ></v-checkbox
          ></v-col>
          <v-col md="1">
            <v-icon class="mt-5" @click="onRemove(i)"
              >mdi-delete-outline</v-icon
            >
          </v-col>
        </v-row>
      </div>

      <v-toolbar elevation="0">
        <v-spacer></v-spacer>
        <v-btn text color="secondary" @click="onAdd">
          <v-icon class="mr-2">mdi-folder-plus</v-icon> Add
        </v-btn>
      </v-toolbar>
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn color="primary" @click="save">Save</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import { mapActions } from "vuex";
export default {
  mounted() {
    if (this.$store.state.app.userSettings.workRoots.length > 0) {
      this.workroots = this.$store.state.app.userSettings.workRoots;
    }
  },
  data() {
    return {
      workroots: [
        {
          name: "",
          path: "",
          isDefault: true,
        },
      ],
    };
  },
  methods: {
    ...mapActions("app", ["saveWorkRoots"]),
    onDefaultChanged: function (value, index) {
      if (value === true) {
        for (let i = 0; i < this.workroots.length; i++) {
          const wr = this.workroots[i];
          if (i != index) {
            wr.isDefault = false;
          }
        }
      }
    },
    onAdd: function () {
      this.workroots.push({
        name: "",
        path: "",
        isDefault: this.workroots.filter((x) => x.isDefault).length === 0,
      });
    },
    onRemove: function (index) {
      this.workroots.splice(index, 1);
      if (this.workroots.length > 0) {
        if (this.workroots.filter((x) => x.isDefault).length === 0) {
          this.workroots[0].isDefault = true;
        }
      }
    },
    async save() {
      const workRoots = this.workroots
        .filter((x) => x.path.length > 0)
        .map((x) => {
          return {
            name: x.name,
            path: x.path,
            isDefault: x.isDefault,
          };
        });

      this.saveWorkRoots({ workRoots });
    },
  },
};
</script>

<style>
</style>