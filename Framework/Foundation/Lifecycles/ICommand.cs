namespace Toolkit.Framework.Foundation;

public interface ICommand<out TResponse> : IMessage { }
public interface ICommand : ICommand<Unit> { }
