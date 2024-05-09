from mininet.topo import Topo
from mininet.net import Mininet
from mininet.util import dumpNodeConnections
from mininet.log import setLogLevel, info
from mininet.cli import CLI
from mininet.node import Node

topos = {'ShipTopo': ( lambda: ShipTopo() )}

class LinuxRouter(Node):
    def config(self, **params):
        super(LinuxRouter, self).config(**params)
        self.cmd('sysctl net.ipv4.ip_forward=1')

    def terminate(self):
        self.cmd('systcl ner.ip_forward=0')
        super(LinuxRouter, self).terminate()

class ShipTopo(Topo):
    def build(self, **_opts):

        print(self.controller)

        edgeRouter = self.addHost('shipEdge')
        
        s1 = self.addSwitch('s1')
        s2 = self.addSwitch('s2')
        s3 = self.addSwitch('s3')

        h1 = self.addHost('h1')
        h2 = self.addHost('h2')
        h3 = self.addHost('h3')

        self.addLink(s1, edgeRouter)
        self.addLink(s2, edgeRouter)
        self.addLink(s3, edgeRouter)
        self.addLink(s1, h1)
        self.addLink(s2, h2)
        self.addLink(s3, h3)
        
        
def run():
    "Create and test a simple network"
    topo = ShipTopo()
    net = Mininet(top)
    net.start()
    CLI(net)
    net.stop()

if __name__ == '__main__':
    # Tell mininet to print useful information
    setLogLevel('info')
    run()
