from diagrams import Diagram, Cluster
from diagrams.k8s.clusterconfig import HPA
from diagrams.k8s.compute import Pod, ReplicaSet, Deployment, StatefulSet
from diagrams.k8s.network import SVC, Ing
from diagrams.onprem.client import Client
from diagrams.k8s.storage import PV, PVC, StorageClass
from diagrams.k8s.podconfig import Secret
from diagrams.onprem.network import Nginx
from diagrams.k8s.ecosystem import Helm

with Diagram("Hackaton - K8S", filename="diagrama_k8s", outformat=["png"], show=False):
    with Cluster("Clients"):
        clients = [Client("client_1"),
                    Client("client_2"),
                    Client("client_3")]

    with Cluster("AWS Cloud"):
        ingress_controller = Nginx("ingress-controller")

        with Cluster("K8S"):
            ## API
            ingress = Ing("api-ingress")
            svc_api = SVC("svc-clusterip")

            with Cluster("Nodes API"):
                pods_api = [Pod("api-1"),
                        Pod("api-2")]
                
            replicaSet = ReplicaSet("rs")
            deployment = Secret("secrets") - Deployment("deploy")
            hpa = HPA("hpa")


    clients >> ingress_controller >> ingress
    ingress >> svc_api
    
    svc_api >> pods_api << replicaSet << deployment << hpa