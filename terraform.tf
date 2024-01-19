variable APP_NAME {}
variable ENV_NAME {}

terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.88.0"
    }
  }

  backend "azurerm" {}
}

provider "azurerm" {
  features {}
}


# Create Resource Group
resource "azurerm_resource_group" "rg" {
  name     = "${var.APP_NAME}-${var.ENV_NAME}-rg"
  location = "westeurope"
}

# Create App Service Plan
resource "azurerm_service_plan" "serviceplan" {
  name                = "${var.APP_NAME}-serviceplan"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "B1"
}

# Create Web App
resource "azurerm_linux_web_app" "webapp2" {
  name                  = "${var.APP_NAME}-webapp"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.serviceplan.id
  https_only            = true
  site_config { 
    minimum_tls_version = "1.2"
  }
}