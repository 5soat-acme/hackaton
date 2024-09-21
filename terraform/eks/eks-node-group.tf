resource "aws_eks_node_group" "node-group" {
  cluster_name    = aws_eks_cluster.hackaton-eks.name
  node_group_name = "NG-${var.projectName}"
  node_role_arn   = var.labRole
  subnet_ids      = ["${var.subnetA}", "${var.subnetB}", "${var.subnetC}"]
  disk_size       = 50
  instance_types  = [var.instanceType]

  scaling_config {
    desired_size = 3
    min_size     = 3
    max_size     = 6
  }

  update_config {
    max_unavailable = 1
  }
}