import apollo from "../apollo";

import QUERY_PIPELINES_BYREPO from "./graphql/Pipelines/GetPipelinesByRepo.gql"

export const getPipelinesByRepo = async (input) => {
    return await apollo.query({
        query: QUERY_PIPELINES_BYREPO,
        variables: { input }
    });
};
