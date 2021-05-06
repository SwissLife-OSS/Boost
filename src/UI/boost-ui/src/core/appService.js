import apollo from "../apollo";

import QUERY_INITIAL_DATA from "./graphql/InitialData.gql";
import QUERY_VERSION_GET from "./graphql/App/GetVersion.gql";

export const getInitialData = async () => {
    return await apollo.query({
        query: QUERY_INITIAL_DATA,
        variables: {}
    });
};

export const getVersion = async () => {
    return await apollo.query({
        query: QUERY_VERSION_GET,
        variables: {}
    });
};
