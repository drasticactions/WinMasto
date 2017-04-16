using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace WinMasto.Tools.Extensions
{
    public static class HtmlRemoval
    {
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }

    public static class TextBlockExtension
    {
        public static string GetFormattedText(DependencyObject obj)
        { return (string)obj.GetValue(FormattedTextProperty); }

        public static void SetFormattedText(DependencyObject obj, string value)
        { obj.SetValue(FormattedTextProperty, value); }

        public static readonly DependencyProperty FormattedTextProperty =
            DependencyProperty.Register("FormattedText", typeof(string), typeof(TextBlockExtension),
                new PropertyMetadata(string.Empty, (sender, e) =>
                {
                    string text = e.NewValue as string;
                    var textBl = sender as TextBlock;
                    if (textBl != null && !string.IsNullOrWhiteSpace(text))
                    {
                        textBl.Inlines.Clear();
                        Regex regx = new Regex(@"(http(s)?://[\S]+|www.[\S]+|[\S]+@[\S]+)", RegexOptions.IgnoreCase);
                        Regex isWWW = new Regex(@"(http[s]?://[\S]+|www.[\S]+)");
                        Regex isEmail = new Regex(@"[\S]+@[\S]+");
                        foreach (var item in regx.Split(text))
                        {
                            if (isWWW.IsMatch(item))
                            {
                                Hyperlink link = new Hyperlink { NavigateUri = new Uri(item.ToLower().StartsWith("http") ? item : $"http://{item}"), Foreground = Application.Current.Resources["SystemControlForegroundAccentBrush"] as SolidColorBrush };
                                link.Inlines.Add(new Run { Text = item });
                                textBl.Inlines.Add(link);
                            }
                            else if (isEmail.IsMatch(item))
                            {
                                Hyperlink link = new Hyperlink { NavigateUri = new Uri($"mailto:{item}"), Foreground = Application.Current.Resources["SystemControlForegroundAccentBrush"] as SolidColorBrush };
                                link.Inlines.Add(new Run { Text = item });
                                textBl.Inlines.Add(link);
                            }
                            else textBl.Inlines.Add(new Run { Text = item });
                        }
                    }
                }));
    }
}
