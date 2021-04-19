<template>
  <v-app>
    <v-system-bar window dark color="grey darken-4" height="42" app>
      <v-img
        max-height="26"
        max-width="26"
        src="@/assets/icon_boost.png"
      ></v-img>
      <span class="white--text ml-2">Boost Auth | {{ $route.name }}</span>
      <v-spacer></v-spacer>
      <v-btn icon @click="logout"><v-icon>mdi-logout</v-icon></v-btn>
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
</template>

<script>
export default {
  name: "App",
  components: {},
  data: () => ({
    dialog: false,
    navItems: [
      {
        text: "Session",
        icon: "mdi-account",
        route: "AuthSession",
      },
    ],
  }),
  computed: {
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
    logout: function () {
      window.location.href = "/logout";
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