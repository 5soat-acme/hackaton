resource "aws_eks_cluster" "hackaton-eks" {
  name     = var.projectName
  role_arn = var.labRole
  version  = var.clusterVersion

  vpc_config {
    subnet_ids         = ["${var.subnetA}", "${var.subnetB}", "${var.subnetC}"]
    security_group_ids = [aws_security_group.sg-eks.id]
  }

  access_config {
    authentication_mode = var.accessConfig
  }
}