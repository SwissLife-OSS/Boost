<template>
  <v-card class="mx-2" color="grey darken-3" :loading="true" :height="height">
    <v-card-text class="console">
      <v-container
        fluid
        :style="{
          'max-height': height + 'px',
          height: height + 'px',
        }"
        class="overflow-y-auto ma-0 pa-0"
      >
        <div
          ref="console"
          class="console-line"
          v-for="(msg, i) in messages"
          :class="msg.tags.map((x) => 'console-' + x)"
          :key="i"
        >
          <v-icon v-if="msg.type == 'fatal'" small color="red"
            >mdi-check-circle</v-icon
          >
          {{ msg.message }}
        </div>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  components: {},
  props: ["loading", "height"],
  created() {},
  mounted() {},
  watch: {
    messages: function () {
      const lastElement = this.$el.querySelector(".container *:last-child");
      if (lastElement) {
        lastElement.scrollIntoView({ behavior: "smooth" });
      }
    },
  },
  data: () => ({
    console: {},
  }),
  computed: {
    messages: function () {
      return this.$store.state.workspace.console.messages;
    },
  },
  methods: {},
};
</script>

<style scoped>
.console {
  font-family: Lucida Console, Lucida Sans Typewriter, monaco,
    Bitstream Vera Sans Mono, monospace;

  font-size: 12px;
  height: 95%;
  margin: 0;
  padding: 2px 0px 2px 14px;
  color: rgba(233, 236, 236, 0.788) !important;
}

.console-line {
  line-height: 20px;
  margin: 0;
  padding: 0;
}

.console-line.console-error {
  color: rgba(255, 147, 143, 0.877) !important;
}

.console-line.console-warning {
  color: rgba(255, 236, 149, 0.788) !important;
}

.console-line.console-success {
  color: rgba(179, 255, 144, 0.788) !important;
}

.console-line.console-command {
  font-weight: bold;
  color: rgba(255, 255, 255) !important;
}
</style>