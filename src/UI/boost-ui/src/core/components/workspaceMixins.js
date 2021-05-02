
import { openFile, getQuickActions, openInCode, openInExplorer, openInTerminal, runSuperBoost } from "../workspaceService";
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
                directory
            })
        },
        async openFileInCode(file) {
            await openInCode({
                file
            })
        },
        async openInExplorer(directory) {
            await openInExplorer(directory);
        },
        async openInTerminal(directory) {
            await openInTerminal(directory);
        },
        async runSuperBoost(input) {
            await runSuperBoost(input);
        },
        async loadQuickActions(directory) {
            const result = await getQuickActions(directory);

            return result.data.quickActions;
        },
    }
}