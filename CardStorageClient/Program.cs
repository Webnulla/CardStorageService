

using ClientServiceProtos;
using Grpc.Net.Client;

Console.WriteLine("Создать клиента");
Console.ReadLine();

using (GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001"))
{
    ClientService.ClientServiceClient client = new ClientService.ClientServiceClient(channel);

    var response = client.Create(new ClientServiceProtos.CreateClientRequest
    {
        FirstName = "Star",
        Surname = "Lord",
    });
    
    Console.WriteLine($"ClientId: {response.ClientId}; ErrorCode: {response.ErrorCode}; ErrorMessage: {response.ErrorMessage}");
}
