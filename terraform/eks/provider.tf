provider "aws" {
  region = "us-east-1"
}

provider "kubernetes" {
  host                   = aws_eks_cluster.hackaton-eks.endpoint
  cluster_ca_certificate = base64decode(aws_eks_cluster.hackaton-eks.certificate_authority[0].data)
  exec {
    api_version = "client.authentication.k8s.io/v1beta1"
    args        = ["eks", "get-token", "--cluster-name", aws_eks_cluster.hackaton-eks.name]
    command     = "aws"
  }
}

provider "helm" {
  kubernetes {
    host                   = aws_eks_cluster.hackaton-eks.endpoint
    cluster_ca_certificate = base64decode(aws_eks_cluster.hackaton-eks.certificate_authority[0].data)
    exec {
      api_version = "client.authentication.k8s.io/v1beta1"
      args        = ["eks", "get-token", "--cluster-name", aws_eks_cluster.hackaton-eks.name]
      command     = "aws"
    }
  }
}