variable "projectName" {
  default = "hackaton"
}

variable "vpcId" {
  default = "vpc-0c4d2b5d33bc142d3"
}

variable "subnetA" {
  default = "subnet-03e64d3f65b93042c"
}

variable "subnetB" {
  default = "subnet-04cffe7f74e36a000"
}

variable "subnetC" {
  default = "subnet-07e1ae53fe8679232"
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