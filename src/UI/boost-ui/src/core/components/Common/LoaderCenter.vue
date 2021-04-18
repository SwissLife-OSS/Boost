<template>
  <div style="width: 100%; height: 60vh">
    <v-container fill-height fluid>
      <v-row align="center" justify="center">
        <v-col class="d-flex justify-center">
          <div class="text-center">
            <v-row>
              <v-col>
                <v-img max-width="450" src="@/assets/logo_boost.png" />
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-progress-linear
                  buffer-value="0"
                  stream
                  reverse
                  color="grey darken-3"
                ></v-progress-linear>
              </v-col>
            </v-row>
            <v-row>
              <v-subheader class="text-center" style="margin: auto"
                >loading boost</v-subheader
              >
            </v-row>
            <br />
          </div>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script>
export default {};
</script>

<style>
</style>