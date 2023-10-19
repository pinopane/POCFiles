using Grpc.Net.Client;
using GrpcGreeterClient;

class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        // URL de tu servicio gRPC
        string grpcServiceUrl = "https://localhost:7201";

        // Crea un canal gRPC
        using var channel = GrpcChannel.ForAddress(grpcServiceUrl);

        // Crea un cliente para tu servicio gRPC
        var client = new Greeter.GreeterClient(channel);

        // Ruta del archivo que deseas cargar
        string filePath = "C:\\Users\\pinol\\Source\\Repos\\GrpcService-Files\\GrpcService-Files\\bin\\Debug\\net7.0\\GrpcService-Files.xml";

        // Lee el archivo en un arreglo de bytes
        byte[] fileData = File.ReadAllBytes(filePath);

        // Crea una solicitud con el nombre del archivo y los datos
        var request = new UploadFileRequest
        {
            Filename = "test",
            Filedata = Google.Protobuf.ByteString.CopyFrom(fileData)
        };

        // Realiza la llamada al método UploadFile
        var response = await client.UploadFileAsync(request);

        // Procesa la respuesta
        if (response.Success)
        {
            Console.WriteLine("Archivo subido exitosamente: " + response.Message);
        }
        else
        {
            Console.WriteLine("Error al subir el archivo: " + response.Message);
        }
    }
}
