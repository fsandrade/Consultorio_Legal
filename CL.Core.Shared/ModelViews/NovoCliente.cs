using System;

namespace CL.Core.Shared.ModelViews
{
    /// <summary>
    /// Objeto utilizado para inserção de um novo cliente.
    /// </summary>
    public class NovoCliente
    {
        /// <summary>
        /// Nome do cliente
        /// </summary>
        /// <example>João do Caminhão</example>
        public string Nome { get; set; }

        /// <summary>
        /// Data do nascimento do cliente.
        /// </summary>
        /// <example>1980-01-01</example>
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Sexo do cliente
        /// </summary>
        /// <example>M</example>
        public char Sexo { get; set; }

        /// <summary>
        /// Telefone do cliente
        /// </summary>
        /// <example>79999887744</example>
        public string Telefone { get; set; }

        /// <summary>
        /// Documento do cliente: CNH, CPF, RG
        /// </summary>
        /// <example>12341231312</example>
        public string Documento { get; set; }

        public NovoEndereco Endereco { get; set; }
    }
}