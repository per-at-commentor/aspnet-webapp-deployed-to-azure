# aspnet-webapp-deployed-to-azure

Experiment with deploying an ASP.NET web application to Azure using GitHub workflows.

# Application name

Create a repository variable `APP_NAME` with a short simple name of the application. It will be used as part of the name of Azure resources. It should only contain lowercase letters and numbers. No spaces, hyphens, underscores or other special charactors. And be no longer than 10 charators long.

# Access to GitHub from GitHub workflows

The workflows need to be able to create and use secrets, variables and environments. Create a personal access token in GitHub (profile settings -> developer settings) with read and write permissions to "Administration", "Contents", "Environment", "Secrets" and "Variables". Add it as a repository secret with the name `WORKFLOW_GITHUB_TOKEN`.

# Access to Azure from GitHub workflows

The workflows need to be able to manage resources in Azure. Create a service principal in Azure with the "Contributor" role on a subscription. Then create the following repository variables `WORKFLOW_AZURE_TENANT_ID`, `WORKFLOW_AZURE_SUBSCRIPTION_ID`, `WORKFLOW_AZURE_CLIENT_ID` and the repository secret `WORKFLOW_AZURE_CLIENT_SECRET`.
To create the user manually you need to create an app-registration and then add a client secret to it. Alternativly use the command: `az ad sp create-for-rbac --name <PRINCIPAL_NAME> --role Contributor --scopes /subscriptions/<SUBSCRIPTION_ID> --json-auth`. The command creates both the app-registration and the client secret.

# SonarCloud integration

The workflows use SonarCloud for static code analysis. This requires the GitHub repository to be imported to SonarCloud as a project. This can be done by logging into sonarcloud.io using your GitHub account and then you will be able to import the repository as a project. Once imported you can go to the "Information" page for the project in SonarCloud and get the "Project Key" and "Organization Key". These keys must be add to GitHub as repository variables `SONAR_PROJECT_KEY` and `SONAR_ORGANIZATION_KEY`. You will also need to generate a token in SonarCloud. Go to the "My Account" page and generate a token on the "Security" tab. The generated token must be added to GitHub as a repository variable named `SONAR_TOKEN`.

# Workflows

The workflows can be run from the repository in GitHub.

## Terraform backend storage

Terraform is used to manage the Azure resources for the environments. Terraform needs storage where it maintains information about the resources it has created. Azure Blob storage is used for this. Run the workflow `Create Terraform backend storage` to create this in Azure.

## Create environment

The workflow `Create environment` can be used to create a new environment. It will create an environment i GitHub used for environment specific variables and secrets. It then calls the `Update environment` workflow to create a resource group in Azure with the resources required to run the application. The workflow takes an environment name that must be short and without spaces or special charcators. The name is used in some of the Azure resource names.

Note that this does not deploy the application.

## Update environment

The file `terraform.tf` describes the Azure resources needed in an environment to run the application.

If this is changed you can run the workflow `Update environment` on existing environments to update them.
To review the plannend changes before applying then, configure the environment in GitHub to require approval. This will block the workflow after the Terraform planning-phase allowing you to inspect the plan before applying it. 

## Delete environment

The workflow `Delete environment` deletes the Azure resource group for the environment and it deletes the environment in GitHub.

## Test

The workflow `Test` builds the code and runs automated tests. It also uses SonarCloud to perform static code analisys and monitor code coverage. The workflow is automatically run on pull-requests and when code is committed to the `main` and `release/*` branches. It can be run manually on a given branch.

## Deploy branch snapshot

The workflow `Deploy branch snapshot` builds and deploys the code from a branch to an environment. It is executed manually with the environment as input. If the environment is configured in GitHub to require approval the workflow will not run until approval is given.

## Release

The branching strategy is to have development of the next major/minor version (like 1.2.0, 1.3.0 and 1.4.0) on the `main` branch. When a release is made, a release branch created (named like release/1.2), containing the various patch versions (like 1.2.0, 1.2.1 and 1.2.2) of the release.

The workflow `Release` calls `Prepare release`, `Build release` and `Publish release`. It can be used on both the `main` branch and on release branches. The called workflows can also be used individually if you need to test the build release before publishing it.

## Prepare release

The workflow `Prepare release` creates a draft-release in GitHub.

If the workflow is run on the `main` branch it creates a new release branch named after the major and minor parts of the version number (like `release/1.2`). On the `main` branch the minor part of the version number is incremented to indicate it is now used for the next release. If the next release is a new major release, the version number must be updated manually afterwards.

The workflow can also be run on a previously a release branch that have previously been published.

## Build release

The workflow `Build release` should only be used on release branches. It builds the application and uploads it to the corresponding release in GitHub (previously created with `Prepare release`).

## Publish release

The workflow `Publish release` should only be used on release branches. It changes the corresponding release in GitHub (previously created with `Prepare release`) from draft to published. It also increments the patch version in the release-branch to indicate that futher development on the branch is for the next patch release.

## Deploy release

The workflow `Deploy release` whould only be used on release-tags. It will download the relase artifacts from the GitHub release matching the given tag and deploy them to the given environment.
