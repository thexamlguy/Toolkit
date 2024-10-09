using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Toolkit.UI.Controls.Avalonia;

public class ContentColorPicker : ContentControl
{
    private Canvas? canvas;
    private Border? preview;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs args)
    {
        base.OnApplyTemplate(args);

        canvas = args.NameScope.Find<Canvas>("Canvas");

        if (canvas is not null)
        {
            canvas.PointerMoved += OnPointerMoved;
            canvas.PointerExited += OnPointerExited;
            canvas.PointerEntered += OnPointerEntered;
        }

        preview = args.NameScope.Find<Border>("Preview");
    }

    private void OnPointerMoved(object? sender, 
        PointerEventArgs args)
    {
        if (canvas is null || preview is null)
        {
            return;
        }

        double relativeX = args.GetPosition(canvas).X;
        double relativeY = args.GetPosition(canvas).Y;

        double newX = relativeX < 0 ? 0 : (relativeX > canvas.Bounds.Width ? canvas.Bounds.Width : relativeX);
        double newY = relativeY < 0 ? 0 : (relativeY > canvas.Bounds.Height ? canvas.Bounds.Height : relativeY);

        if (newX < 0 || newX > canvas.Bounds.Width || newY < 0 || newY > canvas.Bounds.Height)
        {
            preview.IsVisible = false;
            return;
        }

        Canvas.SetLeft(preview, newX);
        Canvas.SetTop(preview, newY);
    }

    private void OnPointerEntered(object? sender, 
        PointerEventArgs args)
    {
        if (preview is null)
        {
            return;
        }

        preview.IsVisible = true;
    }

    private void OnPointerExited(object? sender, 
        PointerEventArgs args)
    {
        if (preview is null)
        {
            return;
        }

        preview.IsVisible = false;
    }
}
