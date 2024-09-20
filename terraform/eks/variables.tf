variable "projectName" {
  default = "hackaton"
}

variable "vpcId" {
  default = "vpc-01d061ac263248f16"
}

variable "subnetA" {
  default = "subnet-0ba4865bd87a0fd6c"
}

variable "subnetB" {
  default = "subnet-0af6012c6aabbd024"
}

variable "subnetC" {
  default = "subnet-03cc8ba2fd4e6d96e"
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