<template>
  <v-row>
    <v-col md="2">
      <v-text-field
        clearable
        :loading="listLoading"
        v-model="searchText"
        @keyup.enter.native="onSearch"
        placeholder="Search"
        prepend-icon="mdi-magnify"
      ></v-text-field>
      <v-list
        two-line
        dense
        :style="{ 'max-height': $vuetify.breakpoint.height - 180 + 'px' }"
        class="overflow-y-auto mt-0"
      >
        <v-list-item-group color="primary" select-object>
          <v-list-item
            v-for="repo in repos"
            :key="repo.id"
            selectable
            @click="onSelectRepo(repo)"
          >
            <v-list-item-avatar size="24">
              <v-img :src="serviceImage(repo)"></v-img>
            </v-list-item-avatar>
            <v-list-item-content>
              <v-list-item-title v-text="repo.name"></v-list-item-title>
              <v-list-item-subtitle
                v-text="repo.workRoot"
              ></v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-list-item-group>
      </v-list>
    </v-col>
    <v-col md="10">
      <v-alert
        v-if="indexEmptyWarning && repos.length === 0"
        type="warning"
        outlined
      >
        Looks like you have not indexed you local repositories. Make sure you
        have configured at least 1 work root and you did execute indexing.
        <br />
        <br />
        <router-link :to="{ name: 'Git.Index' }">Index now</router-link>
      </v-alert>

      <router-view></router-view>
    </v-col>
  </v-row>
</template>

<script>
import { mapActions, mapState } from "vuex";

export default {
  created() {
    this.onSearch();
  },
  data() {
    return {
      searchText: "",
    };
  },
  computed: {
    ...mapState("git", {
      repos: (state) => state.local.items,
      listLoading: (state) => state.local.loading,
      indexEmptyWarning: (state) => state.local.indexEmptyWarning,
    }),
  },
  methods: {
    ...mapActions("git", ["searchLocalRepos"]),
    onSelectRepo: function (repo) {
      this.$router.push({
        name: "Git.Repo.LocalDetails",
        params: { id: repo.id },
      });
    },
    onSearch: function () {
      this.searchLocalRepos({ term: this.searchText });
    },
    serviceImage: function (repo) {
      if (repo.service) {
        return require(`../../../assets/cs/${repo.service.type}.png`);
      }

      return require("../../../assets/git.png");
    },
  },
};
</script>

<style>
</style>