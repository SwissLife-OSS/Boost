<template>
  <sub-navigation-bar :tabs="tabs"></sub-navigation-bar>
</template>

<script>
import SubNavigationBar from "../Common/SubNavigationBar.vue";
export default {
  components: { SubNavigationBar },
  created() {
    if (this.$route.name === "Security") {
      this.$router.push({ name: this.tabs[0].route });
    }
  },
  data() {
    return {
      tabs: [
        { name: "Tokens", route: "Security.Tokens" },
        { name: "Authorize (OIDC)", route: "Security.Authorize" },
        { name: "Token requestor", route: "Security.TokenRequest" },
      ],
    };
  },
};
</script>
