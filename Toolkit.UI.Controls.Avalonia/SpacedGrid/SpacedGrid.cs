using Avalonia;
using Avalonia.Controls;
using System.Collections.Specialized;

namespace Toolkit.UI.Controls.Avalonia;

public class SpacedGrid : Grid
{
    public static readonly StyledProperty<double> ColumnSpacingProperty =
        AvaloniaProperty.Register<SpacedGrid, double>(nameof(ColumnSpacing), 3);

    public static readonly StyledProperty<double> RowSpacingProperty =
        AvaloniaProperty.Register<SpacedGrid, double>(nameof(RowSpacing), 3);

    public SpacedGrid() => Children.CollectionChanged += OnCollectionChanged;

    public double ColumnSpacing
    {
        get => GetValue(ColumnSpacingProperty);
        set => SetValue(ColumnSpacingProperty, value);
    }

    public double RowSpacing
    {
        get => GetValue(RowSpacingProperty);
        set => SetValue(RowSpacingProperty, value);
    }

    public IEnumerable<ColumnDefinition> UserDefinedColumnDefinitions =>
        ColumnDefinitions.Where(definition => definition is not ISpacingDefinition);

    public IEnumerable<RowDefinition> UserDefinedRowDefinitions =>
        RowDefinitions.Where(definition => definition is not ISpacingDefinition);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        RowDefinitions.CollectionChanged += delegate { UpdateSpacedRows(); };
        ColumnDefinitions.CollectionChanged += delegate { UpdateSpacedColumns(); };

        UpdateSpacedRows();
        UpdateSpacedColumns();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        switch (change.Property.Name)
        {
            case nameof(RowSpacing):
                RecalculateRowSpacing();
                break;

            case nameof(ColumnSpacing):
                RecalculateColumnSpacing();
                break;
        }
    }

    private void OnCollectionChanged(object? sender,
        NotifyCollectionChangedEventArgs args)
    {
        if (args.Action == NotifyCollectionChangedAction.Add || args.Action == NotifyCollectionChangedAction.Replace)
        {
            if (args.NewItems is not null)
            {
                foreach (Control item in args.NewItems)
                {
                    SetRow(item, GetRow(item) * 2);
                    SetRowSpan(item, GetRowSpan(item) * 2 - 1);

                    SetColumn(item, GetColumn(item) * 2);
                    SetColumnSpan(item, GetColumnSpan(item) * 2 - 1);
                }
            }
        }
    }

    private void OnInitialized(object? sender, EventArgs args)
    {
        if (sender is Control item)
        {
            item.Initialized -= OnInitialized;

            SetRow(item, GetRow(item) * 2);
            SetRowSpan(item, GetRowSpan(item) * 2 - 1);

            var d = GetColumn(item);

            SetColumn(item, GetColumn(item) * 2);
            SetColumnSpan(item, GetColumnSpan(item) * 2 - 1);
        }
    }

    private void RecalculateColumnSpacing()
    {
        foreach (ISpacingDefinition spacingColumn in ColumnDefinitions.OfType<ISpacingDefinition>())
        {
            spacingColumn.Spacing = ColumnSpacing;
        }
    }

    private void RecalculateRowSpacing()
    {
        foreach (ISpacingDefinition spacingRow in RowDefinitions.OfType<ISpacingDefinition>())
        {
            spacingRow.Spacing = RowSpacing;
        }
    }

    private void UpdateSpacedColumns()
    {
        List<ColumnDefinition> userColumnDefinitions = UserDefinedColumnDefinitions.ToList();
        ColumnDefinitions actualColumnDefinitions = [];

        int currentUserDefinition = 0;
        int currentActualDefinition = 0;

        while (currentUserDefinition < userColumnDefinitions.Count)
        {
            if (currentActualDefinition % 2 == 0)
            {
                actualColumnDefinitions.Add(userColumnDefinitions[currentUserDefinition]);
                currentUserDefinition++;
            }
            else
            {
                actualColumnDefinitions.Add(new SpacingColumnDefinition(ColumnSpacing));
            }

            currentActualDefinition++;
        }

        ColumnDefinitions = actualColumnDefinitions;
        ColumnDefinitions.CollectionChanged += delegate { UpdateSpacedColumns(); };
    }

    private void UpdateSpacedRows()
    {
        List<RowDefinition> userRowDefinitions = UserDefinedRowDefinitions.ToList();
        RowDefinitions actualRowDefinitions = [];

        int currentUserDefinition = 0;
        int currentActualDefinition = 0;

        while (currentUserDefinition < userRowDefinitions.Count)
        {
            if (currentActualDefinition % 2 == 0)
            {
                actualRowDefinitions.Add(userRowDefinitions[currentUserDefinition]);
                currentUserDefinition++;
            }
            else
            {
                actualRowDefinitions.Add(new SpacingRowDefinition(RowSpacing));
            }

            currentActualDefinition++;
        }

        RowDefinitions = actualRowDefinitions;
        RowDefinitions.CollectionChanged += delegate { UpdateSpacedRows(); };
    }
}