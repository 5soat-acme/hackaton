# Cria namespace para o nginx ingress controller
resource "kubernetes_namespace" "nginx-ingress" {
  metadata {
    name = "nginx-ingress"
  }

  depends_on = [
    aws_eks_cluster.hackaton-eks,
    aws_eks_node_group.node-group
  ]
}

# Cria nginx ingress controller
resource "helm_release" "nginx-ingress" {
  name       = "${var.projectName}-nginx"
  repository = "https://kubernetes.github.io/ingress-nginx"
  chart      = "ingress-nginx"
  namespace  = "nginx-ingress"

  depends_on = [
    kubernetes_namespace.nginx-ingress
  ]
}