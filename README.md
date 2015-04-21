# What is Emmit?
Emmit is just a simple wrapper for a SignalR Hub that 'encourages' pushing data from server to listening clients (and only that). In fact, we have hidden the ability for the client to talk to the server (through the exposed API anyway).

# Why isn't client-to-server exposed in Emmit?
HTTP seems to be doing alright handling communication from the client to the server. There are plenty of frameworks that facilitate this. Emmit only cares about communicating to the client(s) from the server.

Since we are using one mechanism for client-to-server communication, and another for server-to-client,our code looks much cleaner.







