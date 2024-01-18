using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersist
{
    void AddToPersistenceManager();
    void SaveState();
    void LoadState();
}
