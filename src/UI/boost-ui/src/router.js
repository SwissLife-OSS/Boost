import Vue from "vue";
import VueRouter from "vue-router";

import GitLocalRepoPage from "./core/components/Git/GitLocalRepoPage"
import GitPage from "./core/components/Git/GitPage"
import GitRemoteRepoPage from "./core/components/Git/GitRemoteRepoPage"
import IndexLocalPage from "./core/components/Git/IndexLocalPage"
import LocalRepositoriesPage from "./core/components/Git/LocalRepositoriesPage"
import RepositoriesPage from "./core/components/Git/RepositoriesPage"
import InfoPage from "./core/components/Info/InfoPage"
import AuthorizePage from "./core/components/Security/AuthorizePage"
import SecurityPage from "./core/components/Security/SecurityPage"
import TokenRequestPage from "./core/components/Security/TokenRequestPage"
import TokensPage from "./core/components/Security/TokensPage"
import ConfigureAzureDevOpsPage from "./core/components/Settings/ConfigureAzureDevOpsPage"
import ConfigureGitHubPage from "./core/components/Settings/ConfigureGitHubPage"
import GeneralSettingsPage from "./core/components/Settings/GeneralSettingsPage"
import SecuritySettingsPage from "./core/components/Settings/SecuritySettingsPage"
import SettingsPage from "./core/components/Settings/SettingsPage"
import Base64ToFilePage from "./core/components/Utils/Base64ToFilePage"
import EncodingPage from "./core/components/Utils/EncodingPage"
import SecurityUtilsPage from "./core/components/Utils/SecurityUtilsPage"
import UtilsPage from "./core/components/Utils/UtilsPage"
import WorkspacePage from "./core/components/Workspace/WorkspacePage"
import AddConnectionCard from "./modules/azureservicebus/components/AddConnectionCard"
import AzureServiceBusPage from "./modules/azureservicebus/components/AzureServiceBusPage"
import ViewAzureServiceBus from "./modules/azureservicebus/components/ViewAzureServiceBus"
import SnapshooterPage from "./modules/snapshooter/components/SnapshooterPage"

// Avoid Redundant route exception
const originalPush = VueRouter.prototype.push;
VueRouter.prototype.push = function push(location) {
  return originalPush.call(this, location).catch(err => err);
};

Vue.use(VueRouter);

const routes = [
  {
    path: "/info",
    name: "Info",
    component: InfoPage
  },
  {
    path: "/utils",
    name: "Utils",
    component: UtilsPage,
    children: [
      {
        path: "/encoding",
        name: "Utils.Encoding",
        component: EncodingPage
      },
      {
        path: "/base64tofile",
        name: "Utils.Base64ToFile",
        component: Base64ToFilePage
      },
      {
        path: "/security",
        name: "Utils.Security",
        component: SecurityUtilsPage
      },
    ]
  },
  {
    path: "/",
    name: "Workspace",
    component: WorkspacePage
  },
  {
    path: "/git",
    name: "Git",
    component: GitPage,
    children: [
      {
        path: "/git/remote",
        name: "Git.Remote",
        component: RepositoriesPage,
        props: true,
        children: [
          {
            path: "/git/remote/:serviceId/:id",
            name: "Git.Repo.Details",
            component: GitRemoteRepoPage,
            props: true
          },
        ]
      },
      {
        path: "/git/local",
        name: "Git.Local",
        component: LocalRepositoriesPage,
        props: true,
        children: [
          {
            path: "/git/local/:id",
            name: "Git.Repo.LocalDetails",
            component: GitLocalRepoPage,
            props: true
          },
        ]
      },
      {
        path: "/git/current",
        name: "Git.Current",
        component: GitLocalRepoPage,
      },
      {
        path: "/git/index",
        name: "Git.Index",
        component: IndexLocalPage,
      }
    ]
  },
  {
    path: "/security",
    name: "Security",
    component: SecurityPage,
    children: [
      {
        path: "security/tokens",
        name: "Security.Tokens",
        component: TokensPage,
      },
      {
        path: "security/authorize",
        name: "Security.Authorize",
        component: AuthorizePage,
      },
      {
        path: "security/tokenrequest",
        name: "Security.TokenRequest",
        component: TokenRequestPage,
      },
    ]
  },
  {
    path: "/settings",
    name: "Settings",
    component: SettingsPage,
    children: [
      {
        path: "general",
        name: "Settings.General",
        component: GeneralSettingsPage
      },
      {
        path: "security",
        name: "Settings.Security",
        component: SecuritySettingsPage
      }
    ]
  },
  {
    path: "/settings/ado",
    name: "Settings.AzureDevOps",
    component: ConfigureAzureDevOpsPage
  },
  {
    path: "/settings/ado/:id",
    name: "Settings.AzureDevOps.Edit",
    component: ConfigureAzureDevOpsPage,
    props: true
  },
  {
    path: "/settings/github",
    name: "Settings.GitHub",
    component: ConfigureGitHubPage
  },
  {
    path: "/settings/github/:id",
    name: "Settings.GitHub.Edit",
    component: ConfigureGitHubPage,
    props: true
  },
  {
    path: "/snap",
    name: "Snapshooter",
    component: SnapshooterPage
  },
  {
    path: "/azureservicebus",
    name: "AzureServiceBus",
    component: AzureServiceBusPage,
  },
  {
    path: "/azureservicebus/view/:connectionName",
    name: "AzureServiceBus.View",
    component: ViewAzureServiceBus,
    props: true
  },
  {
    path: "/azureservicebus/add",
    name: "AzureServiceBus.Add",
    component: AddConnectionCard
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

export default router;
