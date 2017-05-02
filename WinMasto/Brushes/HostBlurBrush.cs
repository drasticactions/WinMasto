using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI;

// From https://github.com/chungweileong94/NeonUI/blob/master/NeonUI/Brushes/HostBlurBrush.cs

namespace WinMasto.Brushes
{
    public class HostBlurBrush : XamlCompositionBrushBase
    {
        public Color BlurColor
        {
            get { return (Color)GetValue(BlurColorProperty); }
            set { SetValue(BlurColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlurColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlurColorProperty =
            DependencyProperty.Register("BlurColor", typeof(Color), typeof(HostBlurBrush), new PropertyMetadata(Color.FromArgb(255, 30, 30, 30), OnBlurColorPropertyChanged));

        private static void OnBlurColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((HostBlurBrush)d).OnBlurColorPropertyChanged((Color)e.NewValue);
        }

        private void OnBlurColorPropertyChanged(Color newValue)
        {
            setupBlurEffect();
        }

        public int BlurAmount
        {
            get { return (int)GetValue(BlurAmountProperty); }
            set { SetValue(BlurAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlurAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlurAmountProperty =
            DependencyProperty.Register("BlurAmount", typeof(int), typeof(HostBlurBrush), new PropertyMetadata(30));

        protected override void OnConnected()
        {
            setupBlurEffect();
        }

        private void setupBlurEffect()
        {
            Compositor compositor = Window.Current.Compositor;

            GaussianBlurEffect blurEffect = new GaussianBlurEffect()
            {
                BlurAmount = BlurAmount,
                BorderMode = EffectBorderMode.Hard,
                Optimization = EffectOptimization.Balanced,
                Source = new ArithmeticCompositeEffect()
                {
                    MultiplyAmount = 0,
                    Source1Amount = .4f,
                    Source2Amount = .6f,
                    Source1 = new CompositionEffectSourceParameter("backdrop"),
                    Source2 = new ColorSourceEffect()
                    {
                        Color = BlurColor
                    }
                }
            };

            var backdropBrush = compositor.CreateHostBackdropBrush();

            CompositionEffectFactory effectFacotory = compositor.CreateEffectFactory(blurEffect);
            var blurBrush = effectFacotory.CreateBrush();
            blurBrush.SetSourceParameter("backdrop", backdropBrush);

            CompositionBrush = blurBrush;
        }
    }
}
