import apollo from "../apollo";

import MUTATION_CONNECTEDSERVICE_SAVE from "./graphql/Settings/SaveConnectedService.gql";
import QUERY_CONNECTEDSERVICES_GET from "./graphql/Settings/ConnectedServices.gql";
import QUERY_CONNECTEDSERVICE_GET from "./graphql/Settings/GetConnectedService.gql";
import MUTATION_WORKSOOTS_SAVE from "./graphql/Settings/SaveWorkRoots.gql";


export const saveConnectedService = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_CONNECTEDSERVICE_SAVE,
        variables: { input }
    });
};

export const getConnectedServices = async () => {
    return await apollo.query({
        query: QUERY_CONNECTEDSERVICES_GET,
        variables: {}
    });
};

export const getConnectedService = async (id) => {
    return await apollo.query({
        query: QUERY_CONNECTEDSERVICE_GET,
        variables: { id }
    });
};

export const saveWorkRoots = async (input) => {
    return await apollo.mutate({
        mutation: MUTATION_WORKSOOTS_SAVE,
        variables: { input }
    });
};

export const mapService = (service, data) => {

    service.name = data.name;
    service.id = data.id;
    service.defaultWorkRoot = data.defaultWorkRoot;

    for (let i = 0; i < data.properties.length; i++) {
        const prop = data.properties[i];

        var name =
            prop.name.substring(0, 1).toLowerCase() + prop.name.substring(1);

        if (Object.prototype.hasOwnProperty.call(service, name)) {
            service[name] = prop.value;
        }
    }
}

export const getServicePropertyValue = (service, name) => {

    const prop = service.properties.find(x => x.name === name);
    if (prop) {
        return prop.value
    }

    return null;

}