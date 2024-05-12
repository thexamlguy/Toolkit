using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Disposables;

namespace Toolkit.Foundation;

public partial class ObservableCollectionViewModel<TViewModel> :
    ObservableObject,
    IObservableCollectionViewModel<TViewModel>,
    IInitializer,
    IActivated,
    IDeactivating,
    IDeactivated,
    IDeactivatable,
    IList<TViewModel>,
    IList,
    IReadOnlyList<TViewModel>,
    INotifyCollectionChanged,
    INotificationHandler<Remove<TViewModel>>,
    INotificationHandler<Create<TViewModel>>,
    INotificationHandler<Insert<TViewModel>>,
    INotificationHandler<Move<TViewModel>>,
    INotificationHandler<Replace<TViewModel>>
    where TViewModel :
    notnull
{
    private readonly ObservableCollection<TViewModel> collection = [];

    [ObservableProperty]
    private bool isInitialized;

    public ObservableCollectionViewModel(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer)
    {
        Provider = provider;
        Factory = factory;
        Mediator = mediator;
        Publisher = publisher;
        Disposer = disposer;

        subscriber.Add(this);

        collection.CollectionChanged += OnCollectionChanged;
    }

    public ObservableCollectionViewModel(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer,
        IEnumerable<TViewModel> items)
    {
        Provider = provider;
        Factory = factory;
        Mediator = mediator;
        Publisher = publisher;
        Disposer = disposer;

        subscriber.Add(this);

        collection.CollectionChanged += OnCollectionChanged;
        AddRange(items);
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public event EventHandler? DeactivateHandler;

    public int Count => collection.Count;

    public IDisposer Disposer { get; private set; }

    public IServiceFactory Factory { get; private set; }

    bool IList.IsFixedSize => false;

    bool ICollection<TViewModel>.IsReadOnly => false;

    bool IList.IsReadOnly => false;

    bool ICollection.IsSynchronized => false;

    public IMediator Mediator { get; }

    public IServiceProvider Provider { get; private set; }

    public IPublisher Publisher { get; private set; }

    object ICollection.SyncRoot => this;

    public TViewModel this[int index]
    {
        get => collection[index];
        set => SetItem(index, value);
    }

    object? IList.this[int index]
    {
        get => collection[index];
        set
        {
            TViewModel? item = default;

            try
            {
                item = (TViewModel)value!;
            }
            catch (InvalidCastException)
            {
            }

            this[index] = item!;
        }
    }

    public virtual Task Activated() =>
        Task.CompletedTask;

    public TViewModel Add()
    {
        TViewModel? item = Factory.Create<TViewModel>();

        Add(item);
        return item;
    }

    public TViewModel Add<T>(params object?[] parameters)
        where T : TViewModel
    {
        T? item = Factory.Create<T>(parameters);
        Add(item);

        return item;
    }

    public TViewModel Add<T>(bool scope = false)
        where T :
        TViewModel
    {
        IServiceFactory? factory = null;
        if (scope)
        {
            IServiceScope serviceScope = Provider.CreateScope();
            factory = serviceScope.ServiceProvider.GetRequiredService<IServiceFactory>();
        }

        T? item = factory is not null ? factory.Create<T>() : Factory.Create<T>();
        Add(item);

        return item;
    }

    public void Add(TViewModel item)
    {
        int index = collection.Count;
        InsertItem(index, item);
    }

    public void Add(object item)
    {
        int index = collection.Count;
        InsertItem(index, (TViewModel)item);
    }

    int IList.Add(object? value)
    {
        TViewModel? item = default;

        try
        {
            item = (TViewModel)value!;
        }
        catch (InvalidCastException)
        {
        }

        Add(item!);
        return Count - 1;
    }

    public void AddRange(IEnumerable<TViewModel> items)
    {
        foreach (TViewModel? item in items)
        {
            Add(item);
        }
    }

    public void Clear()
    {
        foreach (TViewModel item in collection)
        {
            Disposer.Dispose(item);
        }

        ClearItems();
    }

    public bool Contains(TViewModel item) =>
        collection.Contains(item);

    bool IList.Contains(object? value) =>
        IsCompatibleObject(value) && Contains((TViewModel)value!);

    public void CopyTo(TViewModel[] array, int index) =>
        collection.CopyTo(array, index);

    void ICollection.CopyTo(Array array, int index) =>
        collection.CopyTo((TViewModel[])array, index);

    public Task Deactivate()
    {
        DeactivateHandler?.Invoke(this, new EventArgs());
        return Task.CompletedTask;
    }

    public virtual Task Deactivated() =>
        Task.CompletedTask;

    public virtual Task Deactivating() =>
        Task.CompletedTask;

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
        Disposer.Dispose(this);
    }

    public IEnumerator<TViewModel> GetEnumerator() =>
        collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        ((IEnumerable)collection).GetEnumerator();

    public Task Handle(Remove<TViewModel> args,
        CancellationToken cancellationToken)
    {
        foreach (TViewModel item in this.ToList())
        {
            if (args.Value is not null && args.Value.Equals(item))
            {
                Remove(item);
            }
        }

        return Task.CompletedTask;
    }

    public Task Handle(Create<TViewModel> args,
        CancellationToken cancellationToken)
    {
        if (args.Value is TViewModel item)
        {
            Add(item);
        }

        return Task.CompletedTask;
    }

    public Task Handle(Insert<TViewModel> args,
        CancellationToken cancellationToken)
    {
        if (args.Value is TViewModel item)
        {
            Insert(args.Index, item);
        }

        return Task.CompletedTask;
    }

    public Task Handle(Move<TViewModel> args,
        CancellationToken cancellationToken)
    {
        if (args.Value is TViewModel item)
        {
            Move(args.Index, item);
        }

        return Task.CompletedTask;
    }

    public Task Handle(Replace<TViewModel> args,
        CancellationToken cancellationToken)
    {
        if (args.Value is TViewModel item)
        {
            Replace(args.Index, item);
        }

        return Task.CompletedTask;
    }

    public int IndexOf(TViewModel item) =>
        collection.IndexOf(item);

    int IList.IndexOf(object? value) =>
        IsCompatibleObject(value) ?
        IndexOf((TViewModel)value!) : -1;

    public async Task Initialize()
    {
        if (IsInitialized)
        {
            return;
        }

        IsInitialized = true;
        await Enumerate();
    }


    public async Task Enumerate()
    {
        if (this.GetAttribute<EnumerateAttribute>() is EnumerateAttribute attribute)
        {
            if (attribute.Mode == EnumerateMode.Reset)
            {
                Clear();
            }

            object? key = this.GetPropertyValue(() => attribute.Key) is { } value ? value : attribute.Key;
            await Publisher.PublishUI(PrepareEnumeration(key));
        }
    }

    protected virtual IEnumerate PrepareEnumeration(object? key) => 
        new Enumerate<TViewModel>() with { Key = key };

    public void Insert(int index, TViewModel item) =>
        InsertItem(index, item);

    void IList.Insert(int index,
        object? value)
    {
        if (value is TViewModel item)
        {
            Insert(index, item);
        }
    }

    public bool Move(int index, TViewModel item)
    {
        int oldIndex = collection.IndexOf(item);
        if (oldIndex < 0)
        {
            return false;
        }

        RemoveItem(oldIndex);
        Insert(index, item);

        return true;
    }

    public bool Remove(TViewModel item)
    {
        int index = collection.IndexOf(item);
        if (index < 0)
        {
            return false;
        }

        Disposer.Dispose(item);
        RemoveItem(index);

        return true;
    }

    void IList.Remove(object? value)
    {
        if (IsCompatibleObject(value))
        {
            Remove((TViewModel)value!);
        }
    }

    public void RemoveAt(int index) =>
        RemoveItem(index);

    public bool Replace(int index,
        TViewModel item)
    {
        if (index <= Count - 1)
        {
            RemoveItem(index);
        }
        else
        {
            index = Count;
        }

        Insert(index, item);
        return true;
    }

    protected virtual void ClearItems() =>
        collection.Clear();

    protected virtual void InsertItem(int index,
        TViewModel item)
    {
        Disposer.Add(this, item);
        Disposer.Add(item, item, Disposable.Create(() =>
        {
            if (item is IList collection)
            {
                collection.Clear();
            }
        }));

        collection.Insert(index, item);
    }

    protected virtual void RemoveItem(int index) =>
        collection.RemoveAt(index);

    protected virtual void SetItem(int index, TViewModel item) =>
        collection[index] = item;

    private static bool IsCompatibleObject(object? value) =>
        (value is TViewModel) || (value == null && default(TViewModel) == null);

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args) =>
        CollectionChanged?.Invoke(this, args);
}

public partial class ObservableCollectionViewModel<TValue, TViewModel>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscriber subscriber, IDisposer disposer) : ObservableCollectionViewModel<TViewModel>(provider, factory, mediator, publisher, subscriber, disposer)
    where TViewModel : notnull
{
    [ObservableProperty]
    private TValue? value;
}

public class ObservableCollectionViewModel(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer) :
    ObservableCollectionViewModel<IDisposable>(provider, factory, mediator, publisher, subscriber, disposer);