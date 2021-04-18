<template>
  <v-list
    v-if="commits"
    two-line
    :style="'max-height:' + height + 'px'"
    class="overflow-y-auto"
  >
    <v-list-item
      v-for="commit in commits"
      :key="commit.id"
      @click="openExternal(commit.webUrl)"
    >
      <v-list-item-content>
        <v-list-item-title
          ><strong>{{ commit.author }}</strong> |
          {{ commit.createdAt | dateformat }}</v-list-item-title
        >
        <v-card-subtitle>
          {{ commit.message }}
        </v-card-subtitle>
      </v-list-item-content>
      <v-list-item-action> </v-list-item-action>
    </v-list-item>
  </v-list>
</template>

<script>
export default {
  props: {
    commits: {
      type: Array,
      default: () => [],
    },
    height: {
      type: Number,
      default: 200,
    },
  },
  methods: {
    openExternal(url) {
      window.open(url);
    },
  },
};
</script>

<style>
</style>