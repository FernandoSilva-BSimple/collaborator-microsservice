using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.IPublishers;
using Domain.Models;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi.Consumers;
using Xunit;
using Domain.Messages;

namespace WebApi.IntegrationTests.MessagingIntegrationTests
{
    public class MessagingIntegrationTests
    {
        [Fact]
        public async Task When_CollaboratorCreatedEvent_IsPublished_ConsumerShould_ConsumeIt()
        {
            // CODIGO OBTIDO UTILIZANDO GEMINI

            // em comparação com o do prof:

            /*

            Esta é a abordagem recomendada para testar aplicações .NET modernas, que usam intensivamente a injeção de dependência.

            Como funciona: Em vez de criarmos o harness manualmente, pedimos a um ServiceProvider (provider) para o fazer por nós.
            O Papel do provider: Nós usamos o ServiceCollection para "ensinar" o provider a conhecer todos os nossos serviços e as suas dependências, tal como acontece na sua aplicação real.
            Dizemos-lhe: "Quando alguém pedir um ICollaboratorService, entrega este collabServiceMock.Object".
            Usamos AddMassTransitTestHarness: Este método inteligente regista o harness e os nossos consumidores. O mais importante é que ele usa o próprio provider para construir as instâncias dos consumidores.
            A Magia Acontece Aqui: Quando o harness precisa de criar o CollaboratorConsumer, ele não o faz sozinho. Ele pede ao provider: "Dá-me um CollaboratorConsumer". O provider, por sua vez, vê que o CollaboratorConsumer precisa de um ICollaboratorService, fornece o mock que registámos e entrega o consumidor pronto a usar.
            
            */

            // arrange
            var collabServiceDouble = new Mock<ICollaboratorService>();

            // Configurar os serviços e o MassTransit com o TestHarness
            await using var provider = new ServiceCollection()

                // 2. Registar o mock para a INTERFACE ICollaboratorService.
                // Quando o MassTransit criar o CollaboratorConsumer, ele vai pedir um
                // ICollaboratorService e o contentor vai entregar este mock.
                .AddSingleton<ICollaboratorService>(collabServiceDouble.Object)

                .AddMassTransitTestHarness(cfg =>
                {
                    // Registar o consumidor que queremos testar
                    cfg.AddConsumer<CollaboratorCreatedConsumer>();
                })
                .BuildServiceProvider(true);

            // Obter o harness a partir do contentor de serviços
            var harness = provider.GetRequiredService<ITestHarness>();

            // Iniciar o harness
            await harness.Start();

            try
            {
                // Act
                var message = new CollaboratorCreatedMessage(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1))
                );

                await harness.Bus.Publish(message);

                // Assert

                // Verificar se o NOSSO consumidor específico consumiu a mensagem
                Assert.True(await harness.GetConsumerHarness<CollaboratorCreatedConsumer>().Consumed.Any<CollaboratorCreatedMessage>());

                // Verificar se o consumidor realmente fez o seu trabalho, chamando o método correto no serviço
                collabServiceDouble.Verify(
                    s => s.AddCollaboratorReferenceAsync(message.Id, message.UserId, message.PeriodDateTime),
                    Times.Once
                );
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async Task When_CollaboratorUpdatedEvent_IsPublished_ConsumerShould_ConsumeIt()
        {
            // Arrange
            var collabServiceDouble = new Mock<ICollaboratorService>();

            await using var provider = new ServiceCollection()
                .AddSingleton<ICollaboratorService>(collabServiceDouble.Object)
                .AddMassTransitTestHarness(cfg =>
                {
                    // Registar o consumidor de ATUALIZAÇÃO que queremos testar
                    cfg.AddConsumer<CollaboratorUpdatedConsumer>();
                })
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            try
            {
                // Act
                var message = new CollaboratorUpdatedMessage(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1))
                );

                await harness.Bus.Publish(message);

                // Assert
                // Verificar se o nosso consumidor específico consumiu a mensagem
                Assert.True(await harness.GetConsumerHarness<CollaboratorUpdatedConsumer>().Consumed.Any<CollaboratorUpdatedMessage>());

                // Verificar se o consumidor chamou o método de serviço correto
                collabServiceDouble.Verify(
                    s => s.UpdateCollaboratorReferenceAsync(message.Id, message.UserId, message.PeriodDateTime),
                    Times.Once
                );
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}