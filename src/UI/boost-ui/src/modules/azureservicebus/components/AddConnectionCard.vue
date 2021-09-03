<template>
  <div class="ma-4">
    <v-card>
      <v-toolbar color="grey lighten-2" elevation="0" height="44">
        <v-avatar size="30">
          <img alt="AzureDevOps" src="@/assets/azure_service_bus.png" />
        </v-avatar>
        <h4 class="ml-4">Add Azure Service Bus Connection</h4>
        <v-spacer> </v-spacer>
        <v-icon color="grey darken-3" @click="$router.back()">mdi-close</v-icon>
      </v-toolbar>
      <v-card-text>
        <v-form ref="form" v-model="valid" lazy-validation>
          <v-row dense>
            <v-col md="12"
              ><v-text-field
                v-model="config.name"
                :rules="config.nameRules"
                label="Name"
              ></v-text-field
            ></v-col>
          </v-row>
          <v-row dense>
            <v-col md="12"
              ><v-textarea
                v-model="config.connectionString"
                :rules="config.connectionStringRules"
                label="Connection string"
                rows="2"
              ></v-textarea
            ></v-col>
          </v-row>
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="primary" :disabled="!valid" @click="save">Save</v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script>
import { saveConnection } from "../azureServiceBusService";
import { FormRuleBuilder } from "../../../core/components/Common/FormRuleBuilder";

export default {
  data() {
    return {
      valid: false,
      config: {
        name: "",
        connectionString: "",
        nameRules: new FormRuleBuilder("Name", this).addRequired(2).build(),
        connectionStringRules: new FormRuleBuilder("ConnectionString", this)
          .addRequired(2)
          .build()
      }
    };
  },
  methods: {
    async save() {
      const isValid = this.$refs.form.validate();

      if (!isValid) {
        return;
      }

      const result = await saveConnection(
        this.config.name,
        this.config.connectionString
      );
      console.log(result);
      if (result.data.saveAzureServiceBusConnection.success) {
        this.$router.push({
          name: `AzureServiceBus`
        });
      }
    }
  }
};
</script>

<style></style>
