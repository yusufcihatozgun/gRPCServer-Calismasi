using Grpc.Core;
using grpcServer;

namespace grpcServer.Services
{
    public class MessageService : Message.MessageBase
    {
        public override async Task<UnaryResponse> SendMessageUnary(UnaryRequest request, ServerCallContext context)
        {
            Console.Clear();
            Console.WriteLine(request.RequestMessage);
            Console.WriteLine();
            return new UnaryResponse { ResponseMessage = "UnaryResponse" };

        }

        public override async Task SendMessageServerStreaming(ServerStreamingRequest request, IServerStreamWriter<ServerStreamingResponse> responseStream, ServerCallContext context)
        {
            Console.Clear();
            Console.WriteLine(request.RequestMessage);
            Console.WriteLine();

            for (int i = 0; i < 10; i++)
            {
                await responseStream.WriteAsync(new ServerStreamingResponse { ResponseMessage = "ServerStreamResponse No : " + i });
                await Task.Delay(100);
            }
        }

        public override async Task<ClientStreamingResponse> SendMessageClientStreaming(IAsyncStreamReader<ClientStreamingRequest> requestStream, ServerCallContext context)
        {
            Console.Clear();
            while (await requestStream.MoveNext(context.CancellationToken))
            {
                Console.WriteLine(requestStream.Current.RequestMessage);
                await Task.Delay(100);
            }
            Console.WriteLine();

            return new ClientStreamingResponse { ResponseMessage = "Client Streaming Response" };

        }

        public override async Task SendMessageBiDirectionalStreaming(IAsyncStreamReader<BiDirectionalStreamingRequest> requestStream, IServerStreamWriter<BiDirectionalStreamingResponse> responseStream, ServerCallContext context)
        {
            int i = 0;
            Console.Clear();

            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                Console.WriteLine(request.RequestMessage);
                await Task.Delay(100);

                var response = new BiDirectionalStreamingResponse
                {
                    ResponseMessage = "BiDirectionalStreamingResponse No : " + i++
                };

                await responseStream.WriteAsync(response);
            }
            Console.WriteLine();

        }
    }
}