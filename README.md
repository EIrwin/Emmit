## Get it on NuGet!
You can install Emmit via nuget by visiting the [Emmit NuGet](https://www.nuget.org/packages/Emmit/1.0.0) or installing directly via your NuGet package manager.

    Install-Package Emmit

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
* Minimal setup and dependencies

## Can I still use SignalR?
Absolutely! As we mentioned, Emmit is just built as an abstraction on top of SignalR in order to facilitate certain operations. All `Emitter` objects extend SignalR `Hub` and therefore inherits base SignalR functionality.

## How does client consume data from Emmit emitters?
There are several options for implementing your client to use with Emmit?

1. Use SignalR Hub client side code just as you would with a normal SignalR server implementation.
```javascript
        var proxy = null;
        var connection = $.hubConnection();
        var connection.url = "http://localhost:8181/emmit";
        proxy = connection.createHubProxy("stockEmitter");

        proxy.on('onOpenStock',function(message){
           //do something with the data from the server
        });
        
        proxy.on('onCloseStock',function(message){
           //do something with the data from the server
        });
        
         connection.start()
                .done(function () {
                  //connected successfully
                })
                .fail(function () {
                  //couldn't connect
                });    
```                
2. Use the [angular-emmit-client](https://github.com/EIrwin/angular-emmit-client) bower package to create a proxy to the server.
```javascript
        myApp.controller('myController',['Emmit'],function(Emmit){
          Emmit.createProxy({
                emitter:'myEmitter',
                path:'http://localhost:8181/emmit',
                listeners:{
                    'onSendNotification':function(notification){
                        //do something with 'notification' received from server
                    }
                },
                onError:function(){
                    //an error occured
                },
                onDisconnected:function(){
                    //proxy was disconnected
                },
                queryParams:{
                    userId:'12345'  //optional
                }
            }).then(function(newProxy){
                //Do something here with the proxy if you need
            });
        }
``` 

## Sample Usage
Since Emmit is built on top of SignalR, you dont have to change any of the initialization or middleware used to get Emmit up and running. Emmit provides some 'syntactic sugar' to run on top of SignalR when pushing data to the client from the server.

#### SignalR Setup and Configuration

It is good practice to centralize our configuration into a class. In this example, we will create a `Startup` class that will be passed as a generic type definition to `WebApp.Start<Startup>(string url)`

    public class EmnmitStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR("/emmit",new HubConfiguration()
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

    IEmitterFactory factory = new EmitterFactory();
    IStockEmitter stockEmitter = factory.Create<StockEmitter>();
    stockEmitter.OnOpenStock("Google");
    
The `StockEmitter` class is simple and looks like the following:

    public class StockEmitter:Emitter,IStockEmitter
    {
        public StockEmitter() : base(null)
        {
            /* Default constructor needed by SignalR*/
        }
        public StockEmitter(IHubContext context) : base(context)
        {
            /*Overloaded constructor needed for Emmit*/
        }

        public void OnOpenStock(string symbol)
        {
            /*Invoke onOpenStock on client(s)*/
            if (EmitterContext != null)
                EmitterContext.Clients.All.onOpenStock(symbol);
        }

        public void OnCloseStock(string symbol)
        {
            /*Invoke onCloseStock on client(s)*/
            if (EmitterContext != null)
                EmitterContext.Clients.All.onCloseStock(symbol);
        }
    }

### Using Emmit with Self-Hosted NancyFX
The example below shows how Emmit can be combined with NancyFX TinyIocContainer to inject `IEmitter` objects into NancyModules.

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
    
A sample `NancyModule` for this example is the following. The example below shows the `IStockEmitter` being invoked from the module. It is important to note that `IEmitter` implementations can be invoked anywhere they are injected, or created from `IEmitterFactory`. For this example, we just want to show how it can be used after being injected into a `NancyModule`.

    public class StockModule:NancyModule
    {
        public StockModule(IStockEmitter stockEmitter,IStockService stockService)
        {
            Post["/Open",true] = async (context, cancel) =>
                {
                    string symbol = context.Symbol;
                    bool stockOpened = stockService.OpenStock(symbol)
                    stockEmitter.OnOpenStock(symbol;
                    return Task.FromResult(Response.AsJson(true));
                };
        }
    }

#### Advanced Usage


    







