import apollo from "../apollo";

import QUERY_ENCODE from "./graphql/Utils/Encode.gql";
import QUERY_DECODE from "./graphql/Utils/Decode.gql";
import QUERY_GUID_CREATE from "./graphql/Utils/CreateGuids.gql";
import QUERY_GUID_PARSE from "./graphql/Utils/ParseGuid.gql";
import QUERY_HASH_CREATE from "./graphql/Utils/CreateHash.gql";

export const encode = async (value, type) => {
    return await apollo.query({
        query: QUERY_ENCODE,
        variables: { value, type }
    });
};

export const decode = async (value, type) => {
    return await apollo.query({
        query: QUERY_DECODE,
        variables: { value, type }
    });
};

export const createGuids = async (count, format) => {
    return await apollo.query({
        query: QUERY_GUID_CREATE,
        variables: { count, format }
    });
};

export const parseGuid = async (value) => {
    return await apollo.query({
        query: QUERY_GUID_PARSE,
        variables: { value }
    });
};

export const createHash = async (input) => {
    return await apollo.query({
        query: QUERY_HASH_CREATE,
        variables: { input }
    });
};
