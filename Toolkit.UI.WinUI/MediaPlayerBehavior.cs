using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using Windows.Media.Playback;

namespace Toolkit.UI.WinUI;

public class MediaPlayerBehavior : 
    Behavior<MediaPlayerElement>
{
    public static readonly DependencyProperty MediaPlayerProperty =
        DependencyProperty.Register(nameof(MediaPlayer),
            typeof(MediaPlayer), typeof(MediaPlayerBehavior),
            new PropertyMetadata(null, OnMediaPlayerPropertyChanged));

    public MediaPlayer MediaPlayer
    {
        get => (MediaPlayer)GetValue(MediaPlayerProperty);
        set => SetValue(MediaPlayerProperty, value);
    }

    private static void OnMediaPlayerPropertyChanged(DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs args)
    {
        if (dependencyObject is MediaPlayerBehavior behavior)
        {
            behavior.OnMediaPlayerPropertyChanged();
        }
    }

    private void OnMediaPlayerPropertyChanged() => 
        SetMediaPlayer();

    private void SetMediaPlayer()
    {
        if (MediaPlayer is not null && AssociatedObject is not null)
        {
            AssociatedObject.SetMediaPlayer(MediaPlayer);
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        SetMediaPlayer();
    }
}
