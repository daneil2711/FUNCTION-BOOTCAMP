using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class fnDaniPacote
    {
        [FunctionName("fnDaniPacote")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, [Queue("fila-pedido"),StorageAccount("AzureWebJobsStorage")]
            ICollector<Pedido> itemPedido)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var pedido = new Pedido()
            {
                NumeroPedido = Guid.NewGuid().ToString("n"),
                Data = data.datapedido,
                Item = data.item,
                Preco = (decimal)data.preco
            };

            itemPedido.Add(pedido);

            return new OkObjectResult(pedido);
        }
    }
    public class Pedido
    {
        public string NumeroPedido {get; set;}
        public string Data {get; set;}
        public string Item {get; set;}
        public decimal Preco {get; set;}
    }
}
