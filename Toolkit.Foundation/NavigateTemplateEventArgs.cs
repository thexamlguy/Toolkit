namespace Toolkit.Foundation;

public record NavigateTemplateEventArgs(object Region,
    object Template,
    object Content,
    object? Sender = null,
    IDictionary<string, object>? Parameters = null);