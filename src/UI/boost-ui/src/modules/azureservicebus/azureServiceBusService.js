import apollo from "../../apollo";
import MUTATION_DELETECONNECTION from "./graphql/DeleteConnection.gql";
import QUERY_QUEUES from "./graphql/GetQueues.gql";
import QUERY_SETTINGS from "./graphql/GetSettings.gql";
import QUERY_TOPICS from "./graphql/GetTopics.gql";
import MUTATION_SAVECONNECTION from "./graphql/SaveConnection.gql";

export const getQueues = async (connectionName) => {
    return await apollo.query({
        query: QUERY_QUEUES,
        variables: { connectionName}
    });
};

export const getTopics = async (connectionName) => {
  return await apollo.query({
      query: QUERY_TOPICS,
      variables: { connectionName}
  });
};

export const deleteConnection = async (connectionName) => {
  return await apollo.mutate({
    mutation: MUTATION_DELETECONNECTION,
      variables: { connectionName }
  });
};

export const getSettings = async () => {
  return await apollo.query({
      query: QUERY_SETTINGS,
      variables: { }
  });
};

export const saveConnection = async (connectionName, connectionString) => {
  return await apollo.mutate({
      mutation: MUTATION_SAVECONNECTION,
      variables: { input:{
        name: connectionName,
        connectionString
      } }
  });
};
