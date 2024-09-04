using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ArgParseCS
{
    public class OptionSet: IEnumerable<Option>
    {
        readonly List<Option> _options = new List<Option>();

        public OptionSet(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public bool IsActive { get; private set; }

        public void Add(Option option)
        {
            _options.Add(option);
        }

        public IEnumerator<Option> GetEnumerator()
        {
            return _options.GetEnumerator();
        }

        public override string ToString()
        {
            string str = "\n" + Name + ":";
            return _options.Aggregate(str, (current, option) => current + ("\n" + option));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal OptionMatch Parse(string[] args)
        {
            bool isPartial = false;
            foreach(Option option in _options)
            {
                bool isAvailable = option.Parse(args);
                if (isAvailable)
                    isPartial = true;
                if (!isAvailable) // if a required parameter is not provided in the args
                    return isPartial ? OptionMatch.PARTIAL : OptionMatch.NONE;
            }
            IsActive = true;
            return OptionMatch.FULL;
        }

        public Option GetOption(string shortOption)
        {
            foreach (Option option in _options)
                //if (option.IsProvided && option.ShortOption.Equals(shortOption))
                if (option.ShortOption.Equals(shortOption))
                    return option;
            return null;
        }
    }

    internal enum OptionMatch
    {
        FULL,
        PARTIAL,
        NONE
    }
}