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
  </div>
</template>

<script>
import MonacoEditor from "vue-monaco";
export default {
  components: {
    MonacoEditor,
  },
  props: ["file", "height"],
};
</script>

<style scoped>
.editor {
  width: 100%;
}
</style>