namespace Toolkit.Framework.Foundation;

public class WriteHandler<TConfiguration> : IRequestHandler<Write<TConfiguration>> where TConfiguration : class
{
    private readonly IMediator mediator;
    private readonly TConfiguration configuration;
    private readonly IConfigurationWriter<TConfiguration> writer;

    public WriteHandler(TConfiguration configuration,
        IConfigurationWriter<TConfiguration> writer,
        IMediator mediator)
    {
        this.mediator = mediator;
        this.configuration = configuration;
        this.writer = writer;
    }

    public async ValueTask<Unit> Handle(Write<TConfiguration> request, CancellationToken cancellationToken)
    {
        request.UpdateDelegate.Invoke(configuration);
        writer.Write(configuration);

        await mediator.Send(new ConfigurationChanged<TConfiguration>(configuration), cancellationToken);

        return default;
    }
}