using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Specialized;
using System.Reactive.Disposables;

namespace Toolkit.Foundation;

public partial class ObservableCollection<TItem> :
    ObservableObject,
    IObservableCollectionViewModel<TItem>,
    IInitializer,
    IActivated,
    IDeactivating,
    IDeactivated,
    IDeactivatable,
    IList<TItem>,
    IList,
    IReadOnlyList<TItem>,
    INotifyCollectionChanged,
    IServiceProviderRequired,
    IServiceFactoryRequired,
    IMediatorRequired,
    IPublisherRequired,
    IDisposerRequired,
    INotificationHandler<RemoveEventArgs<TItem>>,
    INotificationHandler<RemoveAtEventArgs<TItem>>,
    INotificationHandler<CreateEventArgs<TItem>>,
    INotificationHandler<InsertEventArgs<TItem>>,
    INotificationHandler<MoveEventArgs<TItem>>,
    INotificationHandler<MoveToEventArgs<TItem>>,
    INotificationHandler<ReplaceEventArgs<TItem>>,
    INotificationHandler<SelectionEventArgs<TItem>>
    where TItem :
    IDisposable
{
    private readonly System.Collections.ObjectModel.ObservableCollection<TItem> collection = [];

    private bool clearing;

    [ObservableProperty]
    private bool initialized;

    [ObservableProperty]
    private int selectedIndex = 0;

    [ObservableProperty]
    private TItem? selectedItem;

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscription subscriber,
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

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscription subscriber,
        IDisposer disposer,
        IEnumerable<TItem> items)
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

    bool ICollection<TItem>.IsReadOnly => false;

    bool IList.IsReadOnly => false;

    bool ICollection.IsSynchronized => false;

    public IMediator Mediator { get; }

    public IServiceProvider Provider { get; private set; }

    public IPublisher Publisher { get; private set; }

    object ICollection.SyncRoot => this;

    public TItem this[int index]
    {
        get => collection[index];
        set => SetItem(index, value);
    }

    object? IList.this[int index]
    {
        get => collection[index];
        set
        {
            TItem? item = default;

            try
            {
                item = (TItem)value!;
            }
            catch (InvalidCastException)
            {
            }

            this[index] = item!;
        }
    }

    public TItem Add() =>
        Add<TItem>(null, false);

    public TItem Add<T>(params object?[] parameters)
        where T : TItem => Add<T>(false, parameters);

    public TItem Add<T>(bool scope = false,
        params object?[] parameters)
        where T :
        TItem
    {
        IServiceFactory? factory = null;
        if (scope)
        {
            IServiceScope serviceScope = Provider.CreateScope();
            factory = serviceScope.ServiceProvider.GetRequiredService<IServiceFactory>();
        }

        T? item = factory is not null ? factory.Create<T>(parameters) : Factory.Create<T>(parameters);
        Add(item);

        return item;
    }

    public void Add(TItem item)
    {
        int index = collection.Count;
        InsertItem(index, item);
    }

    public void Add(object item)
    {
        int index = collection.Count;
        InsertItem(index, (TItem)item);
    }

    int IList.Add(object? value)
    {
        TItem? item = default;

        try
        {
            item = (TItem)value!;
        }
        catch (InvalidCastException)
        {

        }

        Add(item!);
        return Count - 1;
    }

    public void AddRange(IEnumerable<TItem> items)
    {
        foreach (TItem? item in items)
        {
            Add(item);
        }
    }

    public void BeginAggregation()
    {
        if (this.GetAttribute<AggerateAttribute>() is AggerateAttribute attribute)
        {
            if (attribute.Mode == AggerateMode.Reset)
            {
                Clear();
            }

            object? key = this.GetPropertyValue(() => attribute.Key) is { } value ? value : attribute.Key;
            Publisher.PublishUI(OnPrepareAggregation(key));
        }
    }

    public void Clear()
    {
        clearing = true;

        foreach (TItem item in this.ToList())
        {
            Disposer.Dispose(item);
            Disposer.Remove(this, item);
        }

        ClearItems();
        clearing = false;
    }

    public bool Contains(TItem item) =>
        collection.Contains(item);

    bool IList.Contains(object? value) =>
        IsCompatibleObject(value) && Contains((TItem)value!);

    public void CopyTo(TItem[] array, int index) =>
        collection.CopyTo(array, index);

    void ICollection.CopyTo(Array array, int index) =>
        collection.CopyTo((TItem[])array, index);

    public Task Deactivate()
    {
        DeactivateHandler?.Invoke(this, new EventArgs());
        return Task.CompletedTask;
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
        Disposer.Dispose(this);
    }

    public IEnumerator<TItem> GetEnumerator() =>
        collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        ((IEnumerable)collection).GetEnumerator();

    public Task Handle(RemoveEventArgs<TItem> args)
    {
        foreach (TItem item in this.ToList())
        {
            if (args.Value is not null && args.Value.Equals(item))
            {
                Remove(item);
            }
        }

        return Task.CompletedTask;
    }

    public Task Handle(CreateEventArgs<TItem> args)
    {
        if (args.Value is TItem item)
        {
            Add(item);
        }

        return Task.CompletedTask;
    }

    public Task Handle(InsertEventArgs<TItem> args)
    {
        if (args.Value is TItem item)
        {
            Insert(args.Index, item);

            if (item is ISelectable selectable)
            {
                if (selectable.Selected)
                {
                    SelectedItem = item;
                    SelectedIndex = this.IndexOf(item);
                }
            }
        }

        return Task.CompletedTask;
    }

    public Task Handle(MoveToEventArgs<TItem> args)
    {
        Move(args.OldIndex, args.NewIndex);
        return Task.CompletedTask;
    }

    public Task Handle(MoveEventArgs<TItem> args)
    {
        if (args.Value is TItem item)
        {
            Move(args.Index, item);
        }

        return Task.CompletedTask;
    }
    public Task Handle(ReplaceEventArgs<TItem> args)
    {
        if (args.Value is TItem item)
        {
            Replace(args.Index, item);
        }

        return Task.CompletedTask;
    }

    public Task Handle(RemoveAtEventArgs<TItem> args)
    {
        if (args.Index >= 0 && args.Index <= Count - 1)
        {
            RemoveAt(args.Index);
        }

        return Task.CompletedTask;
    }

    public int IndexOf(TItem item) =>
        collection.IndexOf(item);

    int IList.IndexOf(object? value) =>
        IsCompatibleObject(value) ?
        IndexOf((TItem)value!) : -1;

    public Task Initialize()
    {
        if (Initialized)
        {
            return Task.CompletedTask;
        }

        Initialized = true;
        BeginAggregation();

        return Task.CompletedTask;
    }

    public void Insert(int index, TItem item) =>
        InsertItem(index, item);

    void IList.Insert(int index,
        object? value)
    {
        if (value is TItem item)
        {
            Insert(index, item);
        }
    }

    public bool Move(int oldIndex, int newIndex)
    {
        if (oldIndex < 0)
        {
            return false;
        }

        TItem item = this[oldIndex];

        RemoveItem(oldIndex);
        Insert(newIndex, item);

        return true;
    }
    public bool Move(int index, TItem item)
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

    public virtual Task OnActivated() =>
        Task.CompletedTask;

    public virtual Task OnDeactivated() =>
        Task.CompletedTask;

    public virtual Task OnDeactivating() =>
        Task.CompletedTask;

    public bool Remove(TItem item)
    {
        int index = collection.IndexOf(item);
        if (index < 0)
        {
            return false;
        }
        
        Disposer.Dispose(item);
        Disposer.Remove(this, item);

        RemoveItem(index);

        return true;
    }

    void IList.Remove(object? value)
    {
        if (IsCompatibleObject(value))
        {
            Remove((TItem)value!);
        }
    }

    public void RemoveAt(int index) =>
        RemoveItem(index);

    public bool Replace(int index,
        TItem item)
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

    public void ResetAndAddRange(Action<ObservableCollection<TItem>> args)
    {
        Clear();
        args.Invoke(this);
    }

    protected virtual void ClearItems() =>
        collection.Clear();

    protected virtual void InsertItem(int index,
        TItem item)
    {
        Disposer.Add(this, item);
        Disposer.Add(item, Disposable.Create(() =>
        {
            if (item is IRemovable && !clearing)
            {
                if (item is IList collection)
                {
                    collection.Clear();
                }

                Remove(item);
            }
        }));

        collection.Insert(index > Count ? Count : index, item);
    }

    protected virtual IAggerate OnPrepareAggregation(object? key) =>
        new AggerateEventArgs<TItem>() with { Key = key };

    protected virtual void RemoveItem(int index) =>
        collection.RemoveAt(index);

    protected virtual void SetItem(int index, TItem item) =>
        collection[index] = item;

    private static bool IsCompatibleObject(object? value) =>
        (value is TItem) || (value == null && default(TItem) == null);

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args) =>
        CollectionChanged?.Invoke(this, args);

    partial void OnSelectedIndexChanged(int oldValue, int newValue)
    {
        if (oldValue >= 0 && oldValue <= this.Count -1 && this[oldValue] is ISelectable removed)
        {
            removed.Selected = false;
        }

        if (newValue >= 0 && newValue <= this.Count - 1 && this[newValue] is ISelectable added)
        {
            added.Selected = true;
        }
    }

    public Task Handle(SelectionEventArgs<TItem> args)
    {
        return Task.CompletedTask;
    }
}

public partial class ObservableCollection<TValue, TViewModel>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscription subscriber, IDisposer disposer) : ObservableCollection<TViewModel>(provider, factory, mediator, publisher, subscriber, disposer)
    where TViewModel : IDisposable
{
    [ObservableProperty]
    private TValue? value;
}

public class ObservableCollection(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscription subscriber,
    IDisposer disposer) :
    ObservableCollection<IDisposable>(provider, factory, mediator, publisher, subscriber, disposer);