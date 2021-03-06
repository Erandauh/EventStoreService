EventStoreService
=================

This is a TopShelf wrapper around the EventStore (http://geteventstore.com) so you can host this as a service.


Sample Configuration

<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <configSections>
        <section name="eventStoreService"
                 type="EventStoreService.Configuration.EventStoreServiceConfiguration, EventStoreService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
    </configSections>

    <startup>
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    </startup>

    <eventStoreService xmlns="urn:EventStoreService.Configuration">
        <windowsService description="Production EventStore" serviceName="EventStoreProduction"
                        displayName="EventStore (Production)" />

        <eventStore runMode="Cluster" binaryPath=".\eventstore">
            
            <database inMemory="true" />
            <projections run="System" threads="3" />
            
            <singleNode httpPort="2113" tcpPort="1113" ip="127.0.0.1" />
           
            <cluster clusterSize="3">
                <dns discoverViaDns="false" />
                <ip external="127.0.0.1" internal="127.0.0.1" />
                <tcp externalPort="1112" internalPort="1111" />
                <http externalPort="2114" internalPort="1113" />
                <gossip>
                    <seeds>
                        <endpoint address="127.0.0.1:2113" />
                        <endpoint address="127.0.0.1:3113" />
                    </seeds>
                </gossip>
            </cluster>
            
        </eventStore>
    </eventStoreService>

</configuration>

All options are optional and will be assigned the default values of the EventStore.
