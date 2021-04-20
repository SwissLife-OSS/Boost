import apollo from "../apollo";

import QUERY_TOKEN_ANALYZE from "./graphql/Security/AnalyzeToken.gql";
import QUERY_USERINFO_GET from "./graphql/Security/GetUserInfoClaims.gql";
import MUTATION_AUTHORIZE_START from "./graphql/Security/StartAuthorize.gql";
import MUTATION_AUTHSERVER_STOP from "./graphql/Security/StopAuthServer.gql";
import MUTATION_TOKEN_REQUEST from "./graphql/Security/RequestToken.gql";
import MUTATION_IDENTITY_REQUEST_SAVE from "./graphql/Security/SaveIdentityRequest.gql";
import MUTATION_IDENTITY_REQUEST_DELETE from "./graphql/Security/DeleteIdentityRequest.gql";

import QUERY_IDENTITY_REQUESTS_SEARCH from "./graphql/Security/SearchIdentityRequests.gql";
import QUERY_IDENTITY_REQUEST_GET from "./graphql/Security/GetIdentityRequest.gql";


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

export const startAuthorize = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_AUTHORIZE_START,
        variables: { input }
    });
};

export const stopAuthServer = async (id) => {
    return await apollo.mutate({
        mutation: MUTATION_AUTHSERVER_STOP,
        variables: { id }
    });
};

export const requestToken = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_TOKEN_REQUEST,
        variables: { input }
    });
};

export const saveRequest = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_IDENTITY_REQUEST_SAVE,
        variables: { input }
    });
};

export const searchIdentityRequests = async (input) => {
    return await apollo.query({
        query: QUERY_IDENTITY_REQUESTS_SEARCH,
        variables: { input }
    });
};

export const getIdentityRequest = async (id) => {
    return await apollo.query({
        query: QUERY_IDENTITY_REQUEST_GET,
        variables: { id }
    });
};

export const deleteRequest = async (id) => {
    return await apollo.mutate({
        mutation: MUTATION_IDENTITY_REQUEST_DELETE,
        variables: { id }
    });
};




