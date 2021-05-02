import apollo from "../apollo";

import MUTATION_EXEC_COMMAND from "./graphql/Workspace/ExecuteCommand.gql";
import MUTATION_CODE_OPEN from "./graphql/Workspace/OpenInCode.gql";
import MUTATION_FILE_OPEN from "./graphql/Workspace/OpenFile.gql";
import MUTATION_EXPLORER_OPEN from "./graphql/Workspace/OpenInExplorer.gql";
import MUTATION_TERMINAL_OPEN from "./graphql/Workspace/OpenInTerminal.gql";
import MUTATION_SUPERBOOST_RUN from "./graphql/Workspace/RunSuperBoost.gql";
import MUTATION_EXECUTE_FILE_ACTION from "./graphql/Workspace/ExecuteFileAction.gql";
import MUTATION_FILE_CREATE_FROMBASE64 from "./graphql/Workspace/CreateFileFromBase64.gql";
import QUERY_WORKSPACE_GET from "./graphql/Workspace/GetWorkspace.gql";
import QUERY_WORKSPACE_FILE_GET from "./graphql/Workspace/GetFile.gql";
import QUERY_WORKSPACE_DIR_CHILDREN from "./graphql/Workspace/GetDirectoryChildren.gql";
import QUERY_QUICKACTIONS_GET from "./graphql/Workspace/GetQuickActions.gql";

export const executeCommand = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_EXEC_COMMAND,
        variables: { input }
    });
};

export const executeFileAction = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_EXECUTE_FILE_ACTION,
        variables: { input }
    });
};

export const openInCode = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_CODE_OPEN,
        variables: { input }
    });
};

export const openFile = async (fileName) => {
    return await apollo.mutate({
        mutation: MUTATION_FILE_OPEN,
        variables: { fileName }
    });
};

export const openInExplorer = async (directory) => {
    return await apollo.mutate({
        mutation: MUTATION_EXPLORER_OPEN,
        variables: { directory }
    });
};

export const openInTerminal = async (directory) => {
    return await apollo.mutate({
        mutation: MUTATION_TERMINAL_OPEN,
        variables: { directory }
    });
};

export const runSuperBoost = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_SUPERBOOST_RUN,
        variables: { input }
    });
};

export const createFileFromBase64 = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_FILE_CREATE_FROMBASE64,
        variables: { input }
    });
};

export const getWorkspace = async () => {
    return await apollo.query({
        query: QUERY_WORKSPACE_GET,
        variables: {}
    });
};

export const getDirectoryChildren = async (path) => {
    return await apollo.query({
        query: QUERY_WORKSPACE_DIR_CHILDREN,
        variables: { path }
    });
};

export const getFile = async (input) => {
    return await apollo.query({
        query: QUERY_WORKSPACE_FILE_GET,
        variables: { input }
    });
};

export const getQuickActions = async (directory) => {
    return await apollo.query({
        query: QUERY_QUICKACTIONS_GET,
        variables: { directory }
    });
};