<template>
  <v-card elevation="1">
    <v-toolbar color="grey lighten-2" elevation="0" height="36">
      <v-toolbar-title>Token Requestor</v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text>
      <v-row>
        <v-col md="12">
          <v-textarea
            label="Identity Servers"
            v-model="identityServers"
            rows="2"
          ></v-textarea
        ></v-col>
      </v-row>

      <v-row>
        <v-col md="12">
          <v-toolbar elevation="0" height="36" color="grey lighten-2">
            <h4>Grants</h4>
            <v-spacer></v-spacer>
            <v-btn text color="red" @click="onAdd">
              <v-icon class="mr-2">mdi-folder-plus</v-icon> Add grant
            </v-btn>
          </v-toolbar>

          <v-row v-for="(grant, i) in grants" :key="i">
            <v-col md="11">
              <v-row>
                <v-col>
                  <v-text-field label="Name" v-model="grant.name"></v-text-field
                ></v-col>
              </v-row>
              <v-toolbar elevation="0" height="36" color="grey lighten-3">
                <h4>Parameters</h4>
                <v-spacer></v-spacer>
                <v-btn text color="red" @click="onAddParameter(grant)">
                  <v-icon class="mr-2">mdi-folder-plus</v-icon> Add parameter
                </v-btn>
              </v-toolbar>

              <v-row v-for="(par, j) in grant.parameters" :key="j">
                <v-col md="5">
                  <v-text-field label="Name" v-model="par.name"></v-text-field>
                </v-col>
                <v-col md="5">
                  <v-text-field
                    label="Label"
                    v-model="par.label"
                  ></v-text-field>
                </v-col>
                <v-col md="2">
                  <v-icon class="mt-5" @click="onRemoveParameter(grant, j)"
                    >mdi-delete-outline</v-icon
                  >
                </v-col>
              </v-row>
            </v-col>
            <v-col md="1">
              <v-icon class="mt-5" @click="onRemove(i)"
                >mdi-delete-outline</v-icon
              >
            </v-col>
          </v-row>
        </v-col>
      </v-row>
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
    this.identityServers = this.$store.state.app.userSettings.tokenGenerator.identityServers.join(
      "\n"
    );
    if (
      this.$store.state.app.userSettings.tokenGenerator.customGrants.length > 0
    ) {
      this.grants = this.$store.state.app.userSettings.tokenGenerator.customGrants;
    }
  },
  data() {
    return {
      identityServers: "",
      grants: [
        {
          name: "",
          parameters: [],
        },
      ],
    };
  },
  methods: {
    ...mapActions("app", ["saveTokenRequestorSettings"]),

    onAdd: function () {
      this.grants.push({
        name: "",
        parameters: [],
      });
    },
    onRemove: function (index) {
      this.grants.splice(index, 1);
    },
    onAddParameter: function (grant) {
      grant.parameters.push({
        name: "",
        label: "",
      });
    },
    onRemoveParameter: function (grant, index) {
      grant.parameters.splice(index, 1);
    },
    async save() {
      const settings = {
        identityServers: this.identityServers
          .split("\n")
          .filter((x) => x.length > 0),
        customGrants: this.grants
          .filter((x) => x.name.length > 0)
          .map((x) => {
            return {
              name: x.name,
              parameters: x.parameters
                .filter((p) => p.name.length > 0)
                .map((p) => {
                  return {
                    name: p.name,
                    label: p.label,
                  };
                }),
            };
          }),
      };

      await this.saveTokenRequestorSettings({
        settings: settings,
      });
    },
  },
};
</script>

<style>
</style>