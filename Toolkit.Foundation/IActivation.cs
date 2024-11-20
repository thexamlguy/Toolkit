namespace Toolkit.Foundation;

public interface IActivation
{
    bool IsActive { get; set; }
}

public interface IActivation<TViewModel> : 
    IActivation;