import apollo from "../apollo";

import QUERY_INITIAL_DATA from "./graphql/InitialData.gql";

export const getInitialData = async () => {
    return await apollo.query({
        query: QUERY_INITIAL_DATA,
        variables: {}
    });
};
