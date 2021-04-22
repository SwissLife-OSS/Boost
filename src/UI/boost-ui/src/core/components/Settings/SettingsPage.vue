<template>
  <sub-navigation-bar :tabs="tabs"></sub-navigation-bar>
</template>

<script>
import SubNavigationBar from "../Common/SubNavigationBar.vue";
export default {
  components: { SubNavigationBar },
  created() {
    if (this.$route.name === "Settings") {
      this.$router.push({ name: this.tabs[0].route });
    }
  },
  data() {
    return {
      tabs: [
        { name: "General", route: "Settings.General" },
        { name: "Security", route: "Settings.Security" },
      ],
    };
  },
};
</script>
