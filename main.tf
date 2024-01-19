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


resource "azurerm_resource_group" "main" {
  name     = "${var.APP_NAME}-${var.ENV_NAME}-rg"
  location = "westeurope"
}