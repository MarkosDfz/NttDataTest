using Microsoft.VisualStudio.TestTools.UnitTesting;
using NttDataTest.Domain.Client;
using NttDataTest.UnitTest.Client.Config;
using System;

namespace NttDataTest.UnitTest.Client
{
    [TestClass]
    public class ClientCreateCommandTest
    {
        [TestMethod]
        public void TryToCreateAClient()
        {
            var context = DataContextInMemory.Get();

            Guid guid = Guid.NewGuid();

            Cliente cliente = new()
            {
                ClienteGuid = guid.ToString(),
                Nombre = "Cliente Test",
                Direccion = "Quito",
                Edad = 27,
                Estado = true,
                Contrasenia = "123456789",
                Genero = 0,
                Identificacion = "151899966",
                Telefono = "0986989654"
            };

            context.Clientes.Add(cliente);
        }
    }
}
