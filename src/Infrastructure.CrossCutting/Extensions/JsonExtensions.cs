﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Modal.Digital.Card.Request.Infrastructure.CrossCutting
{
    public static class JsonExtensions
    {
        public static string MaskFields(this string json, List<string> blacklist, char mask = '*', bool showLength = true)
        {
            if (string.IsNullOrWhiteSpace(json) || !(blacklist?.Any()).GetValueOrDefault())
            {
                return json;
            }

            var jsonObject = (JObject)JsonConvert.DeserializeObject(json);
            MaskFieldsFromJToken(jsonObject, blacklist, mask, showLength);

            return jsonObject.ToString();
        }

        private static void MaskFieldsFromJToken(JToken token, List<string> blacklist, char mask, bool showLength)
        {
            if (!(token is JContainer container))
            {
                // abort recursive
                return; 
            }

            var children = container.Children();

            foreach (var item in children)
            {
                if (item is JProperty prop && IsMatching(blacklist, prop))
                {
                    prop.Value = GetValueMasked(mask, prop, showLength);
                }

                // call recursive 
                MaskFieldsFromJToken(item, blacklist, mask, showLength);
            }
        }

        private static bool IsMatching(List<string> blacklist, JProperty prop)
        {
            return blacklist.Any(item =>
            {
                return Regex.IsMatch(prop.Path, WildCardToRegular(item), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            });
        }

        private static string GetValueMasked(char mask, JProperty prop, bool showLength)
        {
            var length = (prop.Value?.ToString().Length).GetValueOrDefault();
            var value = new string(mask, length);

            if (showLength)
            {
                value = $"{value} [{length} chars]";
            }

            return value;
        }

        private static string WildCardToRegular(string value)
        {
            return "^" + Regex.Escape(value).Replace("\\*", ".*") + "$";
        }
    }
}
