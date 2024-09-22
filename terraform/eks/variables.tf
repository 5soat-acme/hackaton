variable "projectName" {
  default = "hackaton"
}

variable "vpcId" {
  default = "vpc-095f2bd0fd2444f32"
}

variable "subnetA" {
  default = "subnet-013e0a84d3544eb9c"
}

variable "subnetB" {
  default = "subnet-07eb1c186954756d8"
}

variable "subnetC" {
  default = "subnet-0c4aee3cd0b353c1c"
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