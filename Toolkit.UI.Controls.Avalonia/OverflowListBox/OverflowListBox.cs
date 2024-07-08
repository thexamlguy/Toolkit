using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Metadata;
using Avalonia.Threading;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Toolkit.UI.Controls.Avalonia;

public class OverflowListBox : 
    TemplatedControl
{
    public static readonly StyledProperty<ITemplate<Panel?>> ItemsPanelProperty =
        AvaloniaProperty.Register<OverflowListBox, ITemplate<Panel?>>(nameof(ItemsPanel), new FuncTemplate<Panel?>(() => new StackPanel()));

    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<OverflowListBox, IEnumerable?>(nameof(ItemsSource));

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<OverflowListBox, IDataTemplate?>(nameof(ItemTemplate));

    public static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<OverflowListBox, object?>(nameof(SelectedItem), BindingMode.TwoWay);

    private readonly ObservableCollection<object> primaryCollection = new();
    private readonly ObservableCollection<object> secondaryCollection = new();

    private ListBox? primaryListBox;
    private ListBox? secondaryListBox;

    public ITemplate<Panel?> ItemsPanel
    {
        get => GetValue(ItemsPanelProperty);
        set => SetValue(ItemsPanelProperty, value);
    }

    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs args)
    {
        base.OnApplyTemplate(args);

        primaryListBox = args.NameScope.Get<ListBox>("PrimaryListBox");
        primaryListBox?.SetValue(ItemsControl.ItemsSourceProperty, primaryCollection);

        secondaryListBox = args.NameScope.Get<ListBox>("SecondaryListBox");
        secondaryListBox?.SetValue(ItemsControl.ItemsSourceProperty, secondaryCollection);

        InitializeCollections();
        UpdateOverflow();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        base.OnPropertyChanged(args);
        if (args.Property == ItemsSourceProperty)
        {
            if (args.OldValue is IEnumerable oldCollection && oldCollection is INotifyCollectionChanged oldNotifyCollectionChanged)
            {
                oldNotifyCollectionChanged.CollectionChanged -= OnSourceCollectionChanged;
            }

            if (args.NewValue is IEnumerable newCollection && newCollection is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += OnSourceCollectionChanged;
            }

            InitializeCollections();
            UpdateOverflow();
        }
    }

    private void InitializeCollections()
    {
        primaryCollection.Clear();
        secondaryCollection.Clear();

        if (ItemsSource is not null)
        {
            foreach (object? item in ItemsSource)
            {
                primaryCollection.Add(item);
            }
        }
    }

    private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        switch (args.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (args.NewItems is not null)
                {
                    int insertIndex = args.NewStartingIndex > primaryCollection.Count ? primaryCollection.Count : args.NewStartingIndex;
                    foreach (object? newItem in args.NewItems)
                    {
                        primaryCollection.Insert(insertIndex++, newItem);
                    }
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                if (args.OldItems is not null)
                {
                    foreach (object? oldItem in args.OldItems)
                    {
                        primaryCollection.Remove(oldItem);
                        secondaryCollection.Remove(oldItem);
                    }
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                if (args.OldItems is not null && args.NewItems is not null && args.OldItems.Count == args.NewItems.Count)
                {
                    for (int i = 0; i < args.OldItems.Count; i++)
                    {
                        if (args.OldItems[i] is object oldItem &&
                            args.NewItems[i] is object newItem)
                        {
                            int index = primaryCollection.IndexOf(oldItem);
                            if (index != -1)
                            {
                                primaryCollection[index] = newItem;
                            }

                            index = secondaryCollection.IndexOf(oldItem);
                            if (index != -1)
                            {
                                secondaryCollection[index] = newItem;
                            }
                        }
                    }
                }
                break;

            case NotifyCollectionChangedAction.Move:
                if (args.OldItems != null && args.NewItems != null && args.OldItems.Count == args.NewItems.Count)
                {
                    for (int i = 0; i < args.OldItems.Count; i++)
                    {
                        if (args.OldItems[i] is object item)
                        {
                            int oldIndex = primaryCollection.IndexOf(item);
                            if (oldIndex != -1)
                            {
                                primaryCollection.RemoveAt(oldIndex);

                                int newIndex = args.NewStartingIndex + i;
                                primaryCollection.Insert(newIndex, item);
                            }

                            oldIndex = secondaryCollection.IndexOf(item);
                            if (oldIndex != -1)
                            {
                                secondaryCollection.RemoveAt(oldIndex);

                                int newIndex = args.NewStartingIndex + i;
                                secondaryCollection.Insert(newIndex, item);
                            }
                        }
                    }
                }
                break;

            case NotifyCollectionChangedAction.Reset:
                InitializeCollections();
                break;
        }

        UpdateOverflow();
    }

    private void UpdateOverflow()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (ItemsSource is null)
            {
                return;
            }

            double controlWidth = primaryListBox?.DesiredSize.Width ?? 0;
            double accumulatedWidth = 0;
            double itemSpacing = 6;

            List<(object item, int originalIndex)> itemsToMoveToSecondary = new();

            for (int i = 0; i < primaryCollection.Count; i++)
            {
                object? item = primaryCollection[i];
                if (item is not null && primaryListBox?.ContainerFromItem(item) is ListBoxItem itemContainer)
                {
                    itemContainer.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    double itemWidth = itemContainer.DesiredSize.Width;

                    if (accumulatedWidth + itemWidth + (itemsToMoveToSecondary.Count * itemSpacing) > controlWidth)
                    {
                        itemsToMoveToSecondary.Add((item, i));
                    }
                    else
                    {
                        accumulatedWidth += itemWidth + itemSpacing;
                    }
                }
            }

            foreach (var (item, originalIndex) in itemsToMoveToSecondary.OrderByDescending(x => x.originalIndex))
            {
                primaryCollection.Remove(item);
                int insertIndexInSecondary = originalIndex - primaryCollection.Count;
                secondaryCollection.Insert(insertIndexInSecondary, item);
            }
        });
    }

}

