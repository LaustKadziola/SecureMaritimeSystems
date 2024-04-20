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

        shipEdgeRouter = self.addHost('shipEdge', cls=LinuxRouter, ip='10.0.0.1/24')
        shoreEdgeRouter = self.addHost('shoreEdge', cls=LinuxRouter, ip='10.1.0.1/24')
        
        shipSwitch = self.addSwitch('s1')
        shoreSwitch = self.addSwitch('s2')

        self.addLink(shipSwitch,
                     shipEdgeRouter,
                     intfName2 = 'r1-eth1',
                     params2 = {'ip' : '10.0.0.1/24'})
        
        self.addLink(shoreSwitch,
                     shoreEdgeRouter,
                     intfName2 = 'r2-eth1',
                     params2 = {'ip' : '10.1.0.1/24'})

        self.addLink(shipEdgeRouter,
                     shoreEdgeRouter,
                     intfName1 = 'r1-eth2',
                     intfName2 = 'r2-eth2',
                     params1 = {'ip' : '10.100.0.1/24'},
                     params2 = {'ip' : '10.100.0.2/24'})
        
        #NetworkRouter1 = self.addHost('r1')
        #self.addLink(shipEdgeRouter, NetworkRouter1)
        #self.addLink(shoreEdgeRouter, NetworkRouter1)

        shipAgent = self.addHost(name = 'shipAgent',
                                 ip = '10.0.0.251/24',
                                 defaultRoute = 'via 10.0.0.1')
        shoreAgent = self.addHost(name = 'shoreAgent',
                                  ip = '10.1.0.252/24',
                                  defaultRoute = 'via 10.1.0.1')

        self.addLink(shipSwitch, shipAgent)
        self.addLink(shoreSwitch, shoreAgent)


        
        
        #switch1 = self.addSwitch('s1')
	#switch2=self.addSwitch('s2')
        #host1 = self.addHost('h%s' % (1))
        #self.addLink(host1, switch1)
        #host2 = self.addHost('h%s' % (2))
        #self.addLink(host2, switch1)
        #host3 = self.addHost('h%s' % (3))
        #self.addLink(host3, switch2)
        #host4 = self.addHost('h%s' % (4))
        #self.addLink(host4, switch2)	
    
def run():
    "Create and test a simple network"
    topo = ShipTopo()
    net = Mininet(topo)

    info(net['r1'].cmd("ip route and 10.1.0.0/24 via 10.100.0.2 dev r1-eht2"))
    info(net['r2'].cmd("ip route and 10.0.0.0/24 via 10.100.0.1 dev r2-eht2"))
    
    net.start()
    CLI(net)
    net.stop()

if __name__ == '__main__':
    # Tell mininet to print useful information
    setLogLevel('info')
    run()
