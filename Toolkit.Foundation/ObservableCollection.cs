using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Specialized;
using System.Reactive.Disposables;

namespace Toolkit.Foundation;

public partial class ObservableCollection<TViewModel> :
    ObservableObject,
    IObservableCollectionViewModel<TViewModel>,
    IInitialization,
    IActivated,
    IDeactivating,
    IDeactivated,
    IList<TViewModel>,
    IList,
    IReadOnlyList<TViewModel>,
    INotifyCollectionChanged,
    ICollectionSynchronization<TViewModel>,
    IServiceProviderRequired,
    IServiceFactoryRequired,
    IMediatorRequired,
    IPublisherRequired,
    IDisposerRequired,
    INotificationHandler<RemoveEventArgs<TViewModel>>,
    INotificationHandler<RemoveAtEventArgs<TViewModel>>,
    INotificationHandler<CreateEventArgs<TViewModel>>,
    INotificationHandler<InsertEventArgs<TViewModel>>,
    INotificationHandler<MoveEventArgs<TViewModel>>,
    INotificationHandler<MoveToEventArgs<TViewModel>>,
    INotificationHandler<ReplaceEventArgs<TViewModel>>,
    INotificationHandler<SelectionEventArgs<TViewModel>>
    where TViewModel : notnull,
    IDisposable
{
    private readonly System.Collections.ObjectModel.ObservableCollection<TViewModel> collection = [];

    private readonly IDispatcher dispatcher;

    private readonly Queue<object> pendingEvents = [];

    private readonly Dictionary<string, object> trackedProperties = [];

    [ObservableProperty]
    private int count;

    [ObservableProperty]
    private bool isActivated;

    private bool isClearing;

    [ObservableProperty]
    private bool isInitialized;

    [ObservableProperty]
    private TViewModel? selectedItem;

    public ObservableCollection(IServiceProvider provider,
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
        Subscriber = subscriber;
        Disposer = disposer;

        dispatcher = Provider.GetRequiredService<IDispatcher>();
        collection.CollectionChanged += OnCollectionChanged;
    }

    public ObservableCollection(IServiceProvider provider,
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
        Subscriber = subscriber;
        Disposer = disposer;

        dispatcher = Provider.GetRequiredService<IDispatcher>();
        collection.CollectionChanged += OnCollectionChanged;

        AddRange(items);
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public IDisposer Disposer { get; private set; }

    public IServiceFactory Factory { get; private set; }

    bool IList.IsFixedSize => false;

    bool ICollection<TViewModel>.IsReadOnly => false;

    bool IList.IsReadOnly => false;

    bool ICollection.IsSynchronized => false;

    public IMediator Mediator { get; }

    public IServiceProvider Provider { get; private set; }

    public IPublisher Publisher { get; private set; }

    public ISubscriber Subscriber { get; }

    object ICollection.SyncRoot => this;

    public TViewModel this[int index]
    {
        get => collection[index];
        set => SetItem(index, value);
    }

    object? IList.this[int index]
    {
        get => index >= 0 && collection.Count > 0 ? collection[index] : null;
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

    private Func<TViewModel>? defaultSelectionFactory;

    public void SetSource(IList<TViewModel> source,
        Func<TViewModel>? defaultSelectionFactory)
    {
        foreach (TViewModel item in source)
        {
            Add(item);
        }

        if (defaultSelectionFactory is not null)
        {
            this.defaultSelectionFactory = defaultSelectionFactory;
            SelectedItem = defaultSelectionFactory.Invoke();
        }

        if (source is INotifyCollectionChanged observableSource)
        {
            observableSource.CollectionChanged -= SourceCollectionChanged;
            observableSource.CollectionChanged += SourceCollectionChanged;
        }
    }

    private void SourceCollectionChanged(object? sender,
        NotifyCollectionChangedEventArgs args)
    {
        switch (args.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (args.NewItems is not null)
                {
                    foreach (TViewModel newItem in args.NewItems)
                    {
                        Add(newItem);
                    }
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                if (args.OldItems is not null)
                {
                    foreach (TViewModel oldItem in args.OldItems)
                    {
                        if (this.FirstOrDefault(x => x.Equals(oldItem)) is TViewModel removedItem)
                        {
                            Remove(removedItem);
                        }
                    }
                }
                break;

            case NotifyCollectionChangedAction.Reset:

                Clear();
                if (sender is IEnumerable<TViewModel> collection)
                {
                    foreach (TViewModel item in collection)
                    {
                        Add(item);
                    }

                    if (defaultSelectionFactory is not null)
                    {
                        SelectedItem = defaultSelectionFactory.Invoke();
                    }
                }
                break;
        }
    }

    public virtual Task OnActivated()
    {
        IsActivated = true;
        while (pendingEvents.Count > 0)
        {
            object current = pendingEvents.Dequeue();
            Handle((dynamic)current);
        }

        return Task.CompletedTask;
    }

    public TViewModel Add<T>(params object?[] parameters)
        where T : TViewModel
    {
        T? item = Factory.Create<T>(args =>
        {
            if (args is IInitialization initialization)
            {
                initialization.Initialize();
            }
        }, parameters);

        Add(item);
        return item;
    }

    public void Add(TViewModel item)
    {
        int index = collection.Count;
        InsertItem(index, item);

        UpdateSelection(item);
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
            if (item is IInitialization initialization)
            {
                initialization.Initialize();
            }

            Add(item);
        }
    }

    public void Reset(Action<ObservableCollection<TViewModel>> factory, bool disposeItems = true)
    {
        SelectedItem = default;

        Clear(disposeItems);
        factory.Invoke(this);
    }

    public void Clear(bool disposeItems = false)
    {
        isClearing = true;
        if (disposeItems)
        {
            foreach (TViewModel item in this.ToList())
            {
                Disposer.Dispose(item);
                Disposer.Remove(this, item);
            }
        }

        ClearItems();
        isClearing = false;
    }

    public void Clear()
    {
        isClearing = true;
        foreach (TViewModel item in this.ToList())
        {
            Disposer.Dispose(item);
            Disposer.Remove(this, item);
        }

        ClearItems();
        isClearing = false;
    }

    public void Commit()
    {
        foreach (object trackedProperty in trackedProperties.Values)
        {
            ((dynamic)trackedProperty).Commit();
        }
    }

    public bool Contains(TViewModel item) =>
        collection.Contains(item);

    bool IList.Contains(object? value) =>
        IsCompatibleObject(value) && Contains((TViewModel)value!);

    public void CopyTo(TViewModel[] array, int index) =>
        collection.CopyTo(array, index);

    void ICollection.CopyTo(Array array, int index) =>
        collection.CopyTo((TViewModel[])array, index);

    public virtual Task OnDeactivated()
    {
        IsActivated = false;
        return Task.CompletedTask;
    }

    public virtual Task OnDeactivating() =>
        Task.CompletedTask;

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
        Disposer.Dispose(this);
    }

    public void Activate(Func<ActivationBuilder> activateDelegate,
        bool reset = false)
    {
        if (reset)
        {
            Clear();
        }

        ActivationBuilder builder = activateDelegate.Invoke();
        Publisher.Publish(builder.Value, builder.Key);
    }

    public IEnumerator<TViewModel> GetEnumerator() =>
        collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        ((IEnumerable)collection).GetEnumerator();

    public Task Handle(RemoveEventArgs<TViewModel> args)
    {
        if (IsActivated)
        {
            foreach (TViewModel item in this.ToList())
            {
                if (args.Sender is not null && args.Sender.Equals(item))
                {
                    Remove(item);
                }
            }
        }
        else
        {
            pendingEvents.Enqueue(args);
        }

        return Task.CompletedTask;
    }

    public Task Handle(CreateEventArgs<TViewModel> args)
    {
        if (IsActivated)
        {
            if (args.Sender is TViewModel item)
            {
                Add(item);
            }
        }
        else
        {
            pendingEvents.Enqueue(args);
        }

        return Task.CompletedTask;
    }

    public Task Handle(InsertEventArgs<TViewModel> args)
    {
        if (IsActivated)
        {
            if (args.Sender is TViewModel item)
            {
                Insert(args.Index, item);
            }
        }
        else
        {
            pendingEvents.Enqueue(args);
        }

        return Task.CompletedTask;
    }

    public Task Handle(MoveToEventArgs<TViewModel> args)
    {
        if (IsActivated)
        {
            Move(args.OldIndex, args.NewIndex);
        }
        else
        {
            pendingEvents.Enqueue(args);
        }

        return Task.CompletedTask;
    }

    public Task Handle(MoveEventArgs<TViewModel> args)
    {
        if (IsActivated)
        {
            if (args.Sender is TViewModel item)
            {
                Move(args.Index, item);
            }
        }
        else
        {
            pendingEvents.Enqueue(args);
        }

        return Task.CompletedTask;
    }

    public Task Handle(ReplaceEventArgs<TViewModel> args)
    {
        if (IsActivated)
        {
            if (args.Sender is TViewModel item)
            {
                Replace(args.Index, item);
            }
        }
        else
        {
            pendingEvents.Enqueue(args);
        }

        return Task.CompletedTask;
    }

    public Task Handle(RemoveAtEventArgs<TViewModel> args)
    {
        if (IsActivated)
        {
            int index = args.Index;
            if (index >= 0 && index <= Count - 1)
            {
                RemoveAt(index);
            }
        }
        else
        {
            pendingEvents.Enqueue(args);
        }

        return Task.CompletedTask;
    }

    public Task Handle(SelectionEventArgs<TViewModel> args) =>
        Task.CompletedTask;

    public int IndexOf(TViewModel item) =>
        collection.IndexOf(item);

    int IList.IndexOf(object? value) =>
        IsCompatibleObject(value) ?
        IndexOf((TViewModel)value!) : -1;

    public virtual void OnInitialize()
    {
    }

    [RelayCommand]
    public virtual void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }

        IsInitialized = true;
        Subscriber.Subscribe(this);
        OnInitialize();

        Activate();
    }

    public TViewModel Insert<T>(int index = 0,
        params object?[] parameters)
        where T :
        TViewModel
    {
        T? item = Factory.Create<T>(args =>
        {
            if (args is IInitialization initialization)
            {
                initialization.Initialize();
            }
        }, parameters);

        InsertItem(index, item);
        UpdateSelection(item);

        return item;
    }

    public void Insert(int index,
        TViewModel item)
    {
        InsertItem(index, item);
        UpdateSelection(item);
    }

    void IList.Insert(int index,
        object? value)
    {
        if (value is TViewModel item)
        {
            Insert(index, item);
            UpdateSelection(item);
        }
    }

    public bool Move(int oldIndex, int newIndex)
    {
        if (oldIndex < 0)
        {
            return false;
        }

        TViewModel item = this[oldIndex];

        bool moveSelection = false;
        if (item is ISelectable oldSelection)
        {
            if (oldSelection.IsSelected)
            {
                moveSelection = true;
                SelectedItem = default;
            }
        }

        RemoveItem(oldIndex);
        InsertItem(newIndex, item);

        if (moveSelection)
        {
            if (item is ISelectable newSelection)
            {
                newSelection.IsSelected = true;
                dispatcher.Invoke(() => SelectedItem = item);
            }
        }

        return true;
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
        Disposer.Remove(this, item);

        TViewModel? oldSelection = SelectedItem;
        RemoveItem(index);

        if (item.Equals(oldSelection))
        {
            int newIndex = Math.Min(index, Count - 1);
            TViewModel? selectedItem = newIndex >= 0 ? this[newIndex] : default;
            dispatcher.Invoke(() => SelectedItem = selectedItem);
        }

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

    public void Revert()
    {
        foreach (object trackedProperty in trackedProperties.Values)
        {
            ((dynamic)trackedProperty).Revert();
        }
    }

    public void Activate(bool reset = false)
    {
        if (reset)
        {
            Clear();
        }

        ActivationBuilder builder = ActivationBuilder();
        Publisher.PublishUI(builder.Value, builder.Key);
    }

    public void Track<T>(string propertyName, Func<T> getter, Action<T> setter)
    {
        if (!trackedProperties.ContainsKey(propertyName))
        {
            T initialValue = getter();
            trackedProperties[propertyName] = new TrackedProperty<T>(initialValue, setter, getter);
        }
    }

    protected virtual ActivationBuilder ActivationBuilder() =>
        new(new ActivationEventArgs<TViewModel>());

    protected virtual void ClearItems() =>
        collection.Clear();

    protected virtual void InsertItem(int index,
        TViewModel item)
    {
        Disposer.Add(this, item);
        Disposer.Add(item, Disposable.Create(() =>
        {
            if (item is IRemovable && !isClearing)
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

    protected virtual void RemoveItem(int index) =>
        collection.RemoveAt(index);

    protected virtual void SetItem(int index, TViewModel item) =>
        collection[index] = item;

    private static bool IsCompatibleObject(object? value) =>
        (value is TViewModel) || (value == null && default(TViewModel) == null);

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        Count = collection.Count;
        CollectionChanged?.Invoke(this, args);
    }

    partial void OnIsActivatedChanged(bool value)
    {
        if (value)
        {
            while (pendingEvents.Count > 0)
            {
                object current = pendingEvents.Dequeue();
                Handle((dynamic)current);
            }
        }
    }

    partial void OnSelectedItemChanged(TViewModel? oldValue, TViewModel? newValue)
    {
        if (oldValue is ISelectable oldSelection)
        {
            oldSelection.IsSelected = false;
        }

        if (newValue is ISelectable newSelection)
        {
            newSelection.IsSelected = true;
        }

        Publisher.Publish(Selection.As(SelectedItem));
        OnSelectedItemChanged();
    }

    protected virtual void OnSelectedItemChanged()
    {
    }

    private void UpdateSelection(TViewModel item)
    {
        if (item is ISelectable newSelection)
        {
            if (newSelection.IsSelected)
            {
                if (SelectedItem is ISelectable oldSelection)
                {
                    oldSelection.IsSelected = false;
                }

                dispatcher.Invoke(() => SelectedItem = item);
            }
        }
    }
}

public partial class ObservableCollection<TValue, TViewModel> :
    ObservableCollection<TViewModel>
    where TViewModel : IDisposable
{
    [ObservableProperty]
    private TValue? value;

    public ObservableCollection(IServiceProvider provider, 
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher, 
        ISubscriber subscriber,
        IDisposer disposer,
        TValue? value = default) : base(provider, factory, mediator, publisher, subscriber, disposer)
    {
        Value = value;
    }

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory, 
        IMediator mediator,
        IPublisher publisher,
        ISubscriber subscriber, 
        IDisposer disposer, 
        IEnumerable<TViewModel> items,
        TValue? value = default) : base(provider, factory, mediator, publisher, subscriber, disposer, items)
    {
        Value = value;
    }

    protected virtual void OnChanged(TValue? value)
    {

    }

    partial void OnValueChanged(TValue? value) => OnChanged(value);
}

public partial class ObservableCollection<TViewModel, TKey, TValue> :
    ObservableCollection<TViewModel>
    where TViewModel : IDisposable
{
    [ObservableProperty]
    private TKey key;

    [ObservableProperty]
    private TValue? value;

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer,
        TKey key,
        TValue? value = default) : base(provider, factory, mediator, publisher, subscriber, disposer)
    {
        Key = key;
        Value = value;
    }

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer,
        IEnumerable<TViewModel> items,
        TKey key,
        TValue? value = default) : base(provider, factory, mediator, publisher, subscriber, disposer, items)
    {
        Key = key;
        Value = value;
    }
}

public class ObservableCollection :
    ObservableCollection<IDisposable>
{
    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer) : base(provider, factory, mediator, publisher, subscriber, disposer)
    {
    }

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer,
        IEnumerable<IDisposable> items) : base(provider, factory, mediator, publisher, subscriber, disposer, items)
    {
    }
}