# aspnet-webapp-deployed-to-azure

Experiment with deploying an asp.net web application to azure

# Application name

Create a repository variable `APP_NAME` with a short simple name of the application. It will be used as part of the name of Azure resources. It should only contain lowercase letters and numbers. No spaces, hyphens, underscores or other special charactors.

# Access to GitHub from GitHub workflows

The workflows need to be able to create and use secrets, variables and environments. Create a personal access token in GitHub with read and write permissions to "Administration", "Environment", "Secrets" and "Variables". Add it as a repository secret with the name `WORKFLOW_GITHUB_TOKEN`.

# Access to Azure from GitHub workflows

The workflows need to be able to manage resources in Azure. Create a service principal in Azure with the "Contributor" role on a subscription. Then create the following repository variables `WORKFLOW_AZURE_TENANT_ID`, `WORKFLOW_AZURE_SUBSCRIPTION_ID`, `WORKFLOW_AZURE_CLIENT_ID` and the repository secret `WORKFLOW_AZURE_CLIENT_SECRET`.
To create the user manually you need to create an app-registration and the add a client secret to it. Alternativly use the command: `az ad sp create-for-rbac --name <PRINCIPAL_NAME> --role Contributor --scopes /subscriptions/<SUBSCRIPTION_ID> --json-auth`

# Terraform backend storage

The workflows use terraform to manage Azure resources for the environments. Terraform needs storage where it maintains information about the resources it has created. Azure Blob storage is used for this. Run the workflow `Create Terraform backend storage` to create this in Azure.
