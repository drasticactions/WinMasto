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

        // -------------------------------------------------------------------------------------------
        /// <summary>
        /// The regex to detect the URL from the text content
        /// It comes from https://gist.github.com/gruber/249502 (http://daringfireball.net/2010/07/improved_regex_for_matching_urls)
        /// </summary>
        private static readonly Regex UrlRegex = new Regex(@"(?i)\b((?:[a-z][\w-]+:(?:/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’]))", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));

        // -------------------------------------------------------------------------------------------
        /// <summary>
        /// The regex to detect the email addresses
        /// It comes from https://msdn.microsoft.com/en-us/library/01escwtf.aspx
        /// </summary>
        private static readonly Regex EmailRegex = new Regex(@"(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));

        // -------------------------------------------------------------------------------------------
        /// <summary>
        /// The regex to detect the phone numbers from the raw message
        /// </summary>
        private static readonly Regex PhoneRegex = new Regex(@"\+?[\d\-\(\)\. ]{5,}", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

        // -------------------------------------------------------------------------------------------
        /// <summary>
        /// The default prefix to use to convert a relative URI to an absolute URI
        /// The Windows RunTime is only working with absolute URI
        /// </summary>
        private const string RelativeUriDefaultPrefix = "http://";

        public static string GetFormattedText(DependencyObject obj)
        { return (string)obj.GetValue(FormattedTextProperty); }

        public static void SetFormattedText(DependencyObject obj, string value)
        { obj.SetValue(FormattedTextProperty, value); }

        private static void CreateRunElement(Windows.UI.Xaml.Controls.TextBlock textBlock, string rawText, int startPosition, int endPosition)
        {
            var fragment = rawText.Substring(startPosition, endPosition - startPosition);
            textBlock.Inlines.Add(new Run { Text = fragment });
        }

        private static int CreateUrlElement(Windows.UI.Xaml.Controls.TextBlock textBlock, Match urlMatch)
        {
            Uri targetUri;
            if (Uri.TryCreate(urlMatch.Value, UriKind.RelativeOrAbsolute, out targetUri))
            {
                var link = new Hyperlink();
                link.Inlines.Add(new Run { Text = urlMatch.Value });

                if (targetUri.IsAbsoluteUri)
                    link.NavigateUri = targetUri;
                else
                    link.NavigateUri = new Uri(RelativeUriDefaultPrefix + targetUri.OriginalString);


                textBlock.Inlines.Add(link);
            }
            else
            {
                textBlock.Inlines.Add(new Run { Text = urlMatch.Value });
            }

            return urlMatch.Index + urlMatch.Length;
        }

        private static int CreateContactElement(Windows.UI.Xaml.Controls.TextBlock textBlock, Match emailMatch, Match phoneMatch)
        {
            var currentMatch = emailMatch ?? phoneMatch;

            var link = new Hyperlink();
            link.Inlines.Add(new Run { Text = currentMatch.Value });
            link.Click += (s, a) =>
            {
                var contact = new Contact();
                if (emailMatch != null) contact.Emails.Add(new ContactEmail { Address = emailMatch.Value });
                if (phoneMatch != null) contact.Phones.Add(new ContactPhone { Number = new string(phoneMatch.Value.Where(char.IsDigit).ToArray()) });

                ContactManager.ShowFullContactCard(contact, new FullContactCardOptions());
            };

            textBlock.Inlines.Add(link);
            return currentMatch.Index + currentMatch.Length;
        }

        public static readonly DependencyProperty FormattedTextProperty =
            DependencyProperty.Register("FormattedText", typeof(string), typeof(TextBlockExtension),
                new PropertyMetadata(string.Empty, (sender, e) =>
                {
                    string rawText = e.NewValue as string;
                    var textBlock = sender as TextBlock;
                    if (textBlock != null) textBlock.Text = string.Empty;
                    if (string.IsNullOrEmpty(rawText)) return;


                    var lastPosition = 0;
                    var matches = new Match[3];
                    do
                    {
                        matches[0] = UrlRegex.Match(rawText, lastPosition);
                        matches[1] = EmailRegex.Match(rawText, lastPosition);
                        matches[2] = PhoneRegex.Match(rawText, lastPosition);

                        var firstMatch = matches.Where(x => x.Success).OrderBy(x => x.Index).FirstOrDefault();
                        if (firstMatch == matches[0])
                        {
                            // the first match is an URL
                            CreateRunElement(textBlock, rawText, lastPosition, firstMatch.Index);
                            lastPosition = CreateUrlElement(textBlock, firstMatch);
                        }
                        else if (firstMatch == matches[1])
                        {
                            // the first match is an email
                            CreateRunElement(textBlock, rawText, lastPosition, firstMatch.Index);
                            lastPosition = CreateContactElement(textBlock, firstMatch, null);
                        }
                        else if (firstMatch == matches[2])
                        {
                            textBlock.Inlines.Add(new Run { Text = rawText.Substring(lastPosition) });
                            lastPosition = rawText.Length;
                        }
                        else
                        {
                            // no match, we add the whole text
                            textBlock.Inlines.Add(new Run { Text = rawText.Substring(lastPosition) });
                            lastPosition = rawText.Length;
                        }
                    }
                    while (lastPosition < rawText.Length);
                }));
    }
}
