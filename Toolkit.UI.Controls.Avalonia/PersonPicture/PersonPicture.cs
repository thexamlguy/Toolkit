using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Toolkit.UI.Controls.Avalonia;


public class PersonPicture : TemplatedControl
{
    public static readonly StyledProperty<string> BadgeGlyphProperty =
        AvaloniaProperty.Register<PersonPicture, string>(nameof(BadgeGlyph));

    public static readonly StyledProperty<IImage> BadgeImageSourceProperty =
        AvaloniaProperty.Register<PersonPicture, IImage>(nameof(BadgeImageSource));

    public static readonly StyledProperty<int> BadgeNumberProperty =
        AvaloniaProperty.Register<PersonPicture, int>(nameof(BadgeNumber));

    public static readonly StyledProperty<string> BadgeTextProperty =
        AvaloniaProperty.Register<PersonPicture, string>(nameof(BadgeText));

    public static readonly StyledProperty<string> DisplayNameProperty =
        AvaloniaProperty.Register<PersonPicture, string>(nameof(DisplayName));

    public static readonly StyledProperty<string> InitialsProperty =
        AvaloniaProperty.Register<PersonPicture, string>(nameof(Initials));

    public static readonly StyledProperty<bool> IsGroupProperty =
        AvaloniaProperty.Register<PersonPicture, bool>(nameof(IsGroup));

    public static readonly StyledProperty<IImage> ProfilePictureProperty =
        AvaloniaProperty.Register<PersonPicture, IImage>(nameof(ProfilePicture));

    private static readonly StyledProperty<PersonPictureTemplateSettings> TemplateSettingsProperty =
        AvaloniaProperty.Register<PersonPicture, PersonPictureTemplateSettings>(nameof(TemplateSettings));

    private readonly ImageBrush? badgeImageBrush;
    private FontIcon? badgeGlyphIcon;
    private TextBlock? badgeNumberTextBlock;
    private Ellipse? badgingBackgroundEllipse;
    private Ellipse? badgingEllipse;
    private PersonPictureColourGenerator colourGenerator = new(hue: 210, saturation: 0.8f, lightness: 0.6f);
    private string? displayNameInitials;
    private TextBlock? initialsTextBlock;

    public PersonPicture()
    {
        SetValue(TemplateSettingsProperty, new PersonPictureTemplateSettings());
        SizeChanged += OnSizeChanged;
    }

    public string BadgeGlyph
    {
        get => GetValue(BadgeGlyphProperty);
        set => SetValue(BadgeGlyphProperty, value);
    }

    public IImage BadgeImageSource
    {
        get => GetValue(BadgeImageSourceProperty);
        set => SetValue(BadgeImageSourceProperty, value);
    }

    public int BadgeNumber
    {
        get => GetValue(BadgeNumberProperty);
        set => SetValue(BadgeNumberProperty, value);
    }

    public string BadgeText
    {
        get => GetValue(BadgeTextProperty);
        set => SetValue(BadgeTextProperty, value);
    }

    public string DisplayName
    {
        get => GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }

    public string Initials
    {
        get => GetValue(InitialsProperty);
        set => SetValue(InitialsProperty, value);
    }

    public bool IsGroup
    {
        get => GetValue(IsGroupProperty);
        set => SetValue(IsGroupProperty, value);
    }

    public IImage ProfilePicture
    {
        get => GetValue(ProfilePictureProperty);
        set => SetValue(ProfilePictureProperty, value);
    }

    public PersonPictureTemplateSettings TemplateSettings
    {
        get => GetValue(TemplateSettingsProperty);
        set => SetValue(TemplateSettingsProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs args)
    {
        base.OnApplyTemplate(args);

        initialsTextBlock = args.NameScope.Get<TextBlock>("InitialsTextBlock");

        badgeNumberTextBlock = args.NameScope.Get<TextBlock>("BadgeNumberTextBlock");
        badgeGlyphIcon = args.NameScope.Get<FontIcon>("BadgeGlyphIcon");
        badgingEllipse = args.NameScope.Get<Ellipse>("BadgingEllipse");
        badgingBackgroundEllipse = args.NameScope.Get<Ellipse>("BadgingBackgroundEllipse");

        UpdateBadge();
        UpdateIfReady();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == BadgeGlyphProperty)
        {
            UpdateBadge();
        }

        if (change.Property == BadgeImageSourceProperty)
        {
            UpdateBadge();
        }

        if (change.Property == BadgeNumberProperty)
        {
            UpdateBadge();
        }

        if (change.Property == DisplayNameProperty)
        {
            UpdateDisplayName();
        }

        if (change.Property == InitialsProperty)
        {
            UpdateIfReady();
        }

        if (change.Property == IsGroupProperty)
        {
            UpdateIfReady();
        }

        if (change.Property == ProfilePictureProperty)
        {
        }
    }

    private IImage? GetImageSource()
    {
        if (ProfilePicture != null)
        {
            return ProfilePicture;
        }

        return null;
    }

