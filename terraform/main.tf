
module "database" {
  source = "./database"

  projectName       = var.projectName
  password          = var.password
  availabilityZoneA = var.availabilityZoneA
  availabilityZoneB = var.availabilityZoneB
  vpcId             = var.vpcId
  vpcCidrBlocks     = var.vpcCidrBlocks
}

module "eks" {
  source = "./eks"
  
  projectName    = var.projectName
  vpcId          = var.vpcId
  subnetA        = var.subnetA
  subnetB        = var.subnetB
  subnetC        = var.subnetC
  labRole        = var.labRole
  principalArn   = var.principalArn
  policyArn      = var.policyArn
  clusterVersion = var.clusterVersion
  accessConfig   = var.accessConfig
  instanceType   = var.instanceType
}