const MonacoWebpackPlugin = require('monaco-editor-webpack-plugin')
const path = require("path");

module.exports = {
  "transpileDependencies": [
    "vuetify"
  ],
  pluginOptions: {
    apollo: {
      lintGQL: false
    }
  },
  chainWebpack: config => {
    config.plugin('monaco-editor').use(MonacoWebpackPlugin, [
      {
        //https://github.com/microsoft/monaco-editor-webpack-plugin
        languages: ['csharp', 'javascript', 'css', 'html', 'typescript', 'json', 'graphql', 'sql', 'yaml', 'powershell', 'markdown'],
      },
    ])
  },
  outputDir: path.resolve(__dirname, "../../Tool/src/Boost.Tool/UI"),
  configureWebpack: {
    devtool: 'source-map'
  },
  devServer: {
    proxy: {
      "/graphql": {
        ws: true,
        changeOrigin: true,
        target: process.env.API_BASE_URL,
        headers: {
          "Authorization": "dev " + process.env.DEV_USER,
        }
      },
      "/api": {
        ws: false,
        changeOrigin: true,
        target: process.env.API_BASE_URL,
        headers: {
          "Authorization": "dev " + process.env.DEV_USER,
        }
      },
      "/signal": {
        ws: true,
        changeOrigin: true,
        target: process.env.API_BASE_URL,
        headers: {
          "Authorization": "dev " + process.env.DEV_USER,
        }
      }
    }
  }
}