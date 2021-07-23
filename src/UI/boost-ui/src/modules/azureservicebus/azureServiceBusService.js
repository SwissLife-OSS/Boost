import apollo from "../../apollo";

import QUERY_QUEUES from "./graphql/GetQueues.gql";
import QUERY_TOPICS from "./graphql/GetTopics.gql";

export const getQueues = async () => {
    return await apollo.query({
        query: QUERY_QUEUES,
        variables: { }
    });
};

export const getTopics = async () => {
  return await apollo.query({
      query: QUERY_TOPICS,
      variables: { }
  });
};
