<template>
  <v-list
    two-line
    dense
    :style="{ 'max-height': height + 'px' }"
    class="overflow-y-auto mt-0"
  >
    <v-list-item-group color="primary" select-object>
      <v-list-item
        v-for="(link, i) in links"
        :key="i"
        selectable
        @click="onClickLink(link)"
      >
        <v-list-item-avatar size="24">
          <v-icon v-if="link.icon === null || link.icon.startsWith('mdi')">{{
            getIcon(link.icon)
          }}</v-icon>
          <v-img v-else :src="iconSource(link)"></v-img>
        </v-list-item-avatar>

        <v-list-item-content>
          <v-list-item-title v-text="link.title"></v-list-item-title>
          <v-list-item-subtitle v-text="link.subtitle"></v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>
    </v-list-item-group>
  </v-list>
</template>

<script>
export default {
  props: {
    links: {
      type: Array,
      default: () => [],
    },
    height: {
      type: Number,
      default: 500,
    },
  },
  methods: {
    onClickLink: function (link) {
      window.open(link.url);
    },
    getIcon: function (icon) {
      if (icon) {
        return icon;
      }
      return "mdi-earth-arrow-right";
    },
    iconSource: function (link) {
      if (link.icon.startsWith("http")) {
        return link.icon;
      }

      switch (link.icon.toLowerCase()) {
        case "github":
          return require("../../../assets/cs/GitHub.png");
        case "abo":
          return require("../../../assets/cs/AzureDevOps.png");
        default:
          return null;
      }
    },
  },
};
</script>

<style>
</style>