# aspnet-webapp-deployed-to-azure

Experiment with deploying an asp.net web application to azure

# Application name

Create a repository variable `APP_NAME` with a short simple name of the application. It will be used as part of the name of Azure resources. It should only contain lowercase letters and numbers. No spaces, hyphens, underscores or other special charactors. And be no longer than 10 charators long.

# Access to GitHub from GitHub workflows

The workflows need to be able to create and use secrets, variables and environments. Create a personal access token in GitHub with read and write permissions to "Administration", "Environment", "Secrets" and "Variables". Add it as a repository secret with the name `WORKFLOW_GITHUB_TOKEN`.

# Access to Azure from GitHub workflows

The workflows need to be able to manage resources in Azure. Create a service principal in Azure with the "Contributor" role on a subscription. Then create the following repository variables `WORKFLOW_AZURE_TENANT_ID`, `WORKFLOW_AZURE_SUBSCRIPTION_ID`, `WORKFLOW_AZURE_CLIENT_ID` and the repository secret `WORKFLOW_AZURE_CLIENT_SECRET`.
To create the user manually you need to create an app-registration and then add a client secret to it. Alternativly use the command: `az ad sp create-for-rbac --name <PRINCIPAL_NAME> --role Contributor --scopes /subscriptions/<SUBSCRIPTION_ID> --json-auth`. The command creates both the app-registration and the client secret.

# Terraform backend storage

Terraform is used to manage the Azure resources for the environments. Terraform needs storage where it maintains information about the resources it has created. Azure Blob storage is used for this. Run the workflow `Create Terraform backend storage` to create this in Azure.

# Create environment

The GitHub workflow `Create environment` can be used to create a new environment. It will create an environment i GitHub used for environment specific variables and secrets. It then calls the `Update environment` workflow to create a resource group in Azure with the resources required to run the application. The workflow takes an environment name that must be short and without spaces or special charcators. The name is used in some of the Azure resource names.

Note that this does not deploy the application.

# Update environment

The file `terraform.tf` describes the Azure resources needed in an environment to run the application.

If this is changed you can run the GitHub workflow `Update environment` on existing environments to update them.

# Delete environment

The GitHub workflow `Delete environment` deletes the Azure resource group for the environment and it deletes the environment in GitHub.

# Test

The GitHub workflow `Test` builds the code and runs automated tests. It is automatically run on pull-requests and when code is committed to the `main` and `release/*` branches.

# Deploy snapshot

The GitHub workflow `Deploy snapshot` builds and deploys the code to an environment. It is executed manually with the environment as input.

# Prepare release

The GitHub workflow `Prepare release` creates a new release branch from the `main` branch named after the major and minor parts of the version number (like `release/v1.0`). A draft-release is created in GitHub associated with the new release branch.

On the `main` branch the minor part of the version number is incremented to indicate it is now used for the next release. If the next release is a new major release, you must update the version number manually afterwards.
