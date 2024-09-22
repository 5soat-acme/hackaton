variable "projectName" {
  default = "hackaton"
}

variable "vpcId" {
  default = "vpc-0a4cd7ed4fdd511a5"
}

variable "subnetA" {
  default = "subnet-0958a3a4bf1124a21"
}

variable "subnetB" {
  default = "subnet-0217d3d68bfbcd9e3"
}

variable "subnetC" {
  default = "subnet-0950deeecaf4c2320"
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