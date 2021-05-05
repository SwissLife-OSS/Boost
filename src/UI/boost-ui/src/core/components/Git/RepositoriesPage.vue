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
                v-text="repo.fullName"
              ></v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-list-item-group>
      </v-list>
    </v-col>

    <v-col md="10">
      <div v-if="repos.length === 0 && firstLoad">
        <h4>Remote git repositories</h4>
        <p>
          You can search and view your configured remote git repositories here.
        </p>

        <div v-for="cs in connectedServices" :key="cs.id"></div>

        <p v-if="connectedServices.length === 0">
          <v-alert type="warning" outlined>
            Look like you don't have any connected services setup. :-(
            <br /><br />
            <router-link :to="{ name: 'Settings' }">Setup now</router-link>
          </v-alert>
        </p>
      </div>

      <router-view></router-view>
    </v-col>
  </v-row>
</template>

<script>
import { mapActions, mapState } from "vuex";

export default {
  created() {
    if (this.connectedServices.length === 0) {
      this.loadConnectedServices();
    }
  },
  data() {
    return {
      searchText: "",
      firstLoad: true,
    };
  },
  computed: {
    ...mapState("git", {
      repos: (state) => state.list.items,
      listLoading: (state) => state.list.loading,
    }),
    ...mapState("app", {
      connectedServices: (state) => state.connectedServices,
    }),
  },
  methods: {
    ...mapActions("git", ["searchRepos"]),
    ...mapActions("app", ["loadConnectedServices"]),
    onSelectRepo: function (repo) {
      this.$router.push({
        name: "Git.Repo.Details",
        params: { id: repo.id, serviceId: repo.service.id },
      });
    },
    onSearch: function () {
      this.firstLoad = false;
      this.searchRepos({ term: this.searchText });
    },
    serviceImage: function (repo) {
      return require(`../../../assets/cs/${repo.service.type}.png`);
    },
  },
};
</script>

<style>
</style>