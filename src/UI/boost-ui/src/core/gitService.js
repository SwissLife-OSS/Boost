import apollo from "../apollo";

import QUERY_REPOS_SEARCH from "./graphql/Git/SearchRepos.gql"
import QUERY_REPOS_LOCAL_SEARCH from "./graphql/Git/SearchLocalRepos.gql"
import QUERY_REPO_REMOTE_GET from "./graphql/Git/GetRemoteRepository.gql"
import QUERY_REPO_LOCAL_GET from "./graphql/Git/GetLocalRepo.gql"
import MUTATION_CLONE from "./graphql/Git/Clone.gql"

export const searchRepos = async (input) => {
    return await apollo.query({
        query: QUERY_REPOS_SEARCH,
        variables: { input }
    });
};

export const getLocalRepo = async (id) => {
    return await apollo.query({
        query: QUERY_REPO_LOCAL_GET,
        variables: { id }
    });
};


export const searchLocalRepos = async (input) => {
    return await apollo.query({
        query: QUERY_REPOS_LOCAL_SEARCH,
        variables: { input }
    });
};

export const getRemoteRepo = async (input) => {
    return await apollo.query({
        query: QUERY_REPO_REMOTE_GET,
        variables: { input }
    });
};

export const cloneRepo = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_CLONE,
        variables: { input }
    });
};