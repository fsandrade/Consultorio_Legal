using System;

namespace CL.Core.Shared.ModelViews.Erro
{
    public class ErrorResponse
    {
        public string Id { get; set; }
        public DateTime Data { get; set; }
        public string Mensagem { get; set; }

        public ErrorResponse(string id)
        {
            Id = id;
            Data = DateTime.Now;
            Mensagem = "Erro inesperado.";
        }
    }
}