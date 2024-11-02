using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using System.Collections.Specialized;
using System.Numerics;

namespace Toolkit.UI.Controls.Avalonia;

public class CarouselView :
    ItemsControl
{
    private readonly TimeSpan animationDuration = TimeSpan.FromMilliseconds(500);
    private readonly List<ExpressionAnimation> animations = [];
    private readonly int columnCount = 5;
    private readonly List<CompositionVisual> itemVisuals = [];
    private readonly ScopedBatchHelper scopedBatch = new();
    private readonly double spacing = 12;
    private Compositor? compositor;
    private Grid? container;
    private Vector3D finalOffset;
    private float horizontalDelta;
    private Rectangle? indicator;
    private Vector3DKeyFrameAnimation? indicatorAnimation;
    private CompositionVisual? indicatorVisual;
    private bool isAnimating;
    private bool isPressed;
    private List<Border>? items;
    private Point? lastPosition;
    private int newIndex;
    private int SelectedIndex;
    private Point? startPosition;
    private CompositionVisual? touchAreaVisual;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs args)
    {
        container = args.NameScope.Get<Grid>("Container");
        if (container is not null)
        {
            items = container.Children.OfType<Border>().ToList();
            foreach (Border item in items)
            {
                if (item.Child is CarouselViewItem contentControl)
                {
                    contentControl.ContentTemplate = ItemTemplate;
                }
            }
        }

        indicator = args.NameScope.Get<Rectangle>("Indicator");
        if (indicator is not null)
        {
            indicatorVisual = ElementComposition.GetElementVisual(indicator);
        }

        ItemsView.CollectionChanged -= OnCollectionChanged;
        ItemsView.CollectionChanged += OnCollectionChanged;

        base.OnApplyTemplate(args);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs args)
    {
        base.OnSizeChanged(args);
        ArrangeItems(newIndex, isAnimating: false);
    }

    protected override void OnLoaded(RoutedEventArgs args)
    {
        if (container is not null
            && items is not null
            && indicator is not null)
        {
            indicatorVisual = ElementComposition.GetElementVisual(indicator);
            touchAreaVisual = ElementComposition.GetElementVisual(container);
            if (touchAreaVisual is not null)
            {
                compositor = touchAreaVisual.Compositor;
            }

            itemVisuals.Clear();
            foreach (Border item in items)
            {
                if (ElementComposition.GetElementVisual(item) is CompositionVisual visual)
                {
                    itemVisuals.Add(visual);
                }
            }

            ArrangeItems(newIndex);
        }

        base.OnLoaded(args);
    }

    protected override void OnPointerMoved(PointerEventArgs args)
    {
        if (isPressed && indicatorVisual is not null && startPosition.HasValue)
        {
            lastPosition = args.GetPosition(container);
            horizontalDelta = (float)(lastPosition.Value.X - startPosition.Value.X);

            indicatorVisual.Offset = new Vector3(horizontalDelta, 0.0f, 0.0f);
        }

        base.OnPointerMoved(args);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs args)
    {
        if (!isPressed && indicatorVisual is not null)
        {
            if (!isAnimating)
            {
                horizontalDelta = 0;

                isPressed = true;
                startPosition = args.GetPosition(container);

                indicatorVisual.Offset = new Vector3(horizontalDelta, 0.0f, 0.0f);
                PrepareAnimations();

                for (int i = 0; i < itemVisuals.Count; i++)
                {
                    itemVisuals[i].StartAnimation("Offset", animations[i]);
                }
            }
        }

        base.OnPointerPressed(args);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs args)
    {
        if (isPressed && container is not null
            && items is not null
            && indicatorVisual is not null)
        {
            isPressed = false;

            double itemWidth = items[0].Bounds.Width;
            double threshold = itemWidth / 3;

            int oldSelectedIndex = newIndex;
            double offset = indicatorVisual.Offset.X;

            if (offset <= -threshold)
            {
                newIndex = (newIndex + 1) % 5;
                SelectedIndex = (SelectedIndex + 1) % ItemsView.Count;
            }

            if (offset >= threshold)
            {
                newIndex = (newIndex + 4) % 5;
                SelectedIndex = (SelectedIndex + ItemsView.Count - 1) % ItemsView.Count;
            }

            ArrangeItems(newIndex, oldSelectedIndex, true);
        }

        base.OnPointerReleased(args);
    }

    private void ArrangeItems(int newIndex,
        int oldIndex = -1,
        bool isAnimating = false)
    {
        if (compositor is not null
            && container is not null
            && items is not null
            && indicatorVisual is not null)
        {
            double containerHeight = Bounds.Height;
            double containerWidth = Bounds.Width;
            container.Height = containerHeight;

            double targetSize = containerHeight;

            foreach (Border item in items)
            {
                if (item.Child is CarouselViewItem content)
                {
                    content.Width = targetSize;
                    content.Height = targetSize;
                }

                item.Width = targetSize;
                item.Height = targetSize;
            }

            double centreLeft = (containerWidth - targetSize) / 2;
            double leftLeft = -targetSize + centreLeft;
            double rightLeft = containerWidth - centreLeft;

            double[] offsets =
            [
                leftLeft - targetSize + spacing * 1,
                leftLeft + spacing * 2,
                centreLeft + spacing * 3,
                rightLeft + spacing * 4,
                rightLeft + targetSize + spacing * 5
            ];

            double centreOffset = spacing * (columnCount - 1) / 2 + spacing;
            if (!isAnimating)
            {
                for (int i = 0; i < columnCount; i++)
                {
                    itemVisuals[(newIndex + i - 2 + columnCount) % columnCount].Offset =
                        new Vector3((float)(offsets[i] - centreOffset), 0, 100);
                }

                SetItems();
            }
            else
            {
                int difference = newIndex - oldIndex;
                finalOffset = difference switch
                {
                    0 => new Vector3D(0, 0, 0),
                    1 => new Vector3D((float)(-targetSize - spacing), 0, 0),
                    -1 => new Vector3D((float)(targetSize + spacing), 0, 0),
                    _ => new Vector3D((float)(targetSize * Math.Sign(difference) +
                        spacing * Math.Sign(difference)), 0, 0)
                };

                indicatorAnimation = compositor.CreateVector3DKeyFrameAnimation();
                indicatorAnimation.InsertKeyFrame(1.0f, finalOffset);
                indicatorAnimation.Duration = animationDuration;
                indicatorAnimation.StopBehavior = AnimationStopBehavior.LeaveCurrentValue;
                SetItems();

                scopedBatch.Completed += () =>
                {
                    this.isAnimating = false;
                    for (int i = 0; i < columnCount; i++)
                    {
                        itemVisuals[(newIndex + i - 2 + columnCount) % columnCount].Offset =
                            new Vector3((float)(offsets[i] - centreOffset), 0, 0);
                    }
                };

                indicatorVisual.StartAnimation("Offset", indicatorAnimation);
                scopedBatch.Start(animationDuration);

                this.isAnimating = true;
            }
        }
    }

    private void OnCollectionChanged(object? sender,
        NotifyCollectionChangedEventArgs args) => ArrangeItems(newIndex);

    private void PrepareAnimations()
    {
        animations.Clear();
        if (compositor is not null && indicatorVisual is not null && itemVisuals is not null)
        {
            for (int i = 0; i < itemVisuals.Count; i++)
            {
                ExpressionAnimation animation = compositor.CreateExpressionAnimation();

                animation.Expression = $"Source.Offset + Vector3({itemVisuals[i].Offset.X}, 0, 0)";
                animation.SetReferenceParameter("Source", indicatorVisual);
                animations.Add(animation);
            }
        }
    }

    private void SetItems()
    {
        if (items is not null)
        {
            int itemCount = ItemsView.Count;
            if (itemCount == 0)
            {
                return;
            }

            int[] selectedIndexOffsets = new int[columnCount];
            int[] indexOffsets = new int[columnCount];

            SelectedIndex = SelectedIndex < 0 ? 0 : SelectedIndex;

            for (int i = -2; i <= 2; i++)
            {
                selectedIndexOffsets[i + 2] = (SelectedIndex + i + itemCount) % itemCount;
                indexOffsets[i + 2] = (newIndex + i + columnCount) % columnCount;
            }

            for (int i = 0; i < columnCount; i++)
            {
                int index = selectedIndexOffsets[i];
                if (itemCount == 1)
                {
                    index = 0;
                }

                if (items[indexOffsets[i]] is Border border && border.Child is
                    CarouselViewItem content)
                {
                    content.Content = ItemsView[index];
                    content.SetSelected(indexOffsets.Length / 2 == i);
                }
            }
        }
    }
}