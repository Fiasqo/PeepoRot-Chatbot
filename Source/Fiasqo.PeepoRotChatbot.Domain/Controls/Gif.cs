using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Fiasqo.PeepoRotChatbot.Domain.Controls {
public sealed class Gif : Image {
#region Fields

	private Int32Animation _animation;
	private GifBitmapDecoder _decoder;
	private bool _isAnimationWorking;

#endregion

#region Properties

	public static readonly DependencyProperty SpeedRatioProperty =
		DependencyProperty.Register("SpeedRatio",
									typeof(double),
									typeof(Gif),
									new UIPropertyMetadata(1.0, OnAnimatedSpeedRatioChanged));

	public static readonly DependencyProperty FrameIndexProperty =
		DependencyProperty.Register("FrameIndex",
									typeof(int),
									typeof(Gif),
									new UIPropertyMetadata(0, OnFrameIndexChanged));

	public new static readonly DependencyProperty SourceProperty =
		DependencyProperty.Register("Source",
									typeof(ImageSource),
									typeof(Gif),
									new FrameworkPropertyMetadata(null,
																  FrameworkPropertyMetadataOptions.AffectsRender |
																  FrameworkPropertyMetadataOptions.AffectsMeasure,
																  OnSourceChanged));

	public int FrameIndex { get => (int) GetValue(FrameIndexProperty); set => SetValue(FrameIndexProperty, value); }
	public new ImageSource Source { get => (ImageSource) GetValue(SourceProperty); set => SetValue(SourceProperty, value); }
	public double SpeedRatio { get => (double) GetValue(SpeedRatioProperty); set => SetValue(SpeedRatioProperty, value); }

#endregion

#region Private Methods

	private void ClearAnimation() {
		if (_animation != null) BeginAnimation(FrameIndexProperty, null);
		_isAnimationWorking = false;
		_animation = null;
		_decoder = null;
	}

	private void PrepareAnimation(BitmapImage bitmapImage) {
		if (bitmapImage.UriSource != null) {
			_decoder = new GifBitmapDecoder(bitmapImage.UriSource,
											BitmapCreateOptions.PreservePixelFormat,
											BitmapCacheOption.Default);
		} else {
			bitmapImage.StreamSource.Position = 0;
			_decoder = new GifBitmapDecoder(bitmapImage.StreamSource,
											BitmapCreateOptions.PreservePixelFormat,
											BitmapCacheOption.Default);
		}

		//frames / 10fps * 1000ms = frames * 100 is animation duration in milliseconds (1cycle)
		var AnimationTimeSpan =
			new TimeSpan(0, 0, 0, 0, (int) (_decoder.Frames.Count * 100.0 * (1.00 / SpeedRatio)));
		_animation =
			new Int32Animation(0,
							   _decoder.Frames.Count - 1,
							   new Duration(AnimationTimeSpan)) {
				RepeatBehavior = RepeatBehavior.Forever,
			};

		base.Source = _decoder.Frames[0];
		BeginAnimation(FrameIndexProperty, _animation);
		_isAnimationWorking = true;
	}

	private bool IsAnimatedGifImage(BitmapImage bitmapImage) {
		var result = false;

		if (bitmapImage.UriSource != null) {
			var lBitmapDecoder = BitmapDecoder.Create(bitmapImage.UriSource,
													  BitmapCreateOptions.PreservePixelFormat,
													  BitmapCacheOption.Default);
			result = lBitmapDecoder is GifBitmapDecoder;
		} else if (bitmapImage.StreamSource != null) {
			try {
				var lStreamPosition = bitmapImage.StreamSource.Position;
				bitmapImage.StreamSource.Position = 0;
				var lBitmapDecoder =
					new GifBitmapDecoder(bitmapImage.StreamSource,
										 BitmapCreateOptions.PreservePixelFormat,
										 BitmapCacheOption.Default);
				result = lBitmapDecoder.Frames.Count > 1;

				bitmapImage.StreamSource.Position = lStreamPosition;
			} catch {
				result = false;
			}
		}

		return result;
	}

#endregion

#region OnPropertyChanged

	private static void OnFrameIndexChanged(DependencyObject dependencyObject,
											DependencyPropertyChangedEventArgs eventArgs) {
		if (eventArgs.NewValue == eventArgs.OldValue) return;

		var gif = (Gif) dependencyObject;

		if (!gif._isAnimationWorking) return;
		((Image) gif).Source = gif._decoder.Frames[(int) eventArgs.NewValue];
		gif.InvalidateVisual();
	}

	private static void OnSourceChanged(DependencyObject dependencyObject,
										DependencyPropertyChangedEventArgs eventArgs) {
		if (eventArgs.NewValue == eventArgs.OldValue) return;

		var gif = (Gif) dependencyObject;
		var bitmapImage = eventArgs.NewValue as BitmapImage;
		if (bitmapImage == null && eventArgs.NewValue is BitmapFrame bitmapFrame)
			if (bitmapFrame.Decoder != null)
				bitmapImage = new BitmapImage(new Uri(bitmapFrame.Decoder.ToString()));
		gif.ClearAnimation();

		if (bitmapImage == null) {
			var lImageSource = eventArgs.NewValue as ImageSource;
			((Image) gif).Source = lImageSource;
			return;
		}

		if (!gif.IsAnimatedGifImage(bitmapImage)) {
			bitmapImage = new BitmapImage(bitmapImage.UriSource);
			((Image) gif).Source = bitmapImage;
			return;
		}

		gif.PrepareAnimation(bitmapImage);
	}

	private static void OnAnimatedSpeedRatioChanged(DependencyObject dependencyObject,
													DependencyPropertyChangedEventArgs eventArgs) {
		if (eventArgs.NewValue == eventArgs.OldValue) return;

		var gif = (Gif) dependencyObject;
		if (gif._isAnimationWorking) gif.PrepareAnimation((BitmapImage) gif.Source);
	}

#endregion
}
}