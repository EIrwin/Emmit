## What is Emmit?
Emmit is just a simple wrapper for [SignalR](https://github.com/SignalR/SignalR) Hub that 'encourages' pushing data from server to listening clients (and only that). In fact, we have hidden the ability for the client to talk to the server (through the exposed API anyway).

## What can Emmit be used for?
Emmit is intended to be used for the sole purpose of pushing data from the server to the client (not just web browsers).

## Why isn't client-to-server communication 'encouraged' in Emmit?
HTTP seems to be doing alright handling communication from the client to the server. There are plenty of frameworks that facilitate this. Emmit only cares about communicating to the client(s) from the server.

Since we are using one mechanism for client-to-server communication, and another for server-to-client, we can clearly separate these responsibilities in our code.

## What benefits does Emmit provide over just using SignalR?

* Separation of client-to-server and server-to-client transport methods.
* Encourages strongly-typed 'Emitters' (equivalent to SignalR Hubs)
* Designed to be injectable into common IoC containers.

## Get it on NuGet!

    Install-Package Emmit

## Sample Usage
Since Emmit is built on top of SignalR, you dont have to change any of the initialization or middleware used to get Emmit up and running. Emmit provides some 'syntactic sugar' to run on top of SignalR when pushing data to the client from the server.

#### SignalR Setup and Configuration

It is good practice to centralize our configuration into a class. In this example, we will create a `Startup` class that will be passed as a generic type definition to `WebApp.Start<Startup>(string url)`

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR(new HubConfiguration()
                {
                    EnableJavaScriptProxies = true
                });
        }
    }
    
We can start SignalR just as we normally would.

    public class Program
    {
        public static void Main(string[] args)
        {
            string url = "http://localhost:8181";
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }

#### Basic Usage
The example below shows basic standalone usage of Emmit.

    Emmit.IEmitterFactory factory = new Emmit.EmitterFactory();
    IStockEmitter stockEmitter = factory.Create<StockEmitter>();
    stockEmitter.OnOpenStock("Google");

### Using Emmit with Self-Hosted NancyFX
The example below shows how Emmit can be combined with NancyFX TinyIocContainer to inject IEmitter's into NancyModules.

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, global::Nancy.Bootstrapper.IPipelines pipelines)
        {
            //Configure application startup
            ConfigureIocContainer(container);
            base.ApplicationStartup(container, pipelines);
        }

        private void ConfigureIocContainer(TinyIoCContainer container)
        {
            //Tell TinyIoC how we want to inject IStockEmitter
            Emmit.IEmitterFactory factory = new Emmit.EmitterFactory();
            container.Register((cont, overloads) => (IStockEmitter) factory.Create<StockEmitter>());
        }
    }

    