    private string GetInitials()
    {
        if (!string.IsNullOrEmpty(Initials))
        {
            return Initials;
        }
        else if (!string.IsNullOrEmpty(displayNameInitials))
        {
            return displayNameInitials;
        }

        return "";
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs args)
    {
        {
            bool widthChanged = args.NewSize.Width != args.PreviousSize.Width;
            bool heightChanged = args.NewSize.Height != args.PreviousSize.Height;
            double newSize;

            if (widthChanged && heightChanged)
            {
                newSize = args.NewSize.Width < args.NewSize.Height ? args.NewSize.Width : args.NewSize.Height;
            }
            else if (widthChanged)
            {
                newSize = args.NewSize.Width;
            }
            else if (heightChanged)
            {
                newSize = args.NewSize.Height;
            }
            else
            {
                return;
            }

            Height = newSize;
            Width = newSize;
        }

        double fontSize = Math.Max(1.0, Width * .42);

        if (initialsTextBlock is not null)
        {
            initialsTextBlock.FontSize = fontSize;
        }

        if (badgingEllipse is not null && badgingBackgroundEllipse is not null && badgeNumberTextBlock is not null && badgeGlyphIcon is not null)
        {
            double newSize = args.NewSize.Width < args.NewSize.Height ? args.NewSize.Width : args.NewSize.Height;
            badgingEllipse.Height = newSize * 0.5;
            badgingEllipse.Width = newSize * 0.5;
            badgingBackgroundEllipse.Height = newSize * 0.5;
            badgingBackgroundEllipse.Width = newSize * 0.5;
            badgeNumberTextBlock.FontSize = Math.Max(1.0, badgingEllipse.Height * 0.6);
            badgeGlyphIcon.FontSize = Math.Max(1.0, badgingEllipse.Height * 0.6);
        }
    }

    private void UpdateBadge()
    {
        if (BadgeImageSource != null)
        {
            UpdateBadgeImageSource();
        }
        else if (BadgeNumber != 0)
        {
            UpdateBadgeNumber();
        }
        else if (!string.IsNullOrEmpty(BadgeGlyph))
        {
            UpdateBadgeGlyph();
        }
        else
        {
            PseudoClasses.Set(":NoBadge", true);
            if (badgeNumberTextBlock != null)
            {
                badgeNumberTextBlock.Text = "";
            }

            if (badgeGlyphIcon != null)
            {
                badgeGlyphIcon.Glyph = "";
            }
        }
    }

    private void UpdateBadgeGlyph()
    {
        if (badgingEllipse == null || badgeGlyphIcon == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(BadgeGlyph))
        {
            PseudoClasses.Set(":NoBadge", true);
            badgeGlyphIcon.Glyph = "";
            return;
        }

        PseudoClasses.Set(":BadgeWithoutImageSource", true);
        badgeGlyphIcon.Glyph = BadgeGlyph;
    }

    private void UpdateBadgeImageSource()
    {
        if (badgingEllipse == null || badgeImageBrush == null)
        {
            return;
        }

        badgeImageBrush.Source = (Bitmap?)BadgeImageSource;

        if (BadgeImageSource != null)
        {
            PseudoClasses.Set(":BadgeWithImageSource", true);
        }
        else
        {
            PseudoClasses.Set(":NoBadge", true);
        }
    }

    private void UpdateBadgeNumber()
    {
        if (badgingEllipse == null || badgeNumberTextBlock == null)
        {
            return;
        }

        if (BadgeNumber <= 0)
        {
            PseudoClasses.Set(":NoBadge", true);
            badgeNumberTextBlock.Text = "";

            return;
        }

        PseudoClasses.Set(":BadgeWithoutImageSource", true);
        if (BadgeNumber <= 99)
        {
            badgeNumberTextBlock.Text = BadgeNumber.ToString();
        }
        else
        {
            badgeNumberTextBlock.Text = "99+";
        }
    }

    private void UpdateDisplayName()
    {
        displayNameInitials = PersonPictureInitialsGenerator.InitialsFromDisplayName(DisplayName);
        UpdateIfReady();
    }

    private void UpdateIfReady()
    {
        string initials = GetInitials();
        IImage? imageSource = GetImageSource();

        PersonPictureTemplateSettings templateSettings = TemplateSettings;
        templateSettings.ActualInitials = initials;

        if (DisplayName is { Length: > 0 })
        {
            Color rgb = colourGenerator.Create(DisplayName);
            SetValue(BackgroundProperty, new SolidColorBrush(Color.FromArgb(rgb.A, rgb.R, rgb.G, rgb.B)));
        }

        if (imageSource is not null)
        {
            ImageBrush? imageBrush = templateSettings.ActualImageBrush;
            if (imageBrush == null)
            {
                imageBrush = new ImageBrush
                {
                    Stretch = Stretch.UniformToFill
                };

                templateSettings.ActualImageBrush = imageBrush;
            }

            imageBrush.Source = (Bitmap?)imageSource;
        }
        else
        {
            templateSettings.ActualImageBrush = null;
        }

        if (IsGroup)
        {
            PseudoClasses.Set(":Group", true);
        }
        else
        {
            if (imageSource != null)
            {
                PseudoClasses.Set(":Photo", true);
            }
            else if (!string.IsNullOrEmpty(initials))
            {
                PseudoClasses.Set(":Initials", true);
            }
            else
            {
                PseudoClasses.Set(":NoPhotoOrInitials", true);
            }
        }
    }
}