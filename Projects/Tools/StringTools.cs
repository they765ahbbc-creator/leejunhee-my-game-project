using UnityEngine;

namespace ShootemUp
{
    public static class StringTools
    {
        public static string CombineNameAndNumber(string name, int number)
        {
            if(number < 10)
            {
                return (name + "0" + number);
            }
            else if (number < 100)
            {
                return (name + number);
            }
            else
            {
                Debug.Log("StringTools.cs: You have combined a name and a numder (typically as LayerMask)" +
                    "bigger than 99, Only a maximum of 99 is allowed.");

                return (name + "99");
            }
        
        }
        public static string GetLayerName(string name)
        {
            return name.Remove(ProjectConstants.LayerPrefixLength);
        }

        public static void RemoveParenthesesAfterLastSpace(GameObject go)
        {
            string childName = go.name;
            int childNameLength = childName.Length;
            char lastChar = childName[childName.Length - 1];

            if (lastChar != ')')
            {
                return;
            }
            else
            {
                for (int j = childNameLength - 1; j >= 0; j--)
                {
                    if(childName[j] == ' ')
                    {
                        go.name = childName.Remove(j);
                        j = -1;
                    }
                }
            }
        }

        public static string FindClassName(System.Type type)
        {
            string className = type.ToString();
            return className;
        }
    }
}
