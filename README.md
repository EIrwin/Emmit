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

    







