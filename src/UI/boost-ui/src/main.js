import Vue from 'vue'
import App from './core/components/App.vue'
import vuetify from './plugins/vuetify';
import store from './store'
import router from './router'
import { DateTime } from "luxon";
import signalrHub from "./signalrHub";
import Clipboard from 'v-clipboard'
import VueSimpleMarkdown from 'vue-simple-markdown'
import 'vue-simple-markdown/dist/vue-simple-markdown.css'

Vue.use(VueSimpleMarkdown)

Vue.config.productionTip = false

Vue.filter("dateformat", function (value, format = "DATETIME_SHORT") {
  if (!value) return "";

  var date = DateTime.fromISO(value);
  if (DateTime.isDateTime(date)) {
    return date.toLocaleString(DateTime[format]);
  }
  return "";
});

Vue.use(signalrHub);
Vue.use(Clipboard)

new Vue({
  vuetify,
  store,
  router,
  render: h => h(App)
}).$mount('#app')
