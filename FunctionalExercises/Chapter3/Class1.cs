using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Text.RegularExpressions;
using LaYumba.Functional;

namespace Chapter3
{
    public static class MyEnum
    {
        public static Option<T> ParseOptional<T>(string text) where T : struct
        {
            return System.Enum.TryParse(text, out T val) ? F.Some(val) : F.None;
        }
    }

    public static class MyListExtensions
    {
        public static Option<int> Lookup(this List<int> list, Predicate<int> predicate)
        {
            foreach (var i in list)
            {
                if (predicate(i)) return F.Some(i);
            }
            return F.None;
        }
    }

    public class Email
    {
        private const string Regex = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        
        private readonly string _email;
        private Email(string email)
        {
            _email = email;
        }

        public static Option<Email> Create(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, Regex) ? F.Some(new Email(email)) : F.None;
        }

        public static implicit operator string(Email e)
        {
            return e._email;
        }
    }

    public class AppConfigExtension
    {
        private NameValueCollection _settings;

        public AppConfigExtension(NameValueCollection settings)
        {
            _settings = settings ?? new NameValueCollection();
        }

        public Option<T> Get<T>(string key) 
            => _settings.Get(key) != null 
                ? F.Some((T)Convert.ChangeType(_settings.Get(key), typeof(T))) 
                : F.None;
    }
}
