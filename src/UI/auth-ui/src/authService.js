import apollo from "./apollo";

import QUERY_AUTH_SESSION from "./GraphQL/GetAuthSession.gql"

export const getAuthSession = async () => {
    return await apollo.query({
        query: QUERY_AUTH_SESSION,
        variables: {}
    });
};

