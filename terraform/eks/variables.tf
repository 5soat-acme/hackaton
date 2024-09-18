variable "projectName" {
  default = "hackaton"
}

variable "vpcId" {
  default = "vpc-06faf4497df67a50d"
}

variable "subnetA" {
  default = "subnet-09867afb00039f431"
}

variable "subnetB" {
  default = "subnet-0359e9c0d10bf089e"
}

variable "subnetC" {
  default = "subnet-0f747a87e9f9b0759"
}

variable "labRole" {
  default = "arn:aws:iam::412140125864:role/LabRole"
}

variable "principalArn" {
  default = "arn:aws:iam::412140125864:role/voclabs"
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