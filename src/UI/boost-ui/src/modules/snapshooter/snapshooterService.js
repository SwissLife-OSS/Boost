import apollo from "../../apollo";

import QUERY_SNAPSHOOTER_DIR_GET from "./graphql/GetDirectories.gql";
import QUERY_SNAPSHOOTER_CONTENT_GET from "./graphql/GetContent.gql";
import MUTATION_APPROVE from "./graphql/ApproveSnapshot.gql";
import MUTATION_APPROVE_ALL from "./graphql/ApproveAll.gql";

export const getsnapshooterDirectories = async (withMismatchOnly) => {
    return await apollo.query({
        query: QUERY_SNAPSHOOTER_DIR_GET,
        variables: { withMismatchOnly }
    });
};

export const getContent = async (input) => {
    return await apollo.query({
        query: QUERY_SNAPSHOOTER_CONTENT_GET,
        variables: { input }
    });
};

export const approve = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_APPROVE,
        variables: { input }
    });
};

export const approveAll = async () => {
    return await apollo.mutate({
        mutation: MUTATION_APPROVE_ALL,
        variables: {}
    });
};