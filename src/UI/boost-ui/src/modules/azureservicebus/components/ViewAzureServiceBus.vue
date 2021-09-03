<template>
  <v-row class="ma-4">
    <v-col md="6">
      <v-card elevation="1">
        <v-toolbar color="grey lighten-2" elevation="0" height="36">
          <v-toolbar-title>
            Queues/Topics/EventHubs/Notification Hubs/Relays
          </v-toolbar-title>
          <v-spacer></v-spacer>
        </v-toolbar>
        <v-card-text>
          <v-treeview :items="content"></v-treeview>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script>
import { getQueues, getTopics } from "../azureServiceBusService";

export default {
  components: {},
  props: ["connectionName"],
  created() {
    this.getContent();
  },
  data() {
    return {
      loading: false,
      contentLoading: false,
      searchText: "",
      withMismatchOnly: true,
      content: [],
      selectedNode: null
    };
  },
  watch: {},
  computed: {},
  methods: {
    async getContent() {
      this.loading = true;
      const queues = await getQueues(this.connectionName);
      const topics = await getTopics(this.connectionName);

      this.content = [
        {
          name: "Queues",
          children: await queues.data.azureServiceBusQueues.map(x => {
            return {
              name:
                x.name +
                "(" +
                x.activeMessagesCount +
                ", " +
                x.deadletterMessagesCount +
                ")"
            };
          })
        },
        {
          name: "Topics",
          children: await topics.data.azureServiceBusTopics
        },
        {
          name: "Event Hubs",
          children: []
        },
        {
          name: "Notification Hubs",
          children: []
        },
        {
          name: "Relays",
          children: []
        }
      ];

      this.loading = false;
    }
  }
};
</script>

<style></style>
