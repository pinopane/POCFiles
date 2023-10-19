using Grpc.Core;

namespace GrpcService_Files.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Sending hello to {request.Name}");
            return Task.FromResult(new HelloReply { Message = $"Hello {request.Name}" });
        }
        public override async Task<UploadResponse> UploadFile(UploadFileRequest request, ServerCallContext context)
        {
            string extension = request.Extension;
            string filename = request.Filename;
            byte[] fileData = request.Filedata.ToByteArray();

            // Ruta completa para guardar el archivo en la unidad C:
            string savePath = "C:\\Users\\pinol\\OneDrive\\Documentos\\test\\" + filename;

            try
            {
                // Procesa el archivo como desees y guárdalo en la unidad C:
                using (var fileStream = System.IO.File.Create(savePath))
                {
                    fileStream.Write(fileData, 0, fileData.Length);
                }

                return new UploadResponse { Success = true, Message = "Archivo subido exitosamente" };
            }
            catch (Exception ex)
            {
                return new UploadResponse { Success = false, Message = "Error al subir el archivo: " + ex.Message };
            }
        }
    }
}
