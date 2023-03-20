using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePREFS : MonoBehaviour
{
    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
