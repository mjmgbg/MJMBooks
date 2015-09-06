using System;
using System.Net;
using System.Text.RegularExpressions;
using Business;

namespace Data
{
    public static class ParseHtml
    {
        private static string _html;

        public static bool LoadDocument(string isbn)
        {
            var client = new WebClient();
            _html = client.DownloadString("http://www.adlibris.com/se/organisationer/product.aspx?isbn=" + isbn);
            return _html != string.Empty;
        }

        public static string GetTitle()
        {
            var title = string.Empty;
            var mTitle = Regex.Match(_html, "<span itemprop=\"name\">(.*?)</span>");
            if (mTitle.Success)
            {
                title = mTitle.Groups[1].Value;
            }

            return title;
        }

        public static string GetAuthors()
        {
            var tempCat = string.Empty;
            var start = _html.IndexOf("class=\"productAuthor\"", StringComparison.Ordinal);
            if (start <= 0) return string.Empty;
            var end = _html.IndexOf("class=\"solid\"", StringComparison.Ordinal);
            var tag = _html.Substring(start, end);
            int startcat;
            do
            {
                start = tag.IndexOf("<a", StringComparison.Ordinal);
                tag = tag.Substring(start);
                start = tag.IndexOf(">", StringComparison.Ordinal);
                end = tag.IndexOf("</a>", StringComparison.Ordinal);
                tempCat += tag.Substring(start + 1, end - start - 1) + ";";
                tag = tag.Substring(end);
                startcat = tag.IndexOf("<h2>", StringComparison.Ordinal);
            } while (startcat > 0);

            tempCat = Helpers.HandleSpecialChars(tempCat);
            return tempCat.Substring(0, tempCat.Length - 1);
        }

        public static string GetDate()
        {
            var year = string.Empty;
            var month = string.Empty;
            var day = string.Empty;

            var mYear = Regex.Match(_html, "<span id=\"ctl00_main_frame_ctrlproduct_lblPublished\">(.*?)</span>");
            if (!mYear.Success) return year + "-" + month + "-" + day;
            year = mYear.Groups[1].Value.Substring(0, 4);
            month = mYear.Groups[1].Value.Substring(4, 2);
            day = mYear.Groups[1].Value.Length > 7 ? mYear.Groups[1].Value.Substring(6, 2) : "01";

            return year + "-" + month + "-" + day;
        }

        public static string GetDescription()
        {
            var start = _html.IndexOf("<span itemprop=\"description\">", StringComparison.Ordinal);
            var end = _html.IndexOf("class=\"productReview", StringComparison.Ordinal);
            if (start <= 0) return string.Empty;
            var description = _html.Substring(start, (end) - start);
            start = description.IndexOf(">", StringComparison.Ordinal);
            end = description.IndexOf("</span>", StringComparison.Ordinal);
            description = description.Substring(start + 1, (end) - start - 1);
            description = Helpers.StripTagsCharArray(description);
            description = Helpers.HandleSpecialChars(description);
            description = Helpers.AddSpacesToSentence(description, true);
            description = description.Trim();
            return description;
        }

        public static string GetSeries()
        {
            var start = _html.IndexOf("ctl00_main_frame_ctrlproduct_liSeries", StringComparison.Ordinal);
            var end = _html.IndexOf("ctl00_main_frame_ctrlproduct_ulProductInfo2", StringComparison.Ordinal);
            if (start <= 0) return string.Empty;
            var tag = _html.Substring(start, end - start);
            start = tag.IndexOf("<a", StringComparison.Ordinal);
            end = tag.IndexOf("</a>", StringComparison.Ordinal);
            tag = tag.Substring(start, end - start);
            start = tag.IndexOf(">", StringComparison.Ordinal);
            var series = tag.Substring(start + 1);
            series = Helpers.HandleSpecialChars(series);
            return series;
        }

        public static string GetPublisher()
        {
            var publisher = string.Empty;
            var mPublisher = Regex.Match(_html, "<span itemprop=\"publisher\">(.*?)</span>");
            if (mPublisher.Success)
            {
                publisher = mPublisher.Groups[1].Value;
            }

            publisher = Helpers.HandleSpecialChars(publisher);
            return publisher;
        }

        public static string GetGenres()
        {
            var tempCat = string.Empty;
            var start = _html.IndexOf("ctl00_main_frame_ctrlproduct_ctl06_expandedNodes", StringComparison.Ordinal);
            if (start <= 0) return string.Empty;
            start = _html.IndexOf("class=\"productSubHeader", StringComparison.Ordinal);
            var tag = _html.Substring(start + 24);
            start = tag.IndexOf("class=\"productSubHeader", StringComparison.Ordinal);
            tag = tag.Substring(start + 24);
            var end = tag.IndexOf("ctl00_main_frame_ctrlproduct_divSearchMore", StringComparison.Ordinal);
            tag = tag.Substring(0, end);
            var startcat = tag.IndexOf("type=cat&amp;typeid=", StringComparison.Ordinal);
            do
            {
                tag = tag.Substring(startcat + 20);
                start = tag.IndexOf(">", StringComparison.Ordinal);
                end = tag.IndexOf("</a>", StringComparison.Ordinal);
                tempCat += tag.Substring(start + 1, end - start - 1) + ";";
                tag = tag.Substring(end);
                startcat = tag.IndexOf("type=cat&amp;typeid=", StringComparison.Ordinal);
            } while (startcat > 0);

            tempCat = Helpers.HandleSpecialChars(tempCat);
            return tempCat.Substring(0, tempCat.Length - 1);
        }

        public static string GetReader()
        {
            var tempCat = string.Empty;
            var start = _html.IndexOf("<strong>Uppläsare: </strong>", StringComparison.Ordinal);
            if (start <= 0) return string.Empty;
            start = _html.IndexOf("ctl00_main_frame_ctrlproduct_ulProductInfo", StringComparison.Ordinal);
            var end = _html.IndexOf("ctl00_main_frame_ctrlproduct_liPublisher", StringComparison.Ordinal);
            var tag = _html.Substring(start, end - start);
            start = tag.IndexOf("<strong>Uppläsare: </strong>", StringComparison.Ordinal);
            tag = tag.Substring(start);
            var startcat = tag.IndexOf("<strong>Uppläsare: </strong>", StringComparison.Ordinal);
            do
            {
                tag = tag.Substring(startcat + 70);
                start = tag.IndexOf(">", StringComparison.Ordinal);
                end = tag.IndexOf("</a>", StringComparison.Ordinal);
                tempCat += tag.Substring(start + 1, end - start - 1) + ";";
                tag = tag.Substring(end - 70);
                startcat = tag.IndexOf("<strong>Uppläsare: </strong>", StringComparison.Ordinal);
            } while (startcat > 0);

            tempCat = Helpers.HandleSpecialChars(tempCat);
            return tempCat.Substring(0, tempCat.Length - 1);
        }

        public static string GetLanguage()
        {
            var start = _html.IndexOf("<span id=\"ctl00_main_frame_ctrlproduct_lblLanguage\">", StringComparison.Ordinal);
            if (start <= 0) return string.Empty;
            var tag = _html.Substring(start);
            var end = tag.IndexOf("</span>", StringComparison.Ordinal);
            tag = tag.Substring(0, end);
            start = tag.IndexOf(">", StringComparison.Ordinal);
            var language = tag.Substring(start + 1);
            language = Helpers.HandleSpecialChars(language);
            return language;
        }
    }
}