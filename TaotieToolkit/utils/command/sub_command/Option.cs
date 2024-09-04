using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgParseCS
{
    public class Option
    {
        public bool HasParameter { get; }

        public bool IsProvided { get; private set; } // if it is provided by the user as a console argument
        public string ParamValue { get; private set; } // parameter value 

        public Option(string shortOption, string longOption, string description, bool isRequired, bool hasParameter)
        {
            ShortOption = shortOption;
            LongOption = longOption;
            Description = description;
            IsRequired = isRequired;
            HasParameter = hasParameter;
            IsProvided = false;
            ParamValue = "";
        }

        public string ShortOption { get; }
        public string LongOption { get; }
        public string Description { get; }
        public bool IsRequired { get; }

        public override string ToString()
        {
            string argString = HasParameter ? " Value" : "";
            string rqdString = IsRequired ? "" : "[Optional] ";
            string str = "\t" + ShortOption + "\t" + LongOption + argString + "\t\t" + rqdString + Description;
            return str;
        }

        internal bool Parse(string[] args)
        {
            bool checkParameter = false;
            foreach(string item in args)
            {
                if (checkParameter)
                {
                    char[] toRemove = { '\'', '"' };
                    ParamValue = item.Trim().Trim(toRemove);
                    IsProvided = true;
                    return true;
                }
                if (item.Equals(ShortOption) || item.Equals(LongOption))
                {
                    if (HasParameter)
                        checkParameter = true;
                    else
                    {
                        IsProvided = true;
                        return true;
                    }
                }
            }
            if (!IsRequired)
                return true;
            return false;
        }
    }
}
