<template>
  <v-menu
    v-model="menu"
    :close-on-content-click="false"
    :nudge-width="300"
    offset-y
    rounded="2"
    top
  >
    <template v-slot:activator="{ on, attrs }">
      <v-btn text v-bind="attrs" v-on="on">Save request</v-btn>
    </template>
    <v-card>
      <v-card-title>
        <v-toolbar elevation="0" height="38" color="grey lighten-4">
          <h5>Save request</h5>
        </v-toolbar>
      </v-card-title>
      <v-card-text>
        <v-row>
          <v-col md="12">
            <v-text-field v-model="request.name" label="Name"></v-text-field>
          </v-col>
        </v-row>
        <v-row dense>
          <v-col md="12">
            <v-combobox
              label="Tags"
              v-model="request.tags"
              chips
              multiple
              clearable
              deletable-chips
              :items="request.tags"
            ></v-combobox>
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>

        <v-btn text @click="menu = false">Cancel</v-btn>
        <v-btn
          v-if="request.id"
          :disabled="request.name.length < 3"
          @click="onSave(true)"
          >Save as</v-btn
        >
        <v-btn
          :disabled="request.name.length < 3"
          color="primary"
          @click="onSave(false)"
          >Save</v-btn
        >
      </v-card-actions>
    </v-card>
  </v-menu>
</template>

<script>
import { saveRequest } from "../../tokenService";

export default {
  props: {
    request: {
      type: Object,
      default() {
        return {
          id: null,
          name: null,
          tags: [],
        };
      },
    },
    data: {
      type: Object,
    },
  },
  data() {
    return {
      menu: null,
    };
  },
  methods: {
    async onSave(saveAs) {
      let input = {
        name: this.request.name,
        id: this.request.id,
        type: this.request.type,
        tags: this.request.tags,
        data: this.data,
      };
      this.menu = false;

      if (saveAs === true) {
        input.id = null;
      }

      console.log(input);
      const result = await saveRequest(input);

      const { item } = result.data.saveIdentityRequest;
      this.request.id = item.id;
      this.request.name = item.name;
    },
  },
};
</script>

<style>
</style>