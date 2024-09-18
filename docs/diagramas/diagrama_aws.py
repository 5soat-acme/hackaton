from diagrams import Diagram, Cluster
from diagrams.aws.compute import EC2
from diagrams.onprem.client import Client
from diagrams.aws.network import ElbNetworkLoadBalancer, InternetGateway, NATGateway, APIGateway
from diagrams.aws.compute import EKS
from diagrams.aws.compute import Lambda
from diagrams.aws.security import Cognito
from diagrams.aws.database import Aurora, Dynamodb
from diagrams.aws.integration import SQS

with Diagram("Hackaton - AWS", filename="diagrama_aws", outformat=["png"], show=False):
    with Cluster("Clients"):
        clients = [Client("client_1"),
                    Client("client_2"),
                    Client("client_3")]

    with Cluster("AWS Cloud"):
        with Cluster("VPC"):
            internet_gateway = InternetGateway("internet-gateway")
            load_balancer = ElbNetworkLoadBalancer("load-balancer")
            eks = EKS("eks-cluster")
            with Cluster("Subnet"):
                nat_gateway = NATGateway("nat-gateway")
                with Cluster("Node Group"):
                    nodes = [EC2("EC2_1"),
                                EC2("EC2_2")]
                    
        database_aurora_postgresql = Aurora("database-aurora-postgresql")
    
    clients >> load_balancer >> nodes
    internet_gateway - nat_gateway
    eks - nodes
    nodes >> database_aurora_postgresql