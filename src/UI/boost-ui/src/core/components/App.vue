<template>
  <me-loader>
    <template slot="error">
      <v-app>
        <router-view name="root"></router-view>
      </v-app>
    </template>
    <v-app>
      <v-system-bar window dark color="grey darken-4" height="42" app>
        <v-img
          max-height="26"
          max-width="26"
          src="@/assets/icon_boost.png"
        ></v-img>
        <span class="white--text ml-2">Boost | {{ $route.name }}</span>
        <v-spacer></v-spacer>
        <div v-if="app" class="workdir" :title="app.workingDirectory">
          <v-icon class="mt-0" color="yellow">mdi-folder</v-icon>
          <div>{{ app.workingDirectory }}</div>
        </div>
        <v-spacer></v-spacer>
        <div v-if="statusMessage" :title="statusMessage.text">
          <v-icon color="red" v-if="statusMessage.type == 'ERROR'"
            >mdi-alert-circle</v-icon
          >
          <v-icon color="green" v-if="statusMessage.type == 'SUCCESS'"
            >mdi-check-circle</v-icon
          >
          <v-icon color="blue" v-if="statusMessage.type == 'INFO'"
            >mdi-information</v-icon
          >
          <v-icon color="yellow" v-if="statusMessage.type == 'WARINING'"
            >mdi-alert</v-icon
          >
        </div>
        <v-icon @click="dialog = !dialog">mdi-console</v-icon>
        <v-icon @click="onAccountClick">mdi-account</v-icon>
        <span @click="onAccountClick" v-if="me">{{ me.name }}</span>
      </v-system-bar>
      <v-navigation-drawer width="62" class="nav" app>
        <div
          class="nav-item"
          v-for="(nav, i) in navBarItems"
          :key="i"
          :title="nav.name"
          @click="onNavigate(nav)"
        >
          <v-icon class="icon" v-if="nav.icon" large light :color="nav.color">{{
            nav.icon
          }}</v-icon>

          <img v-if="nav.image" :src="nav.image" />
        </div>
      </v-navigation-drawer>
      <v-main>
        <router-view></router-view>
      </v-main>
    </v-app>
  </me-loader>
</template>

<script>
import { mapState } from "vuex";
import MeLoader from "./Common/MeLoader.vue";

export default {
  name: "App",
  components: { MeLoader },
  created() {
    this.$socket.start();

    this.$socket.on("consoleData", (data) => {
      if (data.message === null) {
        return;
      }

      this.$store.dispatch("workspace/addConsoleMessage", data);
    });
  },
  data: () => ({
    dialog: false,
    navItems: [
      {
        text: "Workspace",
        icon: "mdi-folder-outline",
        route: "Workspace",
      },
      {
        text: "Git",
        icon: "mdi-git",
        route: "Git",
      },
      {
        text: "Utils",
        icon: "mdi-hammer-screwdriver",
        route: "Utils",
      },
      {
        text: "Security",
        icon: "mdi-lock-check",
        route: "Security",
      },
      {
        text: "Snapshooter",
        route: "Snapshooter",
        icon: "mdi-test-tube",
        image2: require("../../assets/ss_logo.png"),
      },
      {
        text: "Settings",
        icon: "mdi-cog-outline",
        route: "Settings",
      },
      {
        text: "GraphQL",
        icon: "mdi-graphql",
        isServer: true,
        route: "graphql",
      },
    ],
  }),
  computed: {
    ...mapState("app", ["statusMessage", "app"]),
    me: function () {
      return null;
    },
    navBarItems: function () {
      return this.navItems.map((x) => {
        x.active = x.route === this.$route.name;
        x.color = x.active ? "#fff" : "#b3b3b3";

        return x;
      });
    },
  },
  methods: {
    onNavigate: function (nav) {
      if (nav.isServer) {
        window.location.href = "/" + nav.route;
      } else {
        if (!nav.active) this.$router.push({ name: nav.route });
      }
    },
    onAccountClick: function () {
      this.$router.push({ name: "Account" });
    },
  },
};
</script>

<style scoped>
.nav {
  background-color: rgba(0, 0, 0, 0.753) !important;
}

.nav-item {
  height: 60px;
  width: 60px;
  transition: 0.4s;
}

.nav-item img {
  width: 44px;
  height: 44px;
  margin-left: 8px;
  margin-top: 9px;
}

.icon {
  margin-left: 12px;
  margin-top: 12px;
}

.nav-item:hover {
  background-color: rgba(0, 0, 0, 0.79);
  border-radius: 40px;
}

.status-message {
  overflow: hidden;
}

.console {
  font-family: Lucida Console, Lucida Sans Typewriter, monaco,
    Bitstream Vera Sans Mono, monospace;

  font-size: 12px;
  height: 95%;
  color: rgba(233, 236, 236, 0.788) !important;
}

.workdir {
  background-color: #37474f;
  padding: 5px;
  border: #263238 solid 1px;
  border-radius: 12px;
}

.workdir div {
  width: 260px;
  display: inline-block;
  line-height: 14px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  color: #fff;
  margin-bottom: -4px;
  font-family: Lucida Console;
  font-size: 14px;
}
</style>

<style>
::-webkit-scrollbar {
  width: 6px;
  height: 6px;
}

::-webkit-scrollbar-track:hover {
  background: #fad5c8;
}
::-webkit-scrollbar-track:active {
  background: #dcdfe0;
}

::-webkit-scrollbar-track {
  background: #e3eaf0;
  border: 0px none #ffffff;
  border-radius: 50px;
}

::-webkit-scrollbar-thumb {
  background: #fad5c8;
  border: 0px none #ffffff;
  border-radius: 50px;
}
::-webkit-scrollbar-thumb:hover {
  background: #ffa686;
}
::-webkit-scrollbar-thumb:active {
  background: #ff7c4d;
}
</style>