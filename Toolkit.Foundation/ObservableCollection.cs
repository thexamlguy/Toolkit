using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Specialized;
using System.Reactive.Disposables;

namespace Toolkit.Foundation;

public abstract partial class ObservableCollection<TViewModel> :
    ObservableRecipient,
    IObservableCollectionViewModel<TViewModel>,
    IList<TViewModel>,
    IList,
    IReadOnlyList<TViewModel>,
    INotifyCollectionChanged,
    ICollectionSynchronization<TViewModel>,
    IServiceProviderRequired,
    IServiceFactoryRequired,
    IMessengerRequired,
    IDisposerRequired,
    IRecipient<RemoveEventArgs<TViewModel>>,
    IRecipient<RemoveAtEventArgs<TViewModel>>,
    IRecipient<CreateEventArgs<TViewModel>>,
    IRecipient<InsertEventArgs<TViewModel>>,
    IRecipient<MoveEventArgs<TViewModel>>,
    IRecipient<MoveToEventArgs<TViewModel>>,
    IRecipient<ReplaceEventArgs<TViewModel>>,
    IActivation
    where TViewModel : notnull,
    IDisposable
{
    private readonly System.Collections.ObjectModel.ObservableCollection<TViewModel> collection = [];

    private readonly IDispatcher dispatcher;

    private readonly Dictionary<string, object> trackedProperties = [];

    [ObservableProperty]
    private int count;

    [ObservableProperty]
    private TViewModel? selectedItem;

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMessenger messenger,
        IDisposer disposer) : base(messenger)
    {
        Provider = provider;
        Factory = factory;
        Disposer = disposer;
        Messenger = messenger;

        dispatcher = Provider.GetRequiredService<IDispatcher>();
        collection.CollectionChanged += OnCollectionChanged;
    }

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMessenger messenger,
        IDisposer disposer,
        IEnumerable<TViewModel> items) : base(messenger)
    {
        Provider = provider;
        Factory = factory;
        Disposer = disposer;
        Messenger = messenger;

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

    public new IMessenger Messenger { get; private set; }

    public IServiceProvider Provider { get; private set; }

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

    public TViewModel Add<T>(params object?[] parameters)
        where T : TViewModel
    {
        T? item = Factory.Create<T>(parameters);

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
            Add(item);
        }

        SelectedItem = this.FirstOrDefault();
    }

    public void Clear(bool disposeItems = false)
    {
        if (disposeItems)
        {
            foreach (TViewModel item in this.ToList())
            {
                Disposer.Dispose(item);
                Disposer.Remove(this, item);
            }
        }

        ClearItems();
    }

    public void Clear()
    {
        foreach (TViewModel item in this.ToList())
        {
            Disposer.Dispose(item);
            Disposer.Remove(this, item);
        }

        ClearItems();
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

    public virtual void Dispose()
    {
        Disposer.Dispose(this);
        GC.SuppressFinalize(this);
    }

    public IEnumerator<TViewModel> GetEnumerator() =>
        collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        ((IEnumerable)collection).GetEnumerator();

    public int IndexOf(TViewModel item) =>
        collection.IndexOf(item);

    int IList.IndexOf(object? value) =>
        IsCompatibleObject(value) ?
        IndexOf((TViewModel)value!) : -1;

    public TViewModel Insert<T>(int index = 0,
        params object?[] parameters)
        where T :
        TViewModel
    {
        T? item = Factory.Create<T>(parameters);

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

    public void Receive(RemoveEventArgs<TViewModel> args)
    {
        foreach (TViewModel item in this.ToList())
        {
            if (args.Sender is not null && args.Sender.Equals(item))
            {
                Remove(item);
            }
        }
    }

    public void Receive(CreateEventArgs<TViewModel> args)
    {
        if (args.Sender is TViewModel item)
        {
            Add(item);
        }
    }

    public void Receive(InsertEventArgs<TViewModel> args)
    {
        if (args.Sender is TViewModel item)
        {
            Insert(args.Index, item);
        }
    }

    public void Receive(MoveToEventArgs<TViewModel> args)
    {
        Move(args.OldIndex, args.NewIndex);
    }

    public void Receive(MoveEventArgs<TViewModel> args)
    {
        if (args.Sender is TViewModel item)
        {
            Move(args.Index, item);
        }
    }

    public void Receive(ReplaceEventArgs<TViewModel> args)
    {
        if (args.Sender is TViewModel item)
        {
            Replace(args.Index, item);
        }
    }

    public void Receive(RemoveAtEventArgs<TViewModel> args)
    {
        int index = args.Index;
        if (index >= 0 && index <= Count - 1)
        {
            RemoveAt(index);
        }
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

    public bool Replace<T>(int index,
        params object?[] parameters)
        where T :
        TViewModel
    {
        if (index <= Count - 1)
        {
            RemoveItem(index);
        }
        else
        {
            index = Count;
        }

        T? item = Factory.Create<T>(parameters);

        Insert(index, item);
        return true;
    }

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

    public void Track<T>(string propertyName, Func<T> getter, Action<T> setter)
    {
        if (!trackedProperties.ContainsKey(propertyName))
        {
            T initialValue = getter();
            trackedProperties[propertyName] = new TrackedProperty<T>(initialValue, setter, getter);
        }
    }

    protected virtual void ClearItems() =>
        collection.Clear();

    protected virtual void InsertItem(int index,
        TViewModel item)
    {
        Disposer.Add(this, item);
        Disposer.Add(item, Disposable.Create(() =>
        {
            if (item is IDisposable)
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

    protected override sealed void OnActivated()
    {
        Messenger.RegisterAll(this);
        Activated();
    }

    protected override sealed void OnDeactivated()
    {
        Messenger.UnregisterAll(this);        
        Deactivated();

        Dispose();
    }

    protected virtual void Activated()
    {
    }

    protected virtual void Deactivated()
    {
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
                }
                break;
        }
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
        IMessenger messenger,
        IDisposer disposer,
        TValue? value = default) : base(provider, factory, messenger, disposer)
    {
        Value = value;
    }

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMessenger messenger,
        IDisposer disposer,
        IEnumerable<TViewModel> items,
        TValue? value = default) : base(provider, factory, messenger, disposer, items)
    {
        Value = value;
    }

    protected virtual void OnChanged(TValue? value)
    {
    }

    partial void OnValueChanged(TValue? value) => OnChanged(value);
}

public partial class ObservableCollection<TKey, TValue, TViewModel> :
    ObservableCollection<TViewModel>
    where TViewModel : IDisposable
{
    [ObservableProperty]
    private TKey key;

    [ObservableProperty]
    private TValue? value;

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMessenger messenger,
        IDisposer disposer,
        TKey key,
        TValue? value = default) : base(provider, factory, messenger, disposer)
    {
        Key = key;
        Value = value;
    }

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMessenger messenger,
        IDisposer disposer,
        IEnumerable<TViewModel> items,
        TKey key,
        TValue? value = default) : base(provider, factory, messenger, disposer, items)
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
        IMessenger messenger,
        IDisposer disposer) : base(provider, factory, messenger, disposer)
    {

    }

    public ObservableCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMessenger messenger,
        IDisposer disposer,
        IEnumerable<IDisposable> items) : base(provider, factory, messenger, disposer, items)
    {

    }
}