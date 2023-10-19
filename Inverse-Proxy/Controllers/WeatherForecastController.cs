using Grpc.Net.Client;
using GrpcGreeterClient;
using Microsoft.AspNetCore.Mvc;

namespace Inverse_Proxy.Controllers
{
    [ApiController]
    [Route("api")]
    public class FileUploadController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // URL de tu servicio gRPC
            string grpcServiceUrl = "https://localhost:7201";

            // Crea un canal gRPC
            using var channel = GrpcChannel.ForAddress(grpcServiceUrl, new GrpcChannelOptions
            {
                MaxReceiveMessageSize = 10 * 1024 * 1024, // Establece el mismo límite que en el servidor
            });

            // Crea un cliente para tu servicio gRPC
            var client = new Greeter.GreeterClient(channel);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Copia el contenido del archivo IFormFile al MemoryStream
                file.CopyTo(memoryStream);

                // Obtén los datos del MemoryStream como un arreglo de bytes
                byte[] fileData = memoryStream.ToArray();
                // Crea una solicitud con el nombre del archivo y los datos
                var request = new UploadFileRequest
                {
                    Extension = Path.GetExtension(file.FileName),
                    Filename = file.FileName,
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
            return Ok();
        }
    }
}