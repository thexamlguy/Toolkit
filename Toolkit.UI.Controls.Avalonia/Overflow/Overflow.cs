using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using Avalonia.Threading;
using FluentAvalonia.Core;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Toolkit.UI.Controls.Avalonia;

public class Overflow : 
    TemplatedControl
{
    public static readonly StyledProperty<ITemplate<Panel?>> ItemsPanelProperty =
        AvaloniaProperty.Register<Overflow, ITemplate<Panel?>>(nameof(ItemsPanel), new FuncTemplate<Panel?>(() => new StackPanel()));

    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<Overflow, IEnumerable?>(nameof(ItemsSource));

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<Overflow, IDataTemplate?>(nameof(ItemTemplate));

    public static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<Overflow, object?>(nameof(SelectedItem));

    private static readonly StyledProperty<OverflowTemplateSettings> TemplateSettingsProperty =
        AvaloniaProperty.Register<Overflow, OverflowTemplateSettings>(nameof(TemplateSettings));

    private readonly ObservableCollection<object> primaryCollection = [];

    private readonly ObservableCollection<object> secondaryCollection = [];

    private ListBox? primaryListBox;

    private ListBox? secondaryListBox;

    public Overflow()
    {
        SetValue(TemplateSettingsProperty, new OverflowTemplateSettings());

        TemplateSettings.GetPropertyChangedObservable(OverflowTemplateSettings.PrimarySelectionProperty)
            .AddClassHandler<OverflowTemplateSettings>(OnPrimarySelectionPropertyChanged);

        TemplateSettings.GetPropertyChangedObservable(OverflowTemplateSettings.SecondarySelectionProperty)
            .AddClassHandler<OverflowTemplateSettings>(OnSecondarySelectionPropertyChanged);
    }

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

    public OverflowTemplateSettings TemplateSettings
    {
        get => GetValue(TemplateSettingsProperty);
        set => SetValue(TemplateSettingsProperty, value);
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

        if (args.Property == SelectedItemProperty)
        {
            UpdateSelectedItem();
        }

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

    private void OnPrimarySelectionPropertyChanged(OverflowTemplateSettings sender,
        AvaloniaPropertyChangedEventArgs args)
    {
        object? selection = args.GetNewValue<object>();
        SetValue(SelectedItemProperty, selection);
    }

    private void OnSecondarySelectionPropertyChanged(OverflowTemplateSettings sender,
        AvaloniaPropertyChangedEventArgs args)
    {
        object? selection = args.GetNewValue<object>();
        SetValue(SelectedItemProperty, selection);
    }
    private void OnSourceCollectionChanged(object? sender,
        NotifyCollectionChangedEventArgs args)
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

            double controlWidth = 240;
            double accumulatedWidth = 0;
            double itemSpacing = 6;

            List<object> itemsToMoveToSecondary = [];

            for (int i = 0; i < primaryCollection.Count; i++)
            {
                object? item = primaryCollection[i];
                if (item is not null && primaryListBox?.ContainerFromItem(item) is ListBoxItem itemContainer)
                {
                    itemContainer.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    double itemWidth = itemContainer.DesiredSize.Width;

                    if (accumulatedWidth + itemWidth + (itemsToMoveToSecondary.Count * itemSpacing) > controlWidth)
                    {
                        itemsToMoveToSecondary.Add(item);
                    }
                    else
                    {
                        accumulatedWidth += itemWidth + itemSpacing;
                    }
                }
            }

            foreach (object item in itemsToMoveToSecondary)
            {
                primaryCollection.Remove(item);

                int insertIndexInSecondary = secondaryCollection.Count;
                if (ItemsSource.Contains(item))
                {
                    int indexInItemsSource = ItemsSource.IndexOf(item);
                    insertIndexInSecondary = Math.Min(indexInItemsSource, secondaryCollection.Count);
                }

                secondaryCollection.Insert(insertIndexInSecondary, item);
            }

            PseudoClasses.Set(":overflow", secondaryCollection is { Count: > 0 });
        });
    }

    private void UpdateSelectedItem()
    {
        if (SelectedItem is not null)
        {
            if (primaryCollection.Contains(SelectedItem))
            {
                TemplateSettings.SetValue(OverflowTemplateSettings.PrimarySelectionProperty, SelectedItem);
            }

            if (secondaryCollection.Contains(SelectedItem))
            {
                TemplateSettings.SetValue(OverflowTemplateSettings.SecondarySelectionProperty, SelectedItem);
            }
        }
        else
        {
            TemplateSettings.SetValue(OverflowTemplateSettings.PrimarySelectionProperty, null);
            TemplateSettings.SetValue(OverflowTemplateSettings.SecondarySelectionProperty, null);
        }
    }
}

