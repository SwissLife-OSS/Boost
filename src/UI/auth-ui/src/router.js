
import Vue from "vue";
import VueRouter from "vue-router";
import AuthSessionPage from "./components/AuthSessionPage";

Vue.use(VueRouter);


const routes = [
  {
    path: "/",
    name: "Session",
    component: AuthSessionPage
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

export default router;
