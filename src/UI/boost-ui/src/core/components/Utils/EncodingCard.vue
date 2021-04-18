<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title> Encode & Decode </v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card-text>
      <v-row dense>
        <v-col md="12"
          ><v-textarea
            label="Input"
            outlined
            v-model="input"
            rows="3"
          ></v-textarea
        ></v-col>
      </v-row>
      <v-row dense>
        <v-col md="12">
          <v-toolbar height="24" elevation="0">
            <v-btn-toggle dense v-model="direction" @change="convert" rounded>
              <v-btn small value="ENCODE">Encode</v-btn>
              <v-btn small value="DECODE">Decode</v-btn>
            </v-btn-toggle>
            <v-spacer></v-spacer>
            <v-btn-toggle dense v-model="type" @change="convert" rounded>
              <v-btn small value="BASE64">Base64 (UTF-8)</v-btn>
            </v-btn-toggle>
          </v-toolbar>
        </v-col>
      </v-row>
      <v-row>
        <v-col md="12"
          ><v-textarea
            label="Output"
            outlined
            :value="output"
            rows="3"
          ></v-textarea
        ></v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script>
import { encode, decode } from "../../utilsService";

export default {
  data() {
    return {
      type: "BASE64",
      direction: "ENCODE",
      input: null,
      output: null,
    };
  },
  watch: {
    input: function () {
      this.convert();
    },
  },
  methods: {
    async convert() {
      if (this.direction === "ENCODE") {
        const res = await encode(this.input, this.type);
        this.output = res.data.encode;
      } else if (this.direction === "DECODE") {
        const res = await decode(this.input, this.type);
        this.output = res.data.decode;
      }
    },
  },
};
</script>

<style>
</style>