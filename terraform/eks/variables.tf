variable "projectName" {
  default = "hackaton"
}

variable "vpcId" {
  default = "vpc-04cbc1c2f0b68f851"
}

variable "subnetA" {
  default = "subnet-05b2ab7a52c1cf5e0"
}

variable "subnetB" {
  default = "subnet-0490a4ee24017dff7"
}

variable "subnetC" {
  default = "subnet-0c80f5da0983fac75"
}

variable "labRole" {
  default = "arn:aws:iam::530823417496:role/LabRole"
}

variable "principalArn" {
  default = "arn:aws:iam::530823417496:role/voclabs"
}

variable "policyArn" {
  default = "arn:aws:eks::aws:cluster-access-policy/AmazonEKSClusterAdminPolicy"
}

variable "clusterVersion" {
  default = "1.27"
}

variable "accessConfig" {
  default = "API_AND_CONFIG_MAP"
}

variable "instanceType" {
  default = "t3.micro"
}