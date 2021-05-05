<template>
  <sub-navigation-bar :tabs="tabs"></sub-navigation-bar>
</template>

<script>
import SubNavigationBar from "../Common/SubNavigationBar.vue";
export default {
  components: { SubNavigationBar },
  created() {
    if (this.$route.name === "Utils") {
      this.$router.replace({ name: this.tabs[0].route });
    }
  },
  data() {
    return {
      tabs: [
        { name: "Encoding & Guids", route: "Utils.Encoding" },
        { name: "Base64 to File", route: "Utils.Base64ToFile" },
        { name: "Security", route: "Utils.Security" },
      ],
    };
  },
};
</script>
