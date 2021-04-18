import { mapActions } from "vuex";

export default {
    props: ["id"],

    computed: {
        workRoots: function () {
            return this.$store.state.app.userSettings.workRoots;
        },
    },
    watch: {
        id: {
            immediate: true,
            handler: function (value) {
                if (value) {
                    this.loadService(value);
                }
            },
        },
    },
    methods: {
        ...mapActions("app", ["saveConnectedService"])
    }
}