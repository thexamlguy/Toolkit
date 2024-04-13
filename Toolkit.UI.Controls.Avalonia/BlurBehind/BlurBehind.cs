using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Styling;
using SkiaSharp;

namespace Toolkit.UI.Controls.Avalonia;

public class BlurBehind : 
    Control
{
    public static readonly StyledProperty<ExperimentalAcrylicMaterial> MaterialProperty =
        AvaloniaProperty.Register<BlurBehind, ExperimentalAcrylicMaterial>("Material");

    public static readonly ImmutableExperimentalAcrylicMaterial DefaultAcrylicMaterialDark = 
        (ImmutableExperimentalAcrylicMaterial)new ExperimentalAcrylicMaterial()
    {
        MaterialOpacity = 0.25,
        TintColor = Colors.Black,
        TintOpacity = 0.7,
        PlatformTransparencyCompensationLevel = 0
    }.ToImmutable();

    public static readonly ImmutableExperimentalAcrylicMaterial DefaultAcrylicMaterialLight = 
        (ImmutableExperimentalAcrylicMaterial)new ExperimentalAcrylicMaterial()
    {
        MaterialOpacity = 0.0,
        TintColor = Colors.White,
        TintOpacity = 0.3,
        PlatformTransparencyCompensationLevel = 0
    }.ToImmutable();

    static BlurBehind()
    {
        AffectsRender<BlurBehind>(MaterialProperty);
    }

    public ExperimentalAcrylicMaterial Material
    {
        get => GetValue(MaterialProperty);
        set => SetValue(MaterialProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        ImmutableExperimentalAcrylicMaterial material = Material is not null
            ? (ImmutableExperimentalAcrylicMaterial)Material.ToImmutable()
            : Application.Current?.ActualThemeVariant == ThemeVariant.Dark ? DefaultAcrylicMaterialDark : DefaultAcrylicMaterialLight;
       
        context.Custom(new BlurBehindRenderOperation(material, new Rect(default, Bounds.Size)));
    }

    private class BlurBehindRenderOperation(ImmutableExperimentalAcrylicMaterial material,
        Rect bounds) : ICustomDrawOperation
    {
        private readonly Rect bounds = bounds;
        private readonly ImmutableExperimentalAcrylicMaterial material = material;

        public Rect Bounds => bounds.Inflate(4);

        public void Dispose()
        {

        }

        public bool Equals(ICustomDrawOperation? other) => 
            other is BlurBehindRenderOperation behindRenderOperation && 
            behindRenderOperation.bounds == bounds && behindRenderOperation.material.Equals(material);

        public bool HitTest(Point point) => bounds.Contains(point);

        public void Render(ImmediateDrawingContext context)
        {
            if (context.TryGetFeature<ISkiaSharpApiLeaseFeature>() is ISkiaSharpApiLeaseFeature  leaseFeature)
            {
                using ISkiaSharpApiLease? lease = leaseFeature.Lease();
                if (lease.SkCanvas is SKCanvas canvas)
                {
                    if (canvas.TotalMatrix.TryInvert(out SKMatrix currentInvertedTransform))
                    {
                        if (lease.SkSurface is SKSurface surface)
                        {
                            using SKImage backgroundSnapshot = surface.Snapshot();
                            using SKShader backdropShader = SKShader.CreateImage(backgroundSnapshot, SKShaderTileMode.Clamp,
                                SKShaderTileMode.Clamp, currentInvertedTransform);

                            using SKSurface blurred = SKSurface.Create(lease.GrContext, false,
                                new SKImageInfo((int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height),
                                    SKImageInfo.PlatformColorType, SKAlphaType.Premul));

                            using (SKImageFilter filter = SKImageFilter.CreateBlur(8, 8, SKShaderTileMode.Clamp))
                            using (SKPaint blurPaint = new() { Shader = backdropShader, ImageFilter = filter }) 
                                blurred.Canvas.DrawRect(5, 5, (float)bounds.Width - 20, (float)bounds.Height - 20, blurPaint);

                            using SKImage blurSnap = blurred.Snapshot();
                            using SKShader blurSnapShader = SKShader.CreateImage(blurSnap);
                            using SKPaint blurSnapPaint = new()
                            {
                                Shader = blurSnapShader,
                                IsAntialias = true
                            };

                            canvas.DrawRect(0, 0, (float)bounds.Width, (float)bounds.Height, blurSnapPaint);
                        }

                    }
                }
            }
        }
    }
}

