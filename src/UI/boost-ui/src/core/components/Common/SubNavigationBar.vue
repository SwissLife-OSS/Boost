<template>
  <div>
    <v-system-bar window dark color="#a11c36" height="32">
      <router-link
        :to="{ name: tab.route }"
        class="tab-button"
        v-for="tab in tabs"
        :key="tab.id"
      >
        <v-icon class="mr-1" color="white" v-if="tab.icon">{{
          tab.icon
        }}</v-icon>

        {{ tab.title }}
      </router-link>
      <slot></slot>
    </v-system-bar>
    <div class="ma-2 pa-0">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    items: {
      type: Array,
      default: () => [],
    },
    navId: {
      type: String,
    },
  },

  mounted() {
    if (this.navId && this.$route.name === this.navId) {
      this.$router.replace({ name: this.tabs[0].route });
    }
  },
  computed: {
    tabs: function () {
      if (this.items.length > 0) {
        return this.items;
      }
      const tabs = this.$store.getters["app/subNavigation"](this.navId);

      this.$emit("loaded", tabs);
      return tabs;
    },
  },
};
</script>

<style scoped>
.tab-button {
  color: #fff;
  size: 12px;
  margin-left: 10px;
  padding-left: 4px;
  padding-right: 4px;
  text-decoration: none;
}

.tab-button.router-link-active {
  border-bottom: solid 2px #fff;
  border-radius: 1px;
}
</style>
