<template>
  <div>
    <slot v-if="ready"></slot>
    <slot v-if="isRootSlot" name="error"></slot>
    <v-app v-if="!ready">
      <loader-center></loader-center>
    </v-app>
  </div>
</template>

<script>
import { mapActions } from "vuex";
import LoaderCenter from "./LoaderCenter.vue";
export default {
  components: { LoaderCenter },
  data: () => ({
    message: "Authenticating user...",
    me: {},
  }),

  created() {
    this.loadInitialData();
  },
  watch: {
    error: function (newValue) {
      if (newValue && !this.$route.meta.isRoot) {
        this.$router.push({ name: "Error" });
      }
    },
  },
  computed: {
    ready: function () {
      return this.$store.state.app.userSettings;
    },
    isRootSlot: function () {
      return this.error || this.$route.meta.isRoot;
    },
  },
  methods: {
    ...mapActions("app", ["loadInitialData"]),
  },
};
</script>

<style>
</style>

