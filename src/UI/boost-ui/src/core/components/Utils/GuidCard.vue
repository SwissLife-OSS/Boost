<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title>Guid tools</v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text>
      <v-row dense>
        <v-col md="10">
          <v-text-field
            label="Guid"
            :value="guid"
            prepend-icon="mdi-clipboard"
            append-outer-icon="mdi-refresh"
            @click:append-outer="create"
            v-clipboard
          ></v-text-field>
        </v-col>
        <v-col md="2">
          <v-switch
            dense
            label="Dashes"
            v-model="withDashes"
            @change="create"
          ></v-switch>
        </v-col>
      </v-row>
      <v-row dense>
        <v-col md="12">
          <v-text-field
            outlined
            label="Parse Guid"
            v-model="input"
          ></v-text-field>
        </v-col>
      </v-row>
      <v-row dense>
        <v-col md="12">
          <v-textarea
            outlined
            label="Result"
            :value="parsed"
            rows="3"
          ></v-textarea>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script>
import { createGuids, parseGuid } from "../../utilsService";

export default {
  created() {
    //this.create();
  },
  data() {
    return {
      guid: [],
      input: null,
      parsed: null,
      withDashes: false,
    };
  },
  watch: {
    input: function () {
      this.parse();
    },
  },
  methods: {
    async parse() {
      const result = await parseGuid(this.input);

      this.parsed = result.data.parseGuid.join("\n");
    },
    async create() {
      const result = await createGuids(1, this.withDashes ? null : "N");
      this.guid = result.data.createGuids[0];
      this.$clipboard(this.guid);
    },
  },
};
</script>
