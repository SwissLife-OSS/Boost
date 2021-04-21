<template>
  <v-card elevation="1">
    <v-toolbar light color="grey lighten-2" elevation="0" height="42">
      <v-toolbar-title> Create Hash</v-toolbar-title>
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
            <v-btn-toggle dense v-model="alg" @change="convert" rounded>
              <v-btn v-for="alg in algs" small :key="alg" :value="alg">{{
                alg
              }}</v-btn>
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
import { createHash } from "../../utilsService";

export default {
  data() {
    return {
      alg: "SHA256",
      input: null,
      output: null,
      algs: ["SHA256", "SHA512"],
    };
  },
  watch: {
    input: function () {
      this.convert();
    },
  },
  methods: {
    async convert() {
      const res = await createHash({
        value: this.input,
        alg: this.alg,
      });

      this.output = res.data.createHash;
    },
  },
};
</script>

<style>
</style>