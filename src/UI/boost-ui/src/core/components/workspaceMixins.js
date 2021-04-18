
import { openFile, getQuickActions, openInCode } from "../workspaceService";
export default {

    computed: {
        workingDirectory: function () {
            return this.$store.state.app.workingDirectory;
        },
    },

    methods: {

        async openFile(filename) {
            await openFile(filename)
        },
        async openDirectoryInCode(directory) {
            await openInCode({
                directory: directory
            })
        },
        async openFileInCode(file) {
            await openInCode({
                file: file
            })
        },
        async loadQuickActions(directory) {
            const result = await getQuickActions(directory);

            return result.data.quickActions;
        },
    }
}