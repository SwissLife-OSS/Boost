<template>
  <v-row class="mx-4">
    <v-col md="8">
      <div>
        <h2>Queues/Topics/EventHubs/Notification Hubs/Relays</h2>
        <v-treeview :items="content"></v-treeview>
      </div>
    </v-col>
    <v-col md="4"> </v-col>
  </v-row>
</template>

<script>
import { getQueues, getTopics } from "../azureServiceBusService";

export default {
  components: {},
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
      const queues = await getQueues();
      const topics = await getTopics();

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
