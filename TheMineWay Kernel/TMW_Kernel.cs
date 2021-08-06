using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMW_Kernel : MonoBehaviour
{
    /* VALUES */
    public static string configData
    {
        get
        {
            return PlayerPrefs.GetString("tmw/config");
        }
        set {
            PlayerPrefs.SetString("tmw/config", value);
        }
    }

    /* COMPONENTS */
    public static TMW_Config.Configuration configuration; // Current configuration
    public static Version version = new Version(new int[] { 1, 0, 0 }, true);

    /* BEHAVIOURS */
    public class Version
    {
        private int[] _version;
        private bool _isBeta;

        public Version(bool isBeta = false)
        {
            _version = new int[] { 1, 0, 0 };
            _isBeta = isBeta;
        }
        public Version(int[] version, bool isBeta = false)
        {
            this._version = version;
            if (version.Length <= 0) this._version = new int[] { 1, 0, 0 };
            _isBeta = isBeta;
        }
        public GenericStatus compareWith(Version compare)
        {
            List<int> current = new List<int>();
            current.AddRange(_version);
            List<int> toCompare = new List<int>();
            toCompare.AddRange(compare.getRaw());

            while (current.Count < toCompare.Count) current.Add(0);
            while (current.Count > toCompare.Count) toCompare.Add(0);

            for (int i = 0; i < current.Count; i++)
            {
                if (current[i] > toCompare[i]) return GenericStatus.greater;
                if (current[i] < toCompare[i]) return GenericStatus.smaller;
            }
            return GenericStatus.equal;
        }
        public int[] getRaw()
        {
            return _version;
        }
        public bool isBeta()
        {
            return _isBeta;
        }

        /* Display */
        public string getInfo(bool includeIfBeta = true)
        {
            string toReturn = "";
            for (int i = 0; i < _version.Length; i++)
            {
                toReturn += _version[i];
                if (i < _version.Length - 1) toReturn += ".";
            }
            return (includeIfBeta ? $"BETA " : "") + toReturn;
        }
    }

    public enum GenericStatus
    {
        greater,
        smaller,
        equal
    }
}