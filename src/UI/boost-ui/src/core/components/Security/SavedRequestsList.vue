<template>
  <div>
    <v-row dense>
      <v-col md="6">
        <v-text-field
          clearable
          :loading="loading"
          v-model="searchText"
          placeholder="Search"
          prepend-icon="mdi-magnify"
        ></v-text-field
      ></v-col>
      <v-col md="6">
        <v-select label="Tag" :items="tags" v-model="tag" clearable></v-select>
      </v-col>
    </v-row>

    <v-list
      two-line
      dense
      :style="{ 'max-height': $vuetify.breakpoint.height - 180 + 'px' }"
      class="overflow-y-auto mt-0"
    >
      <v-list-item-group color="primary" select-object>
        <v-list-item
          dense
          v-for="request in filteredRequests"
          :key="request.id"
          selectable
          @click="onSelectRequest(request)"
        >
          <v-list-item-avatar size="24">
            <v-icon>mdi-file-document-outline</v-icon>
          </v-list-item-avatar>

          <v-list-item-content>
            <v-list-item-title v-text="request.name"></v-list-item-title>
            <div>
              <v-chip
                x-small
                color="pink darken-4"
                class="ma-1"
                dark
                v-for="(tag, i) in request.tags"
                :key="i"
                >{{ tag }}</v-chip
              >
            </div>
          </v-list-item-content>
          <v-list-item-action>
            <v-btn icon>
              <v-icon color="grey" @click.stop="onDelete(request)"
                >mdi-delete</v-icon
              >
            </v-btn>
          </v-list-item-action>
        </v-list-item>
      </v-list-item-group>
    </v-list>
  </div>
</template>

<script>
import {
  deleteRequest,
  getIdentityRequest,
  searchIdentityRequests,
} from "../../tokenService";
export default {
  props: ["type"],
  created() {
    this.search();
  },
  data() {
    return {
      requests: [],
      loading: false,
      searchText: "",
      tag: null,
    };
  },
  computed: {
    filteredRequests: function () {
      let request = this.requests;
      if (this.searchText) {
        const regex = new RegExp(`.*${this.searchText}.*`, "i");
        request = request.filter((x) => regex.test(x.name));
      }

      if (this.tag) {
        request = request.filter((x) => x.tags.includes(this.tag));
      }

      return request;
    },
    tags: function () {
      var tags = [];
      for (let i = 0; i < this.requests.length; i++) {
        const request = this.requests[i];
        for (let j = 0; j < request.tags.length; j++) {
          const tag = request.tags[j];
          if (!tags.includes(tag)) {
            tags.push(tag);
          }
        }
      }
      return tags;
    },
  },
  methods: {
    async search() {
      const input = {
        type: this.type,
      };
      this.loading = true;
      const result = await searchIdentityRequests(input);
      this.requests = result.data.searchIdentityRequests;
      this.loading = false;
    },
    async onSelectRequest(request) {
      const result = await getIdentityRequest(request.id);
      this.$emit("select", result.data.identityRequest);
    },
    async onDelete(request) {
      await deleteRequest(request.id);
      const index = this.requests.findIndex((x) => x.id === request.id);
      this.requests.splice(index, 1);
    },
  },
};
</script>

<style>
</style>