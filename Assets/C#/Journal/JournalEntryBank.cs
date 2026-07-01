using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JournalEntryBank", menuName = "Journal/Entry Bank")]
public class JournalEntryBank : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public string id;      // nazwa wpisu (klucz)
        [TextArea(3, 10)]
        public string content; // treść wpisu
    }

    public List<Entry> entries;

    public string GetEntry(string id)
    {
        foreach (var e in entries)
        {
            if (e.id == id)
                return e.content;
        }

        Debug.LogWarning($"Journal entry not found: {id}");
        return null;
    }
}