using System;
using System.Collections;
using System.Collections.Generic;

namespace ArgParseCS
{
    public class ArgParse : IEnumerable<OptionSet>
    {
        readonly List<OptionSet> _optionSets = new List<OptionSet>();
        public void Add(OptionSet optionSet)
        {
            _optionSets.Add(optionSet);
        }

        public string Usage()
        {
            string usage = "Usage:";
            foreach (OptionSet optionSet in _optionSets)
                usage += optionSet;

            usage += "\n";
            Console.Out.Write(usage);
            return usage;
        }

        public void Parse(string[] args)
        {
            int matchedOptionsFull = 0;
            int matchedOptionsPartial = 0;
            foreach (OptionSet optionSet in _optionSets)
            {
                if (optionSet.Parse(args).Equals(OptionMatch.PARTIAL))
                    matchedOptionsPartial += 1;
                if (optionSet.Parse(args).Equals(OptionMatch.FULL))
                    matchedOptionsFull += 1;
            }
            if ((matchedOptionsPartial == 0) && (matchedOptionsFull == 1))
                    return;
            // If all the optionsets are checked and none of them are a match,
            // then raise an exception
            throw new ArgumentException("Invalid arguments; check the parameter list and try again.");
        }

        public OptionSet GetActiveOptionSet()
        {
            foreach (OptionSet optionSet in _optionSets)
                if (optionSet.IsActive)
                    return optionSet;
            return null;
        }

        public IEnumerator<OptionSet> GetEnumerator()
        {
            return _optionSets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
