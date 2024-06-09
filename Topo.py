
from mininet.net import Mininet
from mininet.node import Controller, RemoteController, OVSKernelSwitch, UserSwitch
from mininet.cli import CLI
from mininet.log import setLogLevel
from mininet.link import Link, TCLink
from mininet.topo import Topo
from mininet.util import dumpNodeConnections


class TestTopo( Topo ):
        # net = Mininet( controller=RemoteController, link=TCLink, switch=OVSKernelSwitch )
    def __init__( self ):
        Topo.__init__(self)
        net = Mininet( )
        hosts = []
        r1 = self.addHost( 'r1', ip="10.0.1.1")
        s1 = self.addSwitch('s1')
        self.addLink( r1, s1 )

        # Add hosts and switches2
        for i in range(5):
            host = "h" + str(i)
            h = self.addHost(host)
            hosts.append(h)
            self.addLink( host, s1 )



if __name__ == '__main__':
    setLogLevel( 'info' )
    topo = TestTopo()
    net = Mininet(topo)
    net.start()
    dumpNodeConnections(net.hosts)
    CLI( net )

    net.stop()
