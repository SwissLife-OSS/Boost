<template>
  <sub-navigation-bar :tabs="tabs"></sub-navigation-bar>
</template>

<script>
import SubNavigationBar from "../Common/SubNavigationBar.vue";
export default {
  components: { SubNavigationBar },
  created() {
    if (this.$route.name === "Git") {
      this.$router.push({ name: this.tabs[0].route });
    }
  },
  data() {
    return {
      tabs: [
        { name: "Current", route: "Git.Current" },
        { name: "All Local", route: "Git.Local" },
        { name: "Remote", route: "Git.Remote" },
        { name: "Indexing", route: "Git.Index" },
      ],
    };
  },
};
</script>
