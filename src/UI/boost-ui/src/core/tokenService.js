import apollo from "../apollo";

import QUERY_TOKEN_ANALYZE from "./graphql/Security/AnalyzeToken.gql";
import QUERY_USERINFO_GET from "./graphql/Security/GetUserInfoClaims.gql";

export const analyzeToken = async (token) => {
    return await apollo.query({
        query: QUERY_TOKEN_ANALYZE,
        variables: { token }
    });
};

export const getUserInfo = async (token) => {
    return await apollo.query({
        query: QUERY_USERINFO_GET,
        variables: { token }
    });
};
