# LearngRPC
Main goal of this repo is to understand what is the gRPC protocol and how to use it in the microservices architecture as well as how it implements on the backend.

This repository made by the course [Using gRPC in Microservices Communication with .Net 5](https://www.udemy.com/course/using-grpc-in-microservices-communication-with-net-5/)

gRPC has found huge success because of using HTTP/2 instead of HTTP/1.1 and protocol buffers instead of XML and JSON.

## HTTP/2
HTTP/2 is the latest version of the HTTP protocol which was launched in 2015 to make applications faster and more robust by addressing the drawbacks of the HTTP/1.1 protocol. HTTP/2 has been picking up fast and most browsers support HTTP/2 nowadays.

- Multiplexing: Enable request and response multiplexing which permits multiple requests and responses to be sent at the same time in a single TCP connection
- Header Compression: It compresses headers that have been requested previously to make things more efficient
- Stream prioritization: This allows for the exchange of successive streams at one time
- Server push: The server can send additional information to the client without the client having to request each one explicitly.
- Increased security: HTTP/2 is supported through encrypted connections
- Server-side backward compatibility: To make sure that servers can still support clients that use HTTP/1.1 without any changes.
- Compatibility with the methods, status codes, URIs, and header fields defined by the HTTP/1.1 standard

### Protocol Buffers
gRPC uses Protocol Buffers by Default.
Protocol Buffers are Google’s open source mechanism for serializing structured data.

<div>
  <p align="center">
    <img src="https://i.stack.imgur.com/gl5Oo.png"width="600"/>  
  </p>
</div>

When working with protocol buffers, the first step is to define the structure of the data you want to serialize in a proto file: this is an ordinary text file with the extension .proto.
The protocol buffer data is structured as messages where each message is a small logical information record containing a series of name-value pairs called fields.

Once you’ve determined your data structures, you use the protocol buffer compiler protocol to create data access classes in the languages you prefer from your protocol definition.

### How gRPC works
In GRPC, a client application can directly call a method on a server application on a different machine like it were a local object, making it easy for you to build distributed applications and services.

<div>
  <p align="center">
    <img src="https://alexromanov.github.io/img/20210612/grpc.png"width="600"/>  
  </p>
</div>

As with many RPC systems, gRPC is based on the idea of defining a service that specifies methods that can be called remotely with their parameters and return types. On the server side, the server implements this interface and runs a gRPC server to handle client calls. On the client side, the client has a stub that provides the same methods as the server.

gRPC clients and servers can work and talk to each other in a different of environments, from servers to your own desktop applications, and that can be written in any language that gRPC supports. For example, you can easily create a gRPC server in Java or c# with clients in Go, Python or Ruby.

### gRPC Protocol = HTTP/2 + Protobuf
gRPC is widely used for communication between internal microservices majorly due to its high performance and its polyglot nature.

gRPC uses HTTP/2 as its transfer protocol and hence it inherits the benefits like binary framing from HTTP/2. As I mentioned in my previous article, HTTP/2 is robust, lightweight to transport, and safer to decode compared to other text-based protocols. Due to its binary nature, HTTP/2 forms a good combination with the Protobuf format.

gRPC works in the same way as old Remote Procedure Calls(RPC). It is API oriented as opposed to REST protocol which is resource-based. Protbuf gives it support for 11 languages out of the box and all the benefits of binary protocol superpowers in terms of performance, throughput, and flexibility.

### gRPC for microservices
One of the biggest advantages of microservices is the ability to use different technologies for each independent service i.e polyglot. Each microservices agrees on API to exchange data, data format, error patterns, load balancing, etc. Since gRPC allows for describing a contract in a binary format, it can be used efficiently for microservices communication independent of the languages.

<div>
  <p align="center">
    <img src="https://miro.medium.com/max/552/1*HRO6F1VnuK_3YOo_oPgxvg.png"width="600"/>  
  </p>
</div>

### Overall Picture
See the overall picture. You can see that we will have 6 microservices which we are going to develop.
We will use Worker Services and Asp.Net 5 Grpc applications to build client and server gRPC components defining proto service definition contracts.

<div>
  <p align="center">
    <img src="https://user-images.githubusercontent.com/1147445/98652230-5f66ee80-234c-11eb-9201-8b291b331c9f.png"width="600"/>  
  </p>
</div>

Basically we will implement e-commerce logic with only gRPC communication. We will have 3 gRPC server applications which are Product — ShoppingCart and Discount gRPC services. And we will have 2 worker services which are Product and ShoppingCart Worker Service. Worker services will be client and perform operations over the gRPC server applications. And we will secure the gRPC services with standalone Identity Server microservices with OAuth 2.0 and JWT token.

## ProductGrpc Server Application
First of all, we are going to develop ProductGrpc project. This will be asp.net gRPC server web application and expose apis for Product Crud operations.

### Product Worker Service
After that, we are going to develop Product Worker Service project for consuming ProductGrpc services. This product worker service project will be the client of ProductGrpc application and generate products and insert bulk product records into Product database by using client streaming gRPC proto services of ProductGrpc application. This operation will be in a time interval and looping as a service application.

### ShoppingCartGrpc Server Application
After that, we are going to develop ShoppingCartGrpc project. This will be asp.net gRPC server web application and expose apis for SC and SC items operations. The grpc services will be create sc and add or remove item into sc.

### ShoppingCart Worker Service
After that, we are going to develop ShoppingCart Worker Service project for consuming ShoppingCartGrpc services. This ShoppingCart worker service project will be the client of both ProductGrpc and ShoppingCartGrpc application. This worker service will read the products from ProductGrpc and create sc and add product items into sc by using gRPC proto services of ProductGrpc and ShoppingCartGrpc application. This operation will be in a time interval and looping as a service application.

### DiscountGrpc Server Application
When adding product item into SC, it will retrieve the discount value and calculate the final price of product. This communication also will be gRPC call with SCGrpc and DiscountGrpc application.

### Identity Server
Also, we are going to develop centralized standalone Authentication Server with implementing IdentityServer4 package and the name of microservice is Identity Server.
Identity Server4 is an open source framework which implements OpenId Connect and OAuth2 protocols for .Net Core.
With IdentityServer, we can provide protect our SC gRPC services with OAuth 2.0 and JWT tokens. SC Worker will get the token before send request to SC Grpc server application.