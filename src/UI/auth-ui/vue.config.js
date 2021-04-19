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
  outputDir: path.resolve(__dirname, "../../Tool/src/Boost.Tool/AuthUI"),
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
      }
    }
  }
}