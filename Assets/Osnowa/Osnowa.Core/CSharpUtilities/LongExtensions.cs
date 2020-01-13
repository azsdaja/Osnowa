namespace Osnowa.Osnowa.Core.CSharpUtilities
{
    using System;

    public static class LongExtensions
    {
        /// <summary>
        /// Warning - causes boxing of enum.
        /// </summary>
        public static bool HasFlag(this ulong checker, Enum @checked)
        {
            ulong checkedULong = Convert.ToUInt64(@checked);

            return (checker & checkedULong) == checkedULong;
        }
        
        public static bool HasFlag(this ulong checker, ulong checkedFlag)
        {
            return (checker & checkedFlag) == checkedFlag;
        }
        
        public static ulong WithFlagSet(this ulong checker, Enum flagToSet, bool flagValue)
        {
            ulong flagToSetULong = Convert.ToUInt64(flagToSet);

            ulong checkerWithoutGivenFlag = checker & ~flagToSetULong;
            ulong flagSetter = flagValue ? flagToSetULong : 0; 
            
            return checkerWithoutGivenFlag | flagSetter;
        }
        
        public static ulong WithFlagSet(this ulong checker, ulong flagToSet, bool flagValue)
        {
            ulong checkerWithoutGivenFlag = checker & ~flagToSet;
            ulong flagSetter = flagValue ? flagToSet : 0; 
            
            return checkerWithoutGivenFlag | flagSetter;
        }
    }
}