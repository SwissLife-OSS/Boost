<template>
  <div>
    <MonacoEditor
      v-if="file.editor == 'Code'"
      class="editor"
      v-model="file.content"
      :language="file.meta.language"
      :style="{ height: height + 'px' }"
    />

    <div v-if="file.editor == 'Image'">
      <v-img contain :max-height="height" :src="file.meta.getUrl"></v-img>
    </div>

    <div v-if="file.editor == 'Pdf'">
      <v-toolbar color="grey lighten-2" height="38" elevation="0">
        <v-toolbar-title
          >Page {{ pdf.currentPage }} of {{ pdf.pageCount }}</v-toolbar-title
        >
        <v-spacer></v-spacer>
        <v-btn
          small
          class="mx-1"
          icon
          :disabled="pdf.currentPage === 1"
          @click="onNavigate(-1)"
          ><v-icon>mdi-arrow-left-circle</v-icon></v-btn
        >
        <v-btn
          small
          class="mx-1"
          icon
          :disabled="pdf.currentPage === pdf.pageCount"
          @click="onNavigate(+1)"
          ><v-icon>mdi-arrow-right-circle</v-icon></v-btn
        >
      </v-toolbar>
      <div
        :style="{
          'max-height': height - 60 + 'px',
          height: height - 60 + 'px',
        }"
        class="overflow-y-auto mt-0"
      >
        <pdf
          :src="file.meta.getUrl"
          :page="pdf.currentPage"
          @num-pages="pdf.pageCount = $event"
        ></pdf>
      </div>
    </div>
  </div>
</template>

<script>
import MonacoEditor from "../Common/MonacoEditor";
import pdf from "vue-pdf";

export default {
  components: {
    MonacoEditor,
    pdf,
  },
  props: ["file", "height"],
  data() {
    return {
      pdf: {
        pageCount: null,
        currentPage: 1,
      },
    };
  },
  methods: {
    onNavigate: function (step) {
      this.pdf.currentPage = this.pdf.currentPage + step;
    },
  },
};
</script>

<style scoped>
.editor {
  width: 100%;
}
</style>